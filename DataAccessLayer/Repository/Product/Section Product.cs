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
    public class SectionProductRepository : Interface.Product.ISectionProductRepository
    {
        private readonly Database _context;
        private readonly ILogger<SectionProductRepository> _logger;

        public SectionProductRepository(Database context, ILogger<SectionProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        //******READ*******
        public async Task<IEnumerable<Section_Product>> GetAll()
        {
            _logger.LogInformation("All Section_Product have started to be received from the database.");

            var result = await _context.Section_Product.ToListAsync();

            _logger.LogInformation("{Count} records received.", result.Count);
            return result;
        }
        public async Task<Section_Product?> GetById(int id)
        {
            _logger.LogInformation("Request to receive Section_Product  with ID: {Id}", id);

            var entity = await _context.Section_Product.FindAsync(id);

            if (entity == null)
                _logger.LogWarning("Section_Product  with ID: {Id} not found", id);
            else
                _logger.LogInformation("Section_Product name with ID: {Id} successfully retrieved", id);

            return entity;
        }
        //******CRUD*****
        public async Task<string> Create(int UserId ,Section_Product Section_Product)
        {
            if (Section_Product == null)
                return "داده ارسال نشده است.";
            try
            {
                _logger.LogInformation("Adding new Section_Product: {@Section_Product}", Section_Product);
                bool nameExists = await _context.Section_Product
                    .AsNoTracking()
                    .AnyAsync(i => i.Name == Section_Product.Name);

                if (nameExists)
                {
                    _logger.LogWarning("Duplicate Section_Product name: {Name}", Section_Product.Name);
                    return "نام وارد شده تکراری است.";
                }

                var Logs = new BusinessEntity.Settings.LogUser();
                {
                    Logs.Description = $"ثبت بخش کالا با نام{Section_Product.Name} ";
                    Logs.UserId = UserId;
                    Logs.Date = DateTime.UtcNow;
                }
                _logger.LogInformation("Request to add logs with Async: {Logs}", Logs);
                await _context.LogUser.AddAsync(Logs);
                Section_Product.Id = 0;
                await _context.Section_Product.AddAsync(Section_Product);
                int result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    _logger.LogInformation("Section_Product added successfully. ID: {Id}", Section_Product.Id);
                    return "عملیات با موفقیت انجام شد.";
                }
                _logger.LogWarning("No changes saved when adding Section_Product: {@Section_Product}", Section_Product);
                return "هیچ تغییری اعمال نشد.";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while adding Section_Product: {@Section_Product}", Section_Product);
                return "خطایی در ذخیره اطلاعات رخ داد. لطفاً داده‌ها را بررسی کنید.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while adding Section_Product: {@Section_Product}", Section_Product);
                return "خطای غیرمنتظره رخ داد. لطفاً با پشتیبانی تماس بگیرید.";
            }
        }
        public async Task<string> Update(int UserId , Section_Product Section_Product)
        {
            try
            {
                _logger.LogInformation("Request update for Section_Product: {@Section_Product}", Section_Product);

                if (Section_Product == null)
                {
                    _logger.LogWarning("Null Section_Product submitted.");
                    throw new ArgumentNullException(nameof(Section_Product));
                }

                bool nameExists = await _context.Section_Product
                    .AnyAsync(i => i.Name == Section_Product.Name && i.Id != Section_Product.Id);

                if (nameExists)
                {
                    _logger.LogWarning("Duplicate Section_Product name: {Name}", Section_Product.Name);
                    return "نام وارد شده تکراری است.";
                }

                var existing = await _context.Section_Product.FindAsync(Section_Product.Id);

                if (existing == null)
                {
                    _logger.LogWarning("Section_Product with ID: {Id} not found.", Section_Product.Id);
                    return "شناسه وارد شده با شناسه ذخیره در سیستم مطابقت ندارد.";
                }

                var Logs = new BusinessEntity.Settings.LogUser();
                {
                    Logs.Description = $"ویرایش بخش کالا با نام{Section_Product.Name} ";
                    Logs.UserId = UserId;
                    Logs.Date = DateTime.UtcNow;
                }
                _logger.LogInformation("Request to add logs with Async: {Logs}", Logs);
                await _context.LogUser.AddAsync(Logs);
                _context.Entry(existing).CurrentValues.SetValues(Section_Product);
                int result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    _logger.LogInformation("Section_Product with ID: {Id} successfully updated", Section_Product.Id);
                    return "عملیات با موفقیت انجام شد.";
                }

                _logger.LogWarning("No changes detected for Section_Product with ID: {Id}", Section_Product.Id);
                return "هیچ تغییری اعمال نشد.";
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error updating Section_Product with ID: {Id}", Section_Product?.Id);
                return "این رکورد توسط کاربر دیگری تغییر یافته است. لطفاً صفحه را رفرش کنید و مجدد تلاش کنید.";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error updating Section_Product with ID: {Id}", Section_Product?.Id);
                return "خطایی در ذخیره تغییرات رخ داد. لطفاً با پشتیبانی تماس بگیرید.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating Section_Product");
                return "خطای غیرمنتظره رخ داد. لطفاً مجدداً تلاش کنید.";
            }
        }
        public async Task<string> Delete(int userId, int id)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("درخواست حذف بخش کالا با ID: {Id}", id);
                var entity = await _context.Section_Product.FindAsync(id);

                if (entity == null)
                    return "بخش کالا مورد نظر یافت نشد.";

                if (entity.IsDelete == false)
                    return "امکان حذف این بخش کالا وجود ندارد.";

                bool hasRelatedAccounts = await _context.Product.AnyAsync(a => a.SectionProductId == id);
                if (hasRelatedAccounts)
                    return "امکان حذف این بخش کالا به دلیل وجود حساب‌های وابسته وجود ندارد.";

                // ثبت لاگ
                await _context.LogUser.AddAsync(new BusinessEntity.Settings.LogUser
                {
                    Description = $"حذف بخش کالا با نام{entity.Name}",
                    UserId = userId,
                    Date = DateTime.UtcNow
                });

                _context.Section_Product.Remove(entity);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("بخش کالا با موفقیت حذف شد. ID: {Id}", id);
                return "عملیات با موفقیت انجام شد.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "خطا در بخش نوع کالا با ID: {Id}", id);
                return "خطایی در عملیات حذف رخ داد. لطفاً با پشتیبانی تماس بگیرید.";
            }
        }
    }
}
