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
    public class TypeProductRepository : Interface.Product.ITypeProductRepository
    {
        private readonly Database _context;
        private readonly ILogger<TypeProductRepository> _logger;

        public TypeProductRepository(Database context, ILogger<TypeProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        //******READ*******
        public async Task<IEnumerable<Type_Product>> GetAll()
        {
            _logger.LogInformation("All Type_Product have started to be received from the database.");

            var result = await _context.Type_Product.ToListAsync();

            _logger.LogInformation("{Count} records received.", result.Count);
            return result;
        }
        public async Task<Type_Product?> GetById(int id)
        {
            _logger.LogInformation("Request to receive Type_Product  with ID: {Id}", id);

            var entity = await _context.Type_Product.FindAsync(id);

            if (entity == null)
                _logger.LogWarning("Type_Product  with ID: {Id} not found", id);
            else
                _logger.LogInformation("Type_Product name with ID: {Id} successfully retrieved", id);

            return entity;
        }
        //******CRUD*****
        public async Task<string> Create(int userId, BusinessEntity.Product.Type_Product Type_Product)
        {
            if (Type_Product == null)
                return "داده ارسال نشده است.";
            try
            {
                _logger.LogInformation("Starting creation of Type_Product: {@Type_Product}", Type_Product);

                bool nameExists = await _context.Type_Product
                    .AnyAsync(b => b.Name.Trim().ToLower() == Type_Product.Name.Trim().ToLower());
                if (nameExists)
                {
                    _logger.LogWarning("Duplicate Type_Product name: {Name}", Type_Product.Name);
                    return "نام وارد شده تکراری است.";
                }

                var log = new BusinessEntity.Settings.LogUser
                {
                    Description = $"ثبت نوع کالا با نام {Type_Product.Name}",
                    UserId = userId,
                    Date = DateTime.UtcNow
                };

                await _context.LogUser.AddAsync(log);

                Type_Product.Id = 0;
                await _context.Type_Product.AddAsync(Type_Product);

                _logger.LogInformation("Tracked entries before SaveChanges: {Count}", _context.ChangeTracker.Entries().Count());

                int saved = await _context.SaveChangesAsync();

                if (saved > 0)
                {
                    _logger.LogInformation("BankT created successfully with ID: {Id}", Type_Product.Id);
                    return $"عملیات با موفقیت انجام شد. ";
                }

                _logger.LogWarning("No changes were saved for Type_Product: {@Type_Product}", Type_Product);
                return "هیچ تغییری اعمال نشد.";
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database error while creating Type_Product: {@Type_Product}", Type_Product);
                return "خطا در ذخیره‌سازی اطلاعات رخ داد. لطفاً داده‌ها را بررسی کنید.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating Type_Product: {@Type_Product}", Type_Product);
                return "خطای غیرمنتظره رخ داد. لطفاً با پشتیبانی تماس بگیرید.";
            }
        }
        public async Task<string> Update(int UserId ,Type_Product Type_Product)
        {
            try
            {
                _logger.LogInformation("Request update for Type_Product: {@Type_Product}", Type_Product);

                if (Type_Product == null)
                {
                    _logger.LogWarning("Null Type_Product submitted.");
                    throw new ArgumentNullException(nameof(Type_Product));
                }

                bool nameExists = await _context.Type_Product
                    .AnyAsync(i => i.Name == Type_Product.Name && i.Id != Type_Product.Id);

                if (nameExists)
                {
                    _logger.LogWarning("Duplicate Type_Product name: {Name}", Type_Product.Name);
                    return "نام وارد شده تکراری است.";
                }

                var existing = await _context.Type_Product.FindAsync(Type_Product.Id);

                if (existing == null)
                {
                    _logger.LogWarning("Type_Product with ID: {Id} not found.", Type_Product.Id);
                    return "شناسه وارد شده با شناسه ذخیره در سیستم مطابقت ندارد.";
                }

                var Logs = new BusinessEntity.Settings.LogUser();
                {
                    Logs.Description = $"ثبت نوع کالا با نام{Type_Product.Name} ";
                    Logs.UserId = UserId;
                    Logs.Date = DateTime.UtcNow;
                }
                _logger.LogInformation("Request to add logs with Async: {Logs}", Logs);
                await _context.LogUser.AddAsync(Logs);
                _context.Entry(existing).CurrentValues.SetValues(Type_Product);
                int result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    _logger.LogInformation("Type_Product with ID: {Id} successfully updated", Type_Product.Id);
                    return "عملیات با موفقیت انجام شد.";
                }

                _logger.LogWarning("No changes detected for Type_Product with ID: {Id}", Type_Product.Id);
                return "هیچ تغییری اعمال نشد.";
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error updating Type_Product with ID: {Id}", Type_Product?.Id);
                return "این رکورد توسط کاربر دیگری تغییر یافته است. لطفاً صفحه را رفرش کنید و مجدد تلاش کنید.";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error updating Type_Product with ID: {Id}", Type_Product?.Id);
                return "خطایی در ذخیره تغییرات رخ داد. لطفاً با پشتیبانی تماس بگیرید.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating Type_Product");
                return "خطای غیرمنتظره رخ داد. لطفاً مجدداً تلاش کنید.";
            }
        }
        public async Task<string> Delete(int userId, int id)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("درخواست حذف نوع کالا با ID: {Id}", id);
                var entity = await _context.Type_Product.FindAsync(id);

                if (entity == null)
                    return "نوع کالا مورد نظر یافت نشد.";

                if (entity.IsDelete == false)
                    return "امکان حذف این نوع کالا وجود ندارد.";

                bool hasRelatedAccounts = await _context.Product.AnyAsync(a => a.TypeProductId == id);
                if (hasRelatedAccounts)
                    return "امکان حذف این نوع کالا به دلیل وجود حساب‌های وابسته وجود ندارد.";

                // ثبت لاگ
                await _context.LogUser.AddAsync(new BusinessEntity.Settings.LogUser
                {
                    Description = $"حذف واحد کالا با نام {entity.Name}",
                    UserId = userId,
                    Date = DateTime.UtcNow
                });

                _context.Type_Product.Remove(entity);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("نوع کالا با موفقیت حذف شد. ID: {Id}", id);
                return "عملیات با موفقیت انجام شد.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "خطا در حذف نوع کالا با ID: {Id}", id);
                return "خطایی در عملیات حذف رخ داد. لطفاً با پشتیبانی تماس بگیرید.";
            }
        }
    }
}
