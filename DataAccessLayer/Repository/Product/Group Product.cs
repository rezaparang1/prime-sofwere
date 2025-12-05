using BusinessEntity.People;
using DataAccessLayer.Interface.Product;
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
    public class GroupProductRepository : Interface.Product.IGroupProductRepository
    {
        private readonly Database _context;
        private readonly ILogger<GroupProductRepository> _logger;

        public GroupProductRepository(Database context, ILogger<GroupProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        //******READ*******
        public async Task<IEnumerable<BusinessEntity.Product.Group_Product>> GetAll()
        {
            _logger.LogInformation("All Group_Product have started to be received from the database.");
            var result = await _context.Group_Product.ToListAsync();
            _logger.LogInformation("{Count} records received.", result.Count);
            return result;
        }
        public async Task<BusinessEntity.Product.Group_Product?> GetById(int id)
        {
            _logger.LogInformation("Request to receive Group_Product with ID: {Id}", id);

            var entity = await _context.Group_Product.FindAsync(id);

            if (entity == null)
                _logger.LogWarning("Group_Product with ID: {Id} not found", id);
            else
                _logger.LogInformation("Group_Product name with ID: {Id} successfully retrieved", id);

            return entity;
        }
        //******CRUD*****
        public async Task<ServiceResult> Create(int UserId, BusinessEntity.Product.Group_Product Group_Product)
        {
            if (Group_Product == null)
                return new ServiceResult(false, "داده ارسال نشده است.");
         
            try
            {
                _logger.LogInformation("Starting creation of Group_Product: {@Group_Product}", Group_Product);

                bool nameExists = await _context.Group_Product
                    .AnyAsync(b => b.Name.Trim().ToLower() == Group_Product.Name.Trim().ToLower());

                if (nameExists)
                {
                    _logger.LogWarning("Duplicate Group_Product name: {Name}", Group_Product.Name);
                    return new ServiceResult(false, "نام وارد شده تکراری است.");
                }

                var log = new BusinessEntity.Settings.LogUser
                {
                    Description = $"ثبت بانک با نام {Group_Product.Name}",
                    UserId = UserId,
                    Date = DateTime.UtcNow
                };
                await _context.LogUser.AddAsync(log);

                Group_Product.Id = 0;
                await _context.Group_Product.AddAsync(Group_Product);

                _logger.LogInformation("Tracked entries before SaveChanges: {Count}", _context.ChangeTracker.Entries().Count());

                int saved = await _context.SaveChangesAsync();

                if (saved > 0)
                {
                    _logger.LogInformation("Group_Product created successfully with ID: {Id}", Group_Product.Id);
                    return new ServiceResult(true, "عملیات با موفقیت انجام شد.");
                }

                _logger.LogWarning("No changes were saved for Group_Product: {@Group_Product}", Group_Product);
                return new ServiceResult(false, "هیچ تغییری اعمال نشد.");
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database error while creating Group_Product: {@Group_Product}", Group_Product);
                return new ServiceResult(false, "خطا در ذخیره‌سازی اطلاعات رخ داد. لطفاً داده‌ها را بررسی کنید.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating Group_Product: {@Group_Product}", Group_Product);
                return new ServiceResult(false, "خطای غیرمنتظره رخ داد. لطفاً با پشتیبانی تماس بگیرید.");
            }
        }
        public async Task<ServiceResult> Update(int UserId, BusinessEntity.Product.Group_Product Group_Product)
        {
            if (Group_Product == null)
            {
                _logger.LogWarning("Null Group_Product submitted.");
                throw new ArgumentNullException(nameof(Group_Product));
            }

            try
            {
                _logger.LogInformation("Request update for Group_Product: {@Group_Product}", Group_Product);

                bool nameExists = await _context.Group_Product
                    .AnyAsync(i => i.Name.Trim().ToLower() == Group_Product.Name.Trim().ToLower() && i.Id != Group_Product.Id);

                if (nameExists)
                {
                    _logger.LogWarning("Duplicate Group_Product name: {Name}", Group_Product.Name);
                    return new ServiceResult(false, "نام وارد شده تکراری است.");
                }

                var log = new BusinessEntity.Settings.LogUser
                {
                    Description = $"ویرایش گروه کالا با نام {Group_Product.Name}",
                    UserId = UserId,
                    Date = DateTime.UtcNow
                };
                await _context.LogUser.AddAsync(log);

                var existing = await _context.Group_Product.FindAsync(Group_Product.Id);
                if (existing == null)
                {
                    _logger.LogWarning("Group_Product with ID: {Id} not found.", Group_Product.Id);
                    return new ServiceResult(false, "شناسه وارد شده با شناسه ذخیره در سیستم مطابقت ندارد.");
                }

                _context.Entry(existing).CurrentValues.SetValues(Group_Product);
                int result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    _logger.LogInformation("Group_Product with ID: {Id} successfully updated", Group_Product.Id);
                    return new ServiceResult(true, "عملیات با موفقیت انجام شد.");
                }

                _logger.LogWarning("No changes detected for Group_Product with ID: {Id}", Group_Product.Id);
                return new ServiceResult(false, "هیچ تغییری اعمال نشد.");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error updating Group_Product with ID: {Id}", Group_Product.Id);
                return new ServiceResult(false, "این رکورد توسط کاربر دیگری تغییر یافته است. لطفاً صفحه را رفرش کنید و مجدد تلاش کنید.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error updating Group_Product with ID: {Id}", Group_Product.Id);
                return new ServiceResult(false, "خطایی در ذخیره تغییرات رخ داد. لطفاً با پشتیبانی تماس بگیرید.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating Group_Product");
                return new ServiceResult(false, "خطای غیرمنتظره رخ داد. لطفاً مجدداً تلاش کنید.");
            }
        }
        public async Task<ServiceResult> Delete(int userId, int id)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("درخواست حذف گروه کالا با ID: {Id}", id);
                var entity = await _context.Group_Product.FindAsync(id);

                if (entity == null)
                    return new ServiceResult(false, "گروه کالا مورد نظر یافت نشد.");

                if (entity.IsDelete == false)
                    return new ServiceResult(false, "امکان حذف این گروه کالا وجود ندارد.");

                bool hasRelatedAccounts = await _context.Product.AnyAsync(a => a.GroupProductId == id);
                if (hasRelatedAccounts)
                    return new ServiceResult(false, "امکان حذف این گروه کالا به دلیل وجود حساب‌های وابسته وجود ندارد.");

                // ثبت لاگ
                await _context.LogUser.AddAsync(new BusinessEntity.Settings.LogUser
                {
                    Description = $"حذف گروه کالا با نام {entity.Name}",
                    UserId = userId,
                    Date = DateTime.UtcNow
                });

                _context.Group_Product.Remove(entity);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("گروه کالا با موفقیت حذف شد. ID: {Id}", id);
                return new ServiceResult(true, "عملیات با موفقیت انجام شد.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "خطا در حذف گروه کالا با ID: {Id}", id);
                return new ServiceResult(false, "خطایی در عملیات حذف رخ داد. لطفاً با پشتیبانی تماس بگیرید.");
            }
        }
    }

}
