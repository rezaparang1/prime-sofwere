using BusinessEntity.Fund;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Customer_Club
{
    public class CustomerRepository : Interface.Customer_Club.ICustomerRepository
    {
        private readonly Database _context;
        private readonly ILogger<CustomerRepository> _logger;

        public CustomerRepository(Database context, ILogger<CustomerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        //*****SEARCH*****
        public async Task<List<BusinessEntity.Customer_Club.Customer>> Search(string? name = null)
        {
            var query = _context.Customer.AsQueryable();
            if (!string.IsNullOrEmpty(name))
                query = query.Where(r => r.Name.Contains(name));

            return await query.OrderBy(f => f.Name).ToListAsync();
        }
        //******READ*******
        public async Task<IEnumerable<BusinessEntity.Customer_Club.Customer>> GetAll()
        {
            return await _context.Customer.OrderBy(f => f.Name).ToListAsync();
        }
        public async Task<BusinessEntity.Customer_Club.Customer?> GetById(int id)
        {
            return await _context.Customer.FindAsync(id);
        }
        //****** CREATE *****
        public async Task<string> Create(int userId, BusinessEntity.Customer_Club.Customer Customer)
        {
            if (Customer == null)
                return "داده ارسال نشده است.";

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("شروع ایجاد مشتری جدید: {@Customer}", Customer);

                bool nameExists = await _context.Customer
                    .AnyAsync(b => b.Phone.Trim().ToLower() == Customer.Phone.Trim().ToLower());
                if (nameExists)
                    return "شماره تماس وارد شده تکراری است.";

                // ثبت لاگ کاربر
                await _context.LogUser.AddAsync(new BusinessEntity.Settings.LogUser
                {
                    Description = $"ثبت مشتری با نام {Customer.Name}",
                    UserId = userId,
                    Date = DateTime.UtcNow
                });

                Customer.Id = 0;
                await _context.Customer.AddAsync(Customer);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("مشتری با موفقیت ایجاد شد. ID: {Id}", Customer.Id);
                return "عملیات با موفقیت انجام شد.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "خطا در ایجاد مشتری: {@Customer}", Customer);
                return "خطایی در ذخیره‌سازی اطلاعات رخ داد. لطفاً داده‌ها را بررسی کنید.";
            }
        }
        public async Task<string> Update(int userId, BusinessEntity.Customer_Club.Customer Customer)
        {
            if (Customer == null)
                return "داده ارسال نشده است.";

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("در حال بروزرسانی مشتری: {@Customer}", Customer);

                bool nameExists = await _context.Customer
                    .AnyAsync(b => b.Phone.Trim().ToLower() == Customer.Phone.Trim().ToLower() && b.Id != Customer.Id);
                if (nameExists)
                    return "شماره تماس وارد شده تکراری است.";

                var existing = await _context.Customer.FindAsync(Customer.Id);
                if (existing == null)
                    return "مشتری مورد نظر یافت نشد.";

                // بروزرسانی فیلدها
                existing.Name = Customer.Name;

                // ثبت لاگ کاربر
                await _context.LogUser.AddAsync(new BusinessEntity.Settings.LogUser
                {
                    Description = $"ویرایش مشتری با نام {Customer.Name}",
                    UserId = userId,
                    Date = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("مشتری با موفقیت بروزرسانی شد. ID: {Id}", Customer.Id);
                return "عملیات با موفقیت انجام شد.";
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "خطای همزمانی هنگام بروزرسانی مشتری با ID: {Id}", Customer.Id);
                return "این رکورد توسط کاربر دیگری تغییر یافته است. لطفاً مجدد تلاش کنید.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "خطا در بروزرسانی مشتری: {@Customer}", Customer);
                return "خطای غیرمنتظره رخ داد. لطفاً مجدد تلاش کنید.";
            }
        }
        public async Task<string> Delete(int userId, int id)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("درخواست حذف مشتری با ID: {Id}", id);
                var entity = await _context.Customer.FindAsync(id);

                if (entity == null)
                    return "مشتری مورد نظر یافت نشد.";

                bool hasRelatedAccounts = await _context.Invoices.AnyAsync(a => a.CustomerId == id);
                if (hasRelatedAccounts)
                    return "امکان حذف این مشتری به دلیل وجود حساب‌های وابسته وجود ندارد.";
                
                // ثبت لاگ
                await _context.LogUser.AddAsync(new BusinessEntity.Settings.LogUser
                {
                    Description = $"حذف مشتری با نام {entity.Name}",
                    UserId = userId,
                    Date = DateTime.UtcNow
                });

                _context.Customer.Remove(entity);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("مشتری با موفقیت حذف شد. ID: {Id}", id);
                return "عملیات با موفقیت انجام شد.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "خطا در حذف مشتری با ID: {Id}", id);
                return "خطایی در عملیات حذف رخ داد. لطفاً با پشتیبانی تماس بگیرید.";
            }
        }
    }
}
