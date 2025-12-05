using DataAccessLayer.Interface.Bank;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity;

namespace DataAccessLayer.Repository.Bank
{
    public class DefinitionBankRepository : Interface.Bank.IDefinitionBankRepository
    {
        private readonly Database _context;
        private readonly ILogger<DefinitionBankRepository> _logger;

        public DefinitionBankRepository(Database context, ILogger<DefinitionBankRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        //******READ******
        public async Task<IEnumerable<BusinessEntity.Bank.Definition_Bank>> GetAll()
        {
            _logger.LogInformation("شروع دریافت تمامی بانک‌ها از دیتابیس.");
            var result = await _context.Definition_Bank.ToListAsync();
            _logger.LogInformation("{Count} رکورد دریافت شد.", result.Count);
            return result;
        }
        public async Task<BusinessEntity.Bank.Definition_Bank?> GetById(int id)
        {
            _logger.LogInformation("درخواست دریافت بانک با ID: {Id}", id);
            var entity = await _context.Definition_Bank.FindAsync(id);
            if (entity == null)
                _logger.LogWarning("بانک با ID: {Id} یافت نشد.", id);
            else
                _logger.LogInformation("بانک با ID: {Id} با موفقیت واکشی شد.", id);
            return entity;
        }
        //******CRUD******
        public async Task<string> Create(int userId, BusinessEntity.Bank.Definition_Bank bank)
        {
            if (bank == null)
                return "داده ارسال نشده است.";

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("شروع ایجاد بانک جدید: {@Bank}", bank);

                bool nameExists = await _context.Definition_Bank
                    .AnyAsync(b => b.Name.Trim().ToLower() == bank.Name.Trim().ToLower());
                if (nameExists)
                    return "نام وارد شده تکراری است.";

                // ثبت لاگ کاربر
                await _context.LogUser.AddAsync(new BusinessEntity.Settings.LogUser
                {
                    Description = $"ثبت بانک با نام {bank.Name}",
                    UserId = userId,
                    Date = DateTime.UtcNow
                });

                bank.Id = 0;
                await _context.Definition_Bank.AddAsync(bank);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("بانک با موفقیت ایجاد شد. ID: {Id}", bank.Id);
                return "عملیات با موفقیت انجام شد.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "خطا در ایجاد بانک: {@Bank}", bank);
                return "خطایی در ذخیره‌سازی اطلاعات رخ داد. لطفاً داده‌ها را بررسی کنید.";
            }
        }
        public async Task<string> Update(int userId, BusinessEntity.Bank.Definition_Bank bank)
        {
            if (bank == null)
                return "داده ارسال نشده است.";

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("در حال بروزرسانی بانک: {@Bank}", bank);

                bool nameExists = await _context.Definition_Bank
                    .AnyAsync(b => b.Name.Trim().ToLower() == bank.Name.Trim().ToLower() && b.Id != bank.Id);
                if (nameExists)
                    return "نام وارد شده تکراری است.";

                var existing = await _context.Definition_Bank.FindAsync(bank.Id);
                if (existing == null)
                    return "بانک مورد نظر یافت نشد.";

                // بروزرسانی فیلدها
                existing.Name = bank.Name;
                existing.IsDelete = bank.IsDelete;

                // ثبت لاگ کاربر
                await _context.LogUser.AddAsync(new BusinessEntity.Settings.LogUser
                {
                    Description = $"ویرایش بانک با نام {bank.Name}",
                    UserId = userId,
                    Date = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("بانک با موفقیت بروزرسانی شد. ID: {Id}", bank.Id);
                return "عملیات با موفقیت انجام شد.";
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "خطای همزمانی هنگام بروزرسانی بانک با ID: {Id}", bank.Id);
                return "این رکورد توسط کاربر دیگری تغییر یافته است. لطفاً مجدد تلاش کنید.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "خطا در بروزرسانی بانک: {@Bank}", bank);
                return "خطای غیرمنتظره رخ داد. لطفاً مجدد تلاش کنید.";
            }
        }
        public async Task<string> Delete(int userId, int id)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("درخواست حذف بانک با ID: {Id}", id);
                var entity = await _context.Definition_Bank.FindAsync(id);

                if (entity == null)
                    return "بانک مورد نظر یافت نشد.";

                if (!entity.IsDelete)
                    return "امکان حذف این بانک وجود ندارد.";

                bool hasRelatedAccounts = await _context.Definition_Bank_Account.AnyAsync(a => a.BankId == id);
                if (hasRelatedAccounts)
                    return "امکان حذف این بانک به دلیل وجود حساب‌های وابسته وجود ندارد.";

                // ثبت لاگ
                await _context.LogUser.AddAsync(new BusinessEntity.Settings.LogUser
                {
                    Description = $"حذف بانک با نام {entity.Name}",
                    UserId = userId,
                    Date = DateTime.UtcNow
                });

                _context.Definition_Bank.Remove(entity);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("بانک با موفقیت حذف شد. ID: {Id}", id);
                return "عملیات با موفقیت انجام شد.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "خطا در حذف بانک با ID: {Id}", id);
                return "خطایی در عملیات حذف رخ داد. لطفاً با پشتیبانی تماس بگیرید.";
            }
        }
    }
}
