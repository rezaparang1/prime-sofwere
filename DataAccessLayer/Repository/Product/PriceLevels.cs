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
    public class PriceLevelRepository : Interface.Product.IPriceLevelsRepository
    {
        private readonly Database _context;
        private readonly ILogger<PriceLevelRepository> _logger;

        public PriceLevelRepository(Database context, ILogger<PriceLevelRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        //******READ*******
        public async Task<IEnumerable<PriceLevels>> GetAll()
        {
            _logger.LogInformation("All PriceLevels have started to be received from the database.");

            var result = await _context.PriceLevels.ToListAsync();

            _logger.LogInformation("{Count} records received.", result.Count);
            return result;
        }
        public async Task<PriceLevels?> GetById(int id)
        {
            _logger.LogInformation("Request to receive PriceLevels  with ID: {Id}", id);

            var entity = await _context.PriceLevels.FindAsync(id);

            if (entity == null)
                _logger.LogWarning("PriceLevels  with ID: {Id} not found", id);
            else
                _logger.LogInformation("PriceLevels name with ID: {Id} successfully retrieved", id);

            return entity;
        }
        //******CRUD*****
        public async Task<string> Create(int UserId, BusinessEntity.Product.PriceLevels PriceLevels)
        {
            if (PriceLevels == null)
                return "داده ارسال نشده است.";
            try
            {
                _logger.LogInformation("Adding new PriceLevels: {@PriceLevels}", PriceLevels);
                bool nameExists = await _context.PriceLevels
                    .AsNoTracking()
                    .AnyAsync(i => i.Name == PriceLevels.Name);

                if (nameExists)
                {
                    _logger.LogWarning("Duplicate PriceLevels name: {Name}", PriceLevels.Name);
                    return "نام وارد شده تکراری است.";
                }

                var Logs = new BusinessEntity.Settings.LogUser();
                {
                    Logs.Description = $"ثبت سطح قیمت با نام{PriceLevels.Name} ";
                    Logs.UserId = UserId;
                    Logs.Date = DateTime.UtcNow;
                }
                _logger.LogInformation("Request to add logs with Async: {Logs}", Logs);
                await _context.LogUser.AddAsync(Logs);
                PriceLevels.Id = 0;
                await _context.PriceLevels.AddAsync(PriceLevels);
                int result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    _logger.LogInformation("PriceLevels added successfully. ID: {Id}", PriceLevels.Id);
                    return "عملیات با موفقیت انجام شد.";
                }
                _logger.LogWarning("No changes saved when adding PriceLevels: {@PriceLevels}", PriceLevels);
                return "هیچ تغییری اعمال نشد.";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while adding PriceLevels: {@PriceLevels}", PriceLevels);
                return "خطایی در ذخیره اطلاعات رخ داد. لطفاً داده‌ها را بررسی کنید.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while adding PriceLevels: {@PriceLevels}", PriceLevels);
                return "خطای غیرمنتظره رخ داد. لطفاً با پشتیبانی تماس بگیرید.";
            }
        }
        public async Task<string> Update(int UserId, BusinessEntity.Product.PriceLevels PriceLevels)
        {
            try
            {
                _logger.LogInformation("Request update for PriceLevels: {@PriceLevels}", PriceLevels);

                if (PriceLevels == null)
                {
                    _logger.LogWarning("Null PriceLevels submitted.");
                    throw new ArgumentNullException(nameof(PriceLevels));
                }

                bool nameExists = await _context.PriceLevels
                    .AnyAsync(i => i.Name == PriceLevels.Name && i.Id != PriceLevels.Id);

                if (nameExists)
                {
                    _logger.LogWarning("Duplicate PriceLevels name: {Name}", PriceLevels.Name);
                    return "نام وارد شده تکراری است.";
                }

                var existing = await _context.PriceLevels.FindAsync(PriceLevels.Id);

                if (existing == null)
                {
                    _logger.LogWarning("PriceLevels with ID: {Id} not found.", PriceLevels.Id);
                    return "شناسه وارد شده با شناسه ذخیره در سیستم مطابقت ندارد.";
                }

                var Logs = new BusinessEntity.Settings.LogUser();
                {
                    Logs.Description = $"ویرایش سطح قیمت با نام{PriceLevels.Name} ";
                    Logs.UserId = UserId;
                    Logs.Date = DateTime.UtcNow;
                }
                _logger.LogInformation("Request to add logs with Async: {Logs}", Logs);
                await _context.LogUser.AddAsync(Logs);
                _context.Entry(existing).CurrentValues.SetValues(PriceLevels);
                int result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    _logger.LogInformation("PriceLevels with ID: {Id} successfully updated", PriceLevels.Id);
                    return "عملیات با موفقیت انجام شد.";
                }

                _logger.LogWarning("No changes detected for PriceLevels with ID: {Id}", PriceLevels.Id);
                return "هیچ تغییری اعمال نشد.";
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error updating PriceLevels with ID: {Id}", PriceLevels?.Id);
                return "این رکورد توسط کاربر دیگری تغییر یافته است. لطفاً صفحه را رفرش کنید و مجدد تلاش کنید.";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error updating PriceLevels with ID: {Id}", PriceLevels?.Id);
                return "خطایی در ذخیره تغییرات رخ داد. لطفاً با پشتیبانی تماس بگیرید.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating PriceLevels");
                return "خطای غیرمنتظره رخ داد. لطفاً مجدداً تلاش کنید.";
            }
        }
        public async Task<string> Delete(int userId, int id)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("درخواست حذف سطح قیمت با ID: {Id}", id);
                var entity = await _context.PriceLevels.FindAsync(id);

                if (entity == null)
                    return "سطح قیمت مورد نظر یافت نشد.";

                if (entity.IsDelete == false)
                    return "امکان حذف این سطح قیمت وجود ندارد.";

                bool hasRelatedAccounts = await _context.People.AnyAsync(a => a.PriceLevelID == id);
                if (hasRelatedAccounts)
                    return "امکان حذف این سطح قیمت به دلیل وجود حساب‌های وابسته وجود ندارد.";

                // ثبت لاگ
                await _context.LogUser.AddAsync(new BusinessEntity.Settings.LogUser
                {
                    Description = $"حذف سطح قیمت با نام {entity.Name}",
                    UserId = userId,
                    Date = DateTime.UtcNow
                });

                _context.PriceLevels.Remove(entity);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("سطح قیمت با موفقیت حذف شد. ID: {Id}", id);
                return "عملیات با موفقیت انجام شد.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "خطا در حذف سطح قیمت با ID: {Id}", id);
                return "خطایی در عملیات حذف رخ داد. لطفاً با پشتیبانی تماس بگیرید.";
            }
        }
    }
}
