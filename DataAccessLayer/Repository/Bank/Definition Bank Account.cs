using BusinessEntity;
using BusinessEntity.Bank;
using BusinessEntity.Financial_Operations;
using BusinessEntity.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Bank
{
    public class DefinitionBankAccountRepository : Interface.Bank.IDefinitionBankAccountRepository
    {
        private readonly Database _context;
        private readonly ILogger<DefinitionBankAccountRepository> _logger;

        public DefinitionBankAccountRepository(Database context, ILogger<DefinitionBankAccountRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        //******SEARCH*****
        public async Task<IEnumerable<BankDetailedStatementDto>> GetBankStatement(int? bankId = null,DateTime? dateFrom = null,DateTime? dateTo = null,string? receiptNumber = null,string? description = null)
        {
            var query =
                from t in _context.Transaction.AsNoTracking()
                join acc in _context.Account.AsNoTracking()
                    on t.AccountId equals acc.AccountId
                join ba in _context.Definition_Bank_Account.AsNoTracking()
                    on acc.AccountId equals ba.AccountId
                join b in _context.Definition_Bank.AsNoTracking()
                    on ba.BankId equals b.Id
                select new
                {
                    Transaction = t,
                    Account = acc,
                    BankAccount = ba,
                    Bank = b
                };

            // 🔹 فیلترها
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

            // 🔹 مرتب‌سازی بر اساس تاریخ
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

            // 🔹 محاسبه مانده تجمعی
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

        public async Task<List<BusinessEntity.Bank.Definition_Bank_Account>> Search(string? accountNumber = null,string? branchName = null,string? branchAddres = null,string? typeAccount = null,string? cardNumber = null,string? branchId = null,string? bracnhPhone = null,int? bankId = null)
        {
            _logger.LogInformation(
                "جستجوی حساب‌های بانکی: AccountNumber={AccountNumber}, BranchName={BranchName}, BranchAddres={BranchAddres}, TypeAccount={TypeAccount}, CardNumber={CardNumber}, BranchId={BranchId}, BracnhPhone={BracnhPhone}, BankId={BankId}",
                accountNumber, branchName, branchAddres, typeAccount, cardNumber, branchId, bracnhPhone, bankId);

            var query = _context.Definition_Bank_Account
                .Include(b => b.Bank)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(accountNumber))
                query = query.Where(r => r.AccountNumber.Contains(accountNumber));

            if (!string.IsNullOrWhiteSpace(branchName))
                query = query.Where(r => r.BranchName.Contains(branchName));

            if (!string.IsNullOrWhiteSpace(branchAddres))
                query = query.Where(r => r.BranchAddres.Contains(branchAddres));

            if (!string.IsNullOrWhiteSpace(typeAccount))
                query = query.Where(r => r.TypeAccount.Contains(typeAccount));

            if (!string.IsNullOrWhiteSpace(cardNumber))
                query = query.Where(r => r.CardNumber.Contains(cardNumber));

            if (!string.IsNullOrWhiteSpace(branchId))
                query = query.Where(r => r.BranchId.Contains(branchId));

            if (!string.IsNullOrWhiteSpace(bracnhPhone))
                query = query.Where(r => r.BracnhPhone.Contains(bracnhPhone));

            if (bankId.HasValue)
                query = query.Where(r => r.BankId == bankId.Value);

            var result = await query.OrderBy(r => r.AccountNumber).ToListAsync();

            _logger.LogInformation("{Count} حساب بانکی یافت شد.", result.Count);
            return result;
        }
        //******READ******
        public async Task<IEnumerable<BusinessEntity.Bank.Definition_Bank_Account>> GetAll()
        {
            var result = await _context.Definition_Bank_Account
                .Include(b => b.Bank)
                .OrderBy(b => b.AccountNumber)
                .ToListAsync();

            _logger.LogInformation("{Count} حساب بانکی بازیابی شد.", result.Count);
            return result;
        }
        public async Task<BusinessEntity.Bank.Definition_Bank_Account?> GetById(int id)
        {
            var entity = await _context.Definition_Bank_Account
                .Include(b => b.Account)
                .Include(b => b.Bank)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (entity == null)
                _logger.LogWarning("حساب بانکی با شناسه {Id} یافت نشد", id);
            else
                _logger.LogInformation("حساب بانکی {Id} با موفقیت واکشی شد.", id);

            return entity;
        }
        //******CREATE******
        public async Task<string> Create(int userId, BusinessEntity.Bank.Definition_Bank_Account bankAccount)
        {
            if (bankAccount == null)
                return "داده ارسال نشده است.";

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("در حال ایجاد حساب بانکی جدید: {@Bank}", bankAccount);

                // چک تکراری بودن
                if (await _context.Definition_Bank_Account.AnyAsync(i => i.CardNumber == bankAccount.CardNumber))
                    return "شماره کارت وارد شده تکراری است.";

                if (await _context.Definition_Bank_Account.AnyAsync(i => i.AccountNumber == bankAccount.AccountNumber))
                    return "شماره حساب وارد شده تکراری است.";

                var bank = await _context.Definition_Bank.FindAsync(bankAccount.BankId);
                if (bank == null)
                    return "بانک انتخاب‌شده یافت نشد.";

                // کنترل NegativeBalancePolicy
                if (bankAccount.Inventory < 0 && bankAccount.NegativeBalancePolicy == BusinessEntity.Bank.NegativeBalancePolicy.No)
                    return "موجودی حساب نمی‌تواند منفی باشد.";

                // ساخت حساب مالی مرتبط
                var account = new BusinessEntity.Financial_Operations.Account
                {
                    AccountName = $"{bank.Name} ({bankAccount.AccountNumber})",
                    AccountType = "Bank",
                    Balance = bankAccount.FirstInventory
                };

                bankAccount.Account = account;
                bankAccount.Inventory = bankAccount.FirstInventory;

                await _context.Definition_Bank_Account.AddAsync(bankAccount);

                // لاگ کاربر
                await _context.LogUser.AddAsync(new BusinessEntity.Settings.LogUser
                {
                    Description = $"ایجاد حساب بانکی جدید ({bank.Name}) با شماره حساب {bankAccount.AccountNumber}",
                    UserId = userId,
                    Date = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return "عملیات با موفقیت انجام شد.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "خطا در ایجاد حساب بانکی: {@Bank}", bankAccount);
                return $"خطایی در ذخیره اطلاعات رخ داد: {ex.Message}";
            }
        }
        public async Task<string> Update(int userId, BusinessEntity.Bank.Definition_Bank_Account bankAccount)
        {
            if (bankAccount == null)
                return "داده ارسال نشده است.";

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("در حال ویرایش حساب بانکی: {@Bank}", bankAccount);

                // چک تکراری بودن
                if (await _context.Definition_Bank_Account.AnyAsync(i => i.CardNumber == bankAccount.CardNumber && i.Id != bankAccount.Id))
                    return "شماره کارت وارد شده تکراری است.";

                if (await _context.Definition_Bank_Account.AnyAsync(i => i.AccountNumber == bankAccount.AccountNumber && i.Id != bankAccount.Id))
                    return "شماره حساب وارد شده تکراری است.";

                var existing = await _context.Definition_Bank_Account
                    .Include(f => f.Account)
                    .Include(f => f.Bank)
                    .FirstOrDefaultAsync(f => f.Id == bankAccount.Id);

                if (existing == null)
                    return "حساب بانکی یافت نشد.";

                // بررسی تغییر بانک و بارگذاری بانک جدید در صورت نیاز
                if (existing.BankId != bankAccount.BankId)
                {
                    var newBank = await _context.Definition_Bank.FindAsync(bankAccount.BankId);
                    if (newBank == null)
                        return "بانک انتخاب‌شده یافت نشد.";

                    existing.Bank = newBank;
                    existing.BankId = bankAccount.BankId;
                }

                // کنترل موجودی و NegativeBalancePolicy
                if (bankAccount.Inventory < 0 && bankAccount.NegativeBalancePolicy == BusinessEntity.Bank.NegativeBalancePolicy.No)
                    return "موجودی حساب نمی‌تواند منفی باشد.";

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

                // لاگ کاربر
                await _context.LogUser.AddAsync(new BusinessEntity.Settings.LogUser
                {
                    Description = $"ویرایش حساب بانکی {existing.Bank?.Name} با شماره حساب {existing.AccountNumber}",
                    UserId = userId,
                    Date = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return "عملیات با موفقیت انجام شد.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "خطا در ویرایش حساب بانکی: {@Bank}", bankAccount);
                return $"خطایی در ذخیره تغییرات رخ داد: {ex.Message}";
            }
        }
        public async Task<string> Delete(int userId, int id)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var entity = await _context.Definition_Bank_Account
                    .Include(b => b.Account)
                    .Include(b => b.Bank)
                    .FirstOrDefaultAsync(b => b.Id == id);

                if (entity == null)
                    return "درخواست مورد نظر شما یافت نشد.";

                bool hasTransaction = await _context.Transaction
                    .AnyAsync(t => t.AccountId == entity.AccountId);

                if (hasTransaction)
                    return "امکان حذف حساب بانکی وجود ندارد، زیرا تراکنش‌هایی برای آن ثبت شده است.";

                if (entity.Account != null)
                    _context.Account.Remove(entity.Account);

                _context.Definition_Bank_Account.Remove(entity);

                await _context.LogUser.AddAsync(new BusinessEntity.Settings.LogUser
                {
                    Description = $"حذف حساب بانکی ({entity.Bank?.Name}) با شماره حساب {entity.AccountNumber}",
                    UserId = userId,
                    Date = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return "عملیات با موفقیت انجام شد.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "خطا در حذف حساب بانکی: ID={Id}", id);
                return $"خطایی در عملیات حذف رخ داد: {ex.Message}";
            }
        }
    }

}
