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
    public class UnitProductRepository : Interface.Product.IUnitProductRepository
    {
        private readonly Database _context;
        private readonly ILogger<UnitProductRepository> _logger;

        public UnitProductRepository(Database context, ILogger<UnitProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        //******READ*******
        public async Task<IEnumerable<Unit_Product>> GetAll()
        {
            _logger.LogInformation("All Unit_Product have started to be received from the database.");

            var result = await _context.Unit_Product.ToListAsync();

            _logger.LogInformation("{Count} records received.", result.Count);
            return result;
        }
        public async Task<Unit_Product?> GetById(int id)
        {
            _logger.LogInformation("Request to receive Unit_Product  with ID: {Id}", id);

            var entity = await _context.Unit_Product.FindAsync(id);

            if (entity == null)
                _logger.LogWarning("Unit_Product  with ID: {Id} not found", id);
            else
                _logger.LogInformation("Unit_Product name with ID: {Id} successfully retrieved", id);

            return entity;
        }
        //******CRUD*****
        public async Task<string> Create(int UserId ,Unit_Product Unit_Product)
        {
            if (Unit_Product == null)
                return "داده ارسال نشده است.";
            try
            {
                _logger.LogInformation("Adding new Unit_Product: {@Unit_Product}", Unit_Product);
                bool nameExists = await _context.Unit_Product
                    .AsNoTracking()
                    .AnyAsync(i => i.Name == Unit_Product.Name);

                if (nameExists)
                {
                    _logger.LogWarning("Duplicate Unit_Product name: {Name}", Unit_Product.Name);
                    return "نام وارد شده تکراری است.";
                }

                var Logs = new BusinessEntity.Settings.LogUser();
                {
                    Logs.Description = $"ثبت واحد کالا با نام{Unit_Product.Name} ";
                    Logs.UserId = UserId;
                    Logs.Date = DateTime.UtcNow;
                }
                _logger.LogInformation("Request to add logs with Async: {Logs}", Logs);
                await _context.LogUser.AddAsync(Logs);
                Unit_Product.Id = 0;
                await _context.Unit_Product.AddAsync(Unit_Product);
                int result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    _logger.LogInformation("Unit_Product added successfully. ID: {Id}", Unit_Product.Id);
                    return "عملیات با موفقیت انجام شد.";
                }
                _logger.LogWarning("No changes saved when adding Unit_Product: {@Unit_Product}", Unit_Product);
                return "هیچ تغییری اعمال نشد.";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while adding Unit_Product: {@Unit_Product}", Unit_Product);
                return "خطایی در ذخیره اطلاعات رخ داد. لطفاً داده‌ها را بررسی کنید.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while adding Unit_Product: {@Unit_Product}", Unit_Product);
                return "خطای غیرمنتظره رخ داد. لطفاً با پشتیبانی تماس بگیرید.";
            }
        }
        public async Task<string> Update(int UserId ,Unit_Product Unit_Product)
        {
            try
            {
                _logger.LogInformation("Request update for Unit_Product: {@Unit_Product}", Unit_Product);

                if (Unit_Product == null)
                {
                    _logger.LogWarning("Null Unit_Product submitted.");
                    throw new ArgumentNullException(nameof(Unit_Product));
                }

                bool nameExists = await _context.Unit_Product
                    .AnyAsync(i => i.Name == Unit_Product.Name && i.Id != Unit_Product.Id);

                if (nameExists)
                {
                    _logger.LogWarning("Duplicate Unit_Product name: {Name}", Unit_Product.Name);
                    return "نام وارد شده تکراری است.";
                }

                var existing = await _context.Unit_Product.FindAsync(Unit_Product.Id);

                if (existing == null)
                {
                    _logger.LogWarning("Unit_Product with ID: {Id} not found.", Unit_Product.Id);
                    return "شناسه وارد شده با شناسه ذخیره در سیستم مطابقت ندارد.";
                }

                var Logs = new BusinessEntity.Settings.LogUser();
                {
                    Logs.Description = $"ویرایش واحد کالا با نام{Unit_Product.Name} ";
                    Logs.UserId = UserId;
                    Logs.Date = DateTime.UtcNow;
                }
                _logger.LogInformation("Request to add logs with Async: {Logs}", Logs);
                await _context.LogUser.AddAsync(Logs);
                _context.Entry(existing).CurrentValues.SetValues(Unit_Product);
                int result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    _logger.LogInformation("Unit_Product with ID: {Id} successfully updated", Unit_Product.Id);
                    return "عملیات با موفقیت انجام شد.";
                }

                _logger.LogWarning("No changes detected for Unit_Product with ID: {Id}", Unit_Product.Id);
                return "هیچ تغییری اعمال نشد.";
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error updating Unit_Product with ID: {Id}", Unit_Product?.Id);
                return "این رکورد توسط کاربر دیگری تغییر یافته است. لطفاً صفحه را رفرش کنید و مجدد تلاش کنید.";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error updating Unit_Product with ID: {Id}", Unit_Product?.Id);
                return "خطایی در ذخیره تغییرات رخ داد. لطفاً با پشتیبانی تماس بگیرید.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating Unit_Product");
                return "خطای غیرمنتظره رخ داد. لطفاً مجدداً تلاش کنید.";
            }
        }
        public async Task<string> Delete(int userId, int id)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("درخواست حذف واحد کالا با ID: {Id}", id);
                var entity = await _context.Unit_Product.FindAsync(id);

                if (entity == null)
                    return "واحد کالا مورد نظر یافت نشد.";

                if (entity.IsDelete == false)
                    return "امکان حذف این واحد کالا وجود ندارد.";

                bool hasRelatedAccounts = await _context.Product.AnyAsync(a => a.UnitProductId == id);
                if (hasRelatedAccounts)
                    return "امکان حذف این واحد کالا به دلیل وجود حساب‌های وابسته وجود ندارد.";

                // ثبت لاگ
                await _context.LogUser.AddAsync(new BusinessEntity.Settings.LogUser
                {
                    Description = $"حذف واحد کالا با نام {entity.Name}",
                    UserId = userId,
                    Date = DateTime.UtcNow
                });

                _context.Unit_Product.Remove(entity);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("واحد کالا با موفقیت حذف شد. ID: {Id}", id);
                return "عملیات با موفقیت انجام شد.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "خطا در حذف واحد کالا با ID: {Id}", id);
                return "خطایی در عملیات حذف رخ داد. لطفاً با پشتیبانی تماس بگیرید.";
            }
        }
    }
}
