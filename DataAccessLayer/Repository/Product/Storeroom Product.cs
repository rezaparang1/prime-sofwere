using BusinessEntity.Product;
using DataAccessLayer.Repository.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Product
{
    public class StoreroomProductRepository : Interface.Product.IStoreroomProductRepository
    {
        private readonly Database _context;
        private readonly ILogger<StoreroomProductRepository> _logger;

        public StoreroomProductRepository(Database context, ILogger<StoreroomProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        //*****SEARCH*****
        public async Task<List<StoreroomProductDto>> Search(
       string? Name = null,
       string? Description = null,
       int? IdSection = null)
        {
            _logger.LogInformation(
                "Searching for name: {Name} description: {Description} sectionId: {IdSection}",
                Name, Description, IdSection);

            var query = _context.Storeroom_Product
                .AsNoTracking()
                .Include(x => x.Section_Product)
                .Include(x => x.People)
                .AsQueryable();

            if (!string.IsNullOrEmpty(Name))
                query = query.Where(r => r.Name.Contains(Name));

            if (!string.IsNullOrEmpty(Description))
                query = query.Where(r => r.Description.Contains(Description));

            if (IdSection.HasValue)
                query = query.Where(r => r.SectionProductId == IdSection.Value);

            var result = await query.ToListAsync();

            _logger.LogInformation(
                "{Count} results found for search Async: {Name} {Description} {IdSection}",
                result.Count, Name, Description, IdSection);

            return result.Select(x => x.ToDto()).ToList();
        }

        //******READ*****
        public async Task<IEnumerable<StoreroomProductDto>> GetAll()
        {
            var result = await _context.Storeroom_Product
                .AsNoTracking()
                .Include(x => x.Section_Product)
                .Include(x => x.People)
                .ToListAsync();

            return result.Select(x => x.ToDto());
        }
        public async Task<StoreroomProductDto?> GetById(int id)
        {
            var entity = await _context.Storeroom_Product
                .AsNoTracking()
                .Include(x => x.Section_Product)
                .Include(x => x.People)
                .FirstOrDefaultAsync(x => x.Id == id);

            return entity?.ToDto();
        }

        //******CRUD*****
        public async Task<string> Create(int UserId, Storeroom_Product storeroom_Product)
        {
            if (storeroom_Product == null) throw new ArgumentNullException(nameof(storeroom_Product));

            try
            {
                _logger.LogInformation("Adding new Storeroom_Product: {@Storeroom_Product}", storeroom_Product);

                bool nameExists = await _context.Storeroom_Product
                    .AsNoTracking()
                    .AnyAsync(i => i.Name == storeroom_Product.Name);

                if (nameExists)
                {
                    _logger.LogWarning("Duplicate Storeroom_Product name: {Name}", storeroom_Product.Name);
                    return "نام وارد شده تکراری است.";
                }

                // استفاده از تراکنش تا لاگ و افزودن رکورد همزمان باشند
                await using var transaction = await _context.Database.BeginTransactionAsync();

                var log = new BusinessEntity.Settings.LogUser
                {
                    Description = $"ثبت انبار با نام {storeroom_Product.Name}",
                    UserId = UserId,
                    Date = DateTime.UtcNow
                };

                _logger.LogInformation("Adding LogUser: {@Log}", log);
                await _context.LogUser.AddAsync(log);

                await _context.Storeroom_Product.AddAsync(storeroom_Product);

                int result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    await transaction.CommitAsync();
                    _logger.LogInformation("Storeroom_Product added successfully. ID: {Id}", storeroom_Product.Id);
                    return "عملیات با موفقیت انجام شد.";
                }

                // اگر هیچ تغییری اعمال نشد rollback می‌کنیم
                await transaction.RollbackAsync();
                _logger.LogWarning("No changes saved when adding Storeroom_Product: {@Storeroom_Product}", storeroom_Product);
                return "هیچ تغییری اعمال نشد.";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while adding Storeroom_Product: {@Storeroom_Product}", storeroom_Product);
                return "خطایی در ذخیره اطلاعات رخ داد. لطفاً داده‌ها را بررسی کنید.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while adding Storeroom_Product: {@Storeroom_Product}", storeroom_Product);
                return "خطای غیرمنتظره رخ داد. لطفاً با پشتیبانی تماس بگیرید.";
            }
        }
        public async Task<string> Update(int UserId, Storeroom_Product storeroom_Product)
        {
            if (storeroom_Product == null) throw new ArgumentNullException(nameof(storeroom_Product));

            try
            {
                _logger.LogInformation("Request update for Storeroom_Product: {@Storeroom_Product}", storeroom_Product);

                bool nameExists = await _context.Storeroom_Product
                    .AsNoTracking()
                    .AnyAsync(i => i.Name == storeroom_Product.Name && i.Id != storeroom_Product.Id);

                if (nameExists)
                {
                    _logger.LogWarning("Duplicate Storeroom_Product name: {Name}", storeroom_Product.Name);
                    return "نام وارد شده تکراری است.";
                }

                var existing = await _context.Storeroom_Product.FindAsync(storeroom_Product.Id);
                if (existing == null)
                {
                    _logger.LogWarning("Storeroom_Product with ID: {Id} not found.", storeroom_Product.Id);
                    return "شناسه وارد شده با شناسه ذخیره در سیستم مطابقت ندارد.";
                }

                await using var transaction = await _context.Database.BeginTransactionAsync();

                var log = new BusinessEntity.Settings.LogUser
                {
                    Description = $"ویرایش انبار با نام {storeroom_Product.Name}",
                    UserId = UserId,
                    Date = DateTime.UtcNow
                };

                _logger.LogInformation("Adding LogUser: {@Log}", log);
                await _context.LogUser.AddAsync(log);

                // به جای SetValues که می‌تواند navigation ها را خراب کند، فقط فیلدهای ساده را مقداردهی می‌کنیم
                existing.Name = storeroom_Product.Name;
                existing.Description = storeroom_Product.Description;
                existing.Address = storeroom_Product.Address;
                existing.SectionProductId = storeroom_Product.SectionProductId;
                existing.PeopleId = storeroom_Product.PeopleId;
                existing.NegativeBalancePolicy = storeroom_Product.NegativeBalancePolicy;
                // اگر نیاز به update collection ها هست، آن‌ها را مدیریت جداگانه انجام دهید

                int result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    await transaction.CommitAsync();
                    _logger.LogInformation("Storeroom_Product with ID: {Id} successfully updated", storeroom_Product.Id);
                    return "عملیات با موفقیت انجام شد.";
                }

                await transaction.RollbackAsync();
                _logger.LogWarning("No changes detected for Storeroom_Product with ID: {Id}", storeroom_Product.Id);
                return "هیچ تغییری اعمال نشد.";
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error updating Storeroom_Product with ID: {Id}", storeroom_Product?.Id);
                return "این رکورد توسط کاربر دیگری تغییر یافته است. لطفاً صفحه را رفرش کنید و مجدد تلاش کنید.";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error updating Storeroom_Product with ID: {Id}", storeroom_Product?.Id);
                return "خطایی در ذخیره تغییرات رخ داد. لطفاً با پشتیبانی تماس بگیرید.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating Storeroom_Product");
                return "خطای غیرمنتظره رخ داد. لطفاً مجدداً تلاش کنید.";
            }
        }
        public async Task<string> Delete(int UserId, int id)
        {
            try
            {
                _logger.LogInformation("Request to delete Storeroom_Product with ID: {Id}", id);

                var entity = await _context.Storeroom_Product.FindAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("Storeroom_Product with ID: {Id} not found for deletion.", id);
                    return "درخواست مورد نظر شما یافت نشد .";
                }

                bool hasRelatedProducts = await _context.Product.AnyAsync(b => b.StoreroomProductId == id);
                if (hasRelatedProducts)
                {
                    _logger.LogWarning("Storeroom_Product with ID: {Id} can't be deleted due to related products.", id);
                    return "امکان حذف این رکورد به دلیل وابستگی های موجود وجود ندارد.";
                }

                await using var transaction = await _context.Database.BeginTransactionAsync();

                var log = new BusinessEntity.Settings.LogUser
                {
                    Description = $"حذف انبار با نام {entity.Name}",
                    UserId = UserId,
                    Date = DateTime.UtcNow
                };
                _logger.LogInformation("Adding LogUser: {@Log}", log);
                await _context.LogUser.AddAsync(log);

                _context.Storeroom_Product.Remove(entity);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    await transaction.CommitAsync();
                    _logger.LogInformation("Storeroom_Product with ID: {Id} was successfully deleted.", id);
                    return "عملیات با موفقیت انجام شد .";
                }

                await transaction.RollbackAsync();
                _logger.LogWarning("Failed to delete Storeroom_Product with ID: {Id}", id);
                return "عملیات حذف انجام نشد. لطفاً مجدداً تلاش کنید.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Storeroom_Product with ID: {Id}", id);
                return "خطایی در عملیات مورد نظر رخ داد. لطفاً با پشتیبانی تماس بگیرید.";
            }
        }
    }
}
