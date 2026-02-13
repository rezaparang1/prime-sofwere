using BusinessEntity.DTO.Fund;
using DataAccessLayer.Interface.Fund;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccessLayer.Repository.Fund
{
    public class FundRepository : IFundRepository
    {
        private readonly Database _context;
        private readonly ILogger<FundRepository> _logger;

        public FundRepository(Database context, ILogger<FundRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ***** SEARCH *****
        public async Task<List<BusinessEntity.Fund.Fund>> Search(string? name = null)
        {
            var query = _context.Fund.AsQueryable();
            if (!string.IsNullOrEmpty(name))
                query = query.Where(r => r.Name.Contains(name));

            return await query.OrderBy(f => f.Name).ToListAsync();
        }

        // ***** GetInventoryDetails *****
        public async Task<List<InventoryItemDto>> GetInventoryDetails()
        {
            // اطلاعات بانک‌ها
            var banks = await _context.Definition_Bank_Account
                .Include(b => b.Bank) // اگر نیاز به رابطه دارید
                .Where(b => !b.IsDelete && !b.Bank.IsDelete) // فقط موارد حذف‌نشده
                .Select(b => new InventoryItemDto
                {
                    Type = "بانک",
                    Id = b.Id,
                    // نام بانک + شماره حساب در پرانتز
                    Name = $"{b.Bank.Name} ({b.AccountNumber})",
                    AccountNumber = b.AccountNumber,
                    Inventory = b.Inventory
                })
                .ToListAsync();

            // اطلاعات صندوق‌ها
            var funds = await _context.Fund
                .Where(x => !x.IsDelete)
                .Select(f => new InventoryItemDto
                {
                    Type = "صندوق",
                    Id = f.Id,
                    Name = f.Name, // فقط نام صندوق
                    AccountNumber = string.Empty, // صندوق شماره حساب ندارد
                    Inventory = f.Inventory
                })
                .ToListAsync();

            // ترکیب و مرتب‌سازی بر اساس نام
            var result = banks.Concat(funds)
                              .OrderBy(item => item.Name)
                              .ToList();

            return result;
        }

        // ***** READ *****
        public async Task<IEnumerable<BusinessEntity.Fund.Fund>> GetAll()
        {
            return await _context.Fund
                .Where(f => !f.IsDelete)
                .Include(f => f.Account)
                .OrderBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<BusinessEntity.Fund.Fund?> GetById(int id)
        {
            return await _context.Fund
                .Include(f => f.Account)
                .FirstOrDefaultAsync(f => f.Id == id && !f.IsDelete);
        }

        // ***** CREATE *****
        public async Task<Result> Create(BusinessEntity.Fund.Fund fund)
        {
            try
            {
                // بررسی تکراری نبودن نام صندوق
                bool nameExists = await _context.Fund
                    .AnyAsync(b => b.Name.Trim().ToLower() == fund.Name.Trim().ToLower() && !b.IsDelete);
                if (nameExists)
                    return Result.Failure("نام وارد شده تکراری است.");

                // بررسی تکراری نبودن نام حساب
                bool accountExists = await _context.Account
                    .AnyAsync(a => a.AccountName.Trim().ToLower() == fund.Name.Trim().ToLower() && !a.IsDelete);
                if (accountExists)
                    return Result.Failure("حساب مالی با این نام قبلاً ثبت شده است.");

                // ایجاد حساب مالی مرتبط
                var account = new BusinessEntity.Invoices.Account
                {
                    AccountName = fund.Name,
                    AccountType = "CashBox",
                    Balance = fund.Inventory,
                    IsDelete = false
                };

                fund.Account = account;
                fund.IsDelete = false;
                fund.FirstInventory = fund.Inventory;

                await _context.Fund.AddAsync(fund);
                return Result.Success("عملیات با موفقیت انجام شد.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Fund: {@Fund}", fund);
                return Result.Failure($"خطا در ایجاد صندوق: {ex.Message}");
            }
        }

        // ***** UPDATE *****
        public async Task<Result> Update(BusinessEntity.Fund.Fund fund)
        {
            try
            {
                // بررسی وجود صندوق
                var existingFund = await _context.Fund
                    .Include(f => f.Account)
                    .FirstOrDefaultAsync(f => f.Id == fund.Id && !f.IsDelete);

                if (existingFund == null)
                    return Result.Failure("صندوق یافت نشد.");

                // بررسی تکراری نبودن نام (به جز خودش)
                bool nameExists = await _context.Fund
                    .AnyAsync(f => f.Name == fund.Name && f.Id != fund.Id && !f.IsDelete);
                if (nameExists)
                    return Result.Failure("نام وارد شده تکراری است.");

                // به‌روزرسانی اطلاعات صندوق
                existingFund.Name = fund.Name;
                existingFund.Description = fund.Description;
                existingFund.Inventory = fund.Inventory;
                existingFund.NegativeBalancePolicy = fund.NegativeBalancePolicy;

                // به‌روزرسانی حساب مالی مرتبط
                if (existingFund.Account != null)
                {
                    existingFund.Account.AccountName = fund.Name;
                    existingFund.Account.Balance = fund.Inventory;
                }

                _context.Fund.Update(existingFund);
                return Result.Success("عملیات با موفقیت انجام شد.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Fund: {@Fund}", fund);
                return Result.Failure($"خطا در بروزرسانی صندوق: {ex.Message}");
            }
        }

        // ***** DELETE *****
        public async Task<Result> Delete(int id)
        {
            try
            {
                var fund = await _context.Fund
                    .Include(f => f.Account)
                    .FirstOrDefaultAsync(f => f.Id == id && !f.IsDelete);

                if (fund == null)
                    return Result.Failure("صندوق یافت نشد.");

                // بررسی وجود تراکنش برای حساب این صندوق
                bool hasTransaction = await _context.Transaction
                    .AnyAsync(t => t.AccountId == fund.AccountId);
                if (hasTransaction)
                    return Result.Failure("امکان حذف صندوق وجود ندارد، زیرا تراکنش‌هایی برای آن ثبت شده است.");

                // Soft Delete
                fund.IsDelete = true;
                if (fund.Account != null)
                    fund.Account.IsDelete = true;

                return Result.Success("عملیات با موفقیت انجام شد.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Fund with ID: {Id}", id);
                return Result.Failure($"خطا در حذف صندوق: {ex.Message}");
            }
        }
    }
}
