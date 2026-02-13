using BusinessEntity.Fund;
using DataAccessLayer.Interface.Fund;
using global::DataAccessLayer.Interface.Fund_and_Bank;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BusinessEntity.DTO.Bank;

namespace DataAccessLayer.Repository.Bank
{
    public class DefinitionBankAccountRepository : IDefinitionBankAccountRepository
    {
        private readonly Database _context;
        private readonly ILogger<DefinitionBankAccountRepository> _logger;

        public DefinitionBankAccountRepository(Database context,
            ILogger<DefinitionBankAccountRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ***** Get Bank Statement *****
        public async Task<IEnumerable<BusinessEntity.DTO.Bank.BankDetailedStatementDto>> GetBankStatement(
            int? bankId = null, DateTime? dateFrom = null, DateTime? dateTo = null,
            string? receiptNumber = null, string? description = null)
        {
            var query =
                from t in _context.Transaction.AsNoTracking()
                join acc in _context.Account.AsNoTracking()
                    on t.AccountId equals acc.AccountId
                join ba in _context.Definition_Bank_Account.AsNoTracking()
                    on acc.AccountId equals ba.AccountId
                join b in _context.Definition_Bank.AsNoTracking()
                    on ba.BankId equals b.Id
                where !ba.IsDelete && !b.IsDelete
                select new
                {
                    Transaction = t,
                    Account = acc,
                    BankAccount = ba,
                    Bank = b
                };

            // فیلترها
            if (bankId.HasValue)
                query = query.Where(x => x.Bank.Id == bankId.Value);

            if (dateFrom.HasValue)
                query = query.Where(x => x.Transaction.Date >= dateFrom.Value);

            if (dateTo.HasValue)
                query = query.Where(x => x.Transaction.Date <= dateTo.Value);

            if (!string.IsNullOrWhiteSpace(receiptNumber))
                query = query.Where(x =>
                    x.Transaction.RelatedDocumentId.ToString().Contains(receiptNumber));

            if (!string.IsNullOrWhiteSpace(description))
                query = query.Where(x =>
                    x.Transaction.Description.Contains(description));

            // مرتب‌سازی بر اساس تاریخ
            var list = await query
                .OrderBy(x => x.Transaction.Date)
                .Select(x => new
                {
                    Date = x.Transaction.Date,
                    Amount = x.Transaction.Amount,
                    Type = x.Transaction.Type,
                    Description = x.Transaction.Description,
                    Receipt = x.Transaction.RelatedDocumentId != null
                        ? x.Transaction.RelatedDocumentId.ToString()
                        : "-",
                    BankName = x.Bank.Name,
                    AccountNumber = x.BankAccount.AccountNumber,
                    PersonName = x.Account.AccountName
                })
                .ToListAsync();

            // محاسبه مانده تجمعی
            long runningBalance = 0;
            var result = new List<BankDetailedStatementDto>();

            foreach (var item in list)
            {
                if (item.Type.Equals("Increase", StringComparison.OrdinalIgnoreCase))
                    runningBalance += item.Amount;
                else if (item.Type.Equals("Decrease", StringComparison.OrdinalIgnoreCase))
                    runningBalance -= item.Amount;

                result.Add(new BankDetailedStatementDto
                {
                    Date = item.Date,
                    PersonName = item.PersonName,
                    Description = item.Description,
                    OperationType = item.Type.Equals("Increase", StringComparison.OrdinalIgnoreCase)
                        ? "واریز"
                        : "برداشت",
                    ReceiptNumber = item.Receipt,
                    Amount = item.Amount,
                    Balance = runningBalance,
                    BankName = item.BankName,
                    AccountNumber = item.AccountNumber
                });
            }

            return result;
        }

        // ***** Search *****
        public async Task<List<Definition_Bank_Account>> Search(
            string? accountNumber = null, string? branchName = null,
            string? branchAddres = null, string? typeAccount = null,
            string? cardNumber = null, string? branchId = null,
            string? bracnhPhone = null, int? bankId = null)
        {
            var query = _context.Definition_Bank_Account
                .Include(b => b.Bank)
                .Include(b => b.Account)
                .Where(b => !b.IsDelete);

            if (!string.IsNullOrWhiteSpace(accountNumber))
                query = query.Where(r => r.AccountNumber.Contains(accountNumber));

            if (!string.IsNullOrWhiteSpace(branchName))
                query = query.Where(r => r.BranchName.Contains(branchName));

            if (!string.IsNullOrWhiteSpace(branchAddres))
                query = query.Where(r => r.BranchAddres.Contains(branchAddres));

            if (!string.IsNullOrWhiteSpace(typeAccount))
                query = query.Where(r => r.TypeAccount == typeAccount);

            if (!string.IsNullOrWhiteSpace(cardNumber))
                query = query.Where(r => r.CardNumber.Contains(cardNumber));

            if (!string.IsNullOrWhiteSpace(branchId))
                query = query.Where(r => r.BranchId.Contains(branchId));

            if (!string.IsNullOrWhiteSpace(bracnhPhone))
                query = query.Where(r => r.BracnhPhone.Contains(bracnhPhone));

            if (bankId.HasValue)
                query = query.Where(r => r.BankId == bankId.Value);

            return await query.OrderBy(r => r.AccountNumber).ToListAsync();
        }

        // ***** GetAll *****
        public async Task<IEnumerable<Definition_Bank_Account>> GetAll()
        {
            return await _context.Definition_Bank_Account
                .Include(b => b.Bank)
                .Include(b => b.Account)
                .Where(b => !b.IsDelete && !b.Bank.IsDelete)
                .OrderBy(b => b.AccountNumber)
                .ToListAsync();
        }

        // ***** GetById *****
        public async Task<Definition_Bank_Account?> GetById(int id)
        {
            return await _context.Definition_Bank_Account
                .Include(b => b.Account)
                .Include(b => b.Bank)
                .FirstOrDefaultAsync(b => b.Id == id && !b.IsDelete);
        }

        // ***** Create *****
        public async Task<Result> Create(Definition_Bank_Account bankAccount)
        {
            try
            {
                // بررسی تکراری بودن شماره کارت
                if (await _context.Definition_Bank_Account
                    .AnyAsync(i => i.CardNumber == bankAccount.CardNumber && !i.IsDelete))
                    return Result.Failure("شماره کارت وارد شده تکراری است.");

                // بررسی تکراری بودن شماره حساب
                if (await _context.Definition_Bank_Account
                    .AnyAsync(i => i.AccountNumber == bankAccount.AccountNumber && !i.IsDelete))
                    return Result.Failure("شماره حساب وارد شده تکراری است.");

                // بررسی وجود بانک
                var bank = await _context.Definition_Bank
                    .FirstOrDefaultAsync(b => b.Id == bankAccount.BankId && !b.IsDelete);
                if (bank == null)
                    return Result.Failure("بانک انتخاب‌شده یافت نشد.");

                // کنترل NegativeBalancePolicy
                if (bankAccount.Inventory < 0 &&
                    bankAccount.NegativeBalancePolicy == NegativeBalancePolicy.No)
                    return Result.Failure("موجودی حساب نمی‌تواند منفی باشد.");

                // ایجاد حساب مالی مرتبط
                var account = new BusinessEntity.Invoices.Account
                {
                    AccountName = $"{bank.Name} ({bankAccount.AccountNumber})",
                    AccountType = "Bank",
                    Balance = bankAccount.FirstInventory,
                    IsDelete = false
                };

                bankAccount.Account = account;
                bankAccount.Inventory = bankAccount.FirstInventory;
                bankAccount.IsDelete = false;

                await _context.Definition_Bank_Account.AddAsync(bankAccount);
                return Result.Success("عملیات با موفقیت انجام شد.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Bank Account: {@BankAccount}", bankAccount);
                return Result.Failure($"خطا در ایجاد حساب بانکی: {ex.Message}");
            }
        }

        // ***** Update *****
        public async Task<Result> Update(Definition_Bank_Account bankAccount)
        {
            try
            {
                // بررسی وجود حساب بانکی
                var existing = await _context.Definition_Bank_Account
                    .Include(f => f.Account)
                    .Include(f => f.Bank)
                    .FirstOrDefaultAsync(f => f.Id == bankAccount.Id && !f.IsDelete);

                if (existing == null)
                    return Result.Failure("حساب بانکی یافت نشد.");

                // بررسی تکراری بودن شماره کارت
                if (await _context.Definition_Bank_Account
                    .AnyAsync(i => i.CardNumber == bankAccount.CardNumber &&
                                   i.Id != bankAccount.Id && !i.IsDelete))
                    return Result.Failure("شماره کارت وارد شده تکراری است.");

                // بررسی تکراری بودن شماره حساب
                if (await _context.Definition_Bank_Account
                    .AnyAsync(i => i.AccountNumber == bankAccount.AccountNumber &&
                                   i.Id != bankAccount.Id && !i.IsDelete))
                    return Result.Failure("شماره حساب وارد شده تکراری است.");

                // بررسی تغییر بانک
                if (existing.BankId != bankAccount.BankId)
                {
                    var newBank = await _context.Definition_Bank
                        .FirstOrDefaultAsync(b => b.Id == bankAccount.BankId && !b.IsDelete);
                    if (newBank == null)
                        return Result.Failure("بانک انتخاب‌شده یافت نشد.");

                    existing.Bank = newBank;
                    existing.BankId = bankAccount.BankId;
                }

                // کنترل موجودی و NegativeBalancePolicy
                if (bankAccount.Inventory < 0 &&
                    bankAccount.NegativeBalancePolicy == NegativeBalancePolicy.No)
                    return Result.Failure("موجودی حساب نمی‌تواند منفی باشد.");

                // بروزرسانی فیلدها
                existing.AccountNumber = bankAccount.AccountNumber;
                existing.TypeAccount = bankAccount.TypeAccount;
                existing.PeopleAccount = bankAccount.PeopleAccount;
                existing.CardNumber = bankAccount.CardNumber;
                existing.BranchName = bankAccount.BranchName;
                existing.BranchId = bankAccount.BranchId;
                existing.BracnhPhone = bankAccount.BracnhPhone;
                existing.BranchAddres = bankAccount.BranchAddres;
                existing.CardReader = bankAccount.CardReader;
                existing.FirstInventory = bankAccount.FirstInventory;
                existing.Inventory = bankAccount.Inventory;
                existing.NegativeBalancePolicy = bankAccount.NegativeBalancePolicy;

                // به‌روزرسانی حساب مالی مرتبط
                if (existing.Account != null)
                {
                    existing.Account.AccountName = $"{existing.Bank?.Name} ({existing.AccountNumber})";
                    existing.Account.Balance = bankAccount.Inventory;
                }

                _context.Definition_Bank_Account.Update(existing);
                return Result.Success("عملیات با موفقیت انجام شد.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Bank Account: {@BankAccount}", bankAccount);
                return Result.Failure($"خطا در بروزرسانی حساب بانکی: {ex.Message}");
            }
        }

        // ***** Delete *****
        public async Task<Result> Delete(int id)
        {
            try
            {
                var entity = await _context.Definition_Bank_Account
                    .Include(b => b.Account)
                    .Include(b => b.Bank)
                    .FirstOrDefaultAsync(b => b.Id == id && !b.IsDelete);

                if (entity == null)
                    return Result.Failure("حساب بانکی یافت نشد.");

                // بررسی وجود تراکنش
                bool hasTransaction = await _context.Transaction
                    .AnyAsync(t => t.AccountId == entity.AccountId);

                if (hasTransaction)
                    return Result.Failure("امکان حذف حساب بانکی وجود ندارد، زیرا تراکنش‌هایی برای آن ثبت شده است.");

                // Soft Delete
                entity.IsDelete = true;
                if (entity.Account != null)
                    entity.Account.IsDelete = true;

                return Result.Success("عملیات با موفقیت انجام شد.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Bank Account: ID={Id}", id);
                return Result.Failure($"خطا در حذف حساب بانکی: {ex.Message}");
            }
        }
    }
}

