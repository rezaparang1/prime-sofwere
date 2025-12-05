using BusinessEntity.Bank;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Bank
{
    public class BankToBankRepository : Interface.Bank.IBankToBankRepository
    {
        private readonly Database _context;
        private readonly ILogger<BankToBankRepository> _logger;

        public BankToBankRepository(Database context, ILogger<BankToBankRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        //*****SEARCH*****
        public async Task<IEnumerable<BusinessEntity.Bank.BankToBankListDto>> Search(DateTime? DateFirst = null, DateTime? DateEnd = null, long? AmountFirst = null, long? AmountEnd = null, int? BankFirst = null, int? BankEnd = null, string? SandFirst = null, string? SandEnd = null, string? Description = null)
        {
            var query = _context.Bank_To_Bank
                .Include(b => b.BankFirst)
                .Include(b => b.BankEnd)
                .Include(b => b.BankAccountFirst)
                .Include(b => b.BankAccountEnd)
                .AsQueryable();

            // 🔹 فیلتر تاریخ
            if (DateFirst.HasValue)
                query = query.Where(b => b.Date >= DateFirst.Value);

            if (DateEnd.HasValue)
                query = query.Where(b => b.Date <= DateEnd.Value);

            // 🔹 فیلتر مبلغ
            if (AmountFirst.HasValue)
                query = query.Where(b => b.Amount >= AmountFirst.Value);

            if (AmountEnd.HasValue)
                query = query.Where(b => b.Amount <= AmountEnd.Value);

            // 🔹 فیلتر بانک مبدا و مقصد
            if (BankFirst.HasValue)
                query = query.Where(b => b.BankFirstId == BankFirst.Value);

            if (BankEnd.HasValue)
                query = query.Where(b => b.BankEndId == BankEnd.Value);

            // 🔹 فیلتر شناسه‌های سند
            if (!string.IsNullOrWhiteSpace(SandFirst))
                query = query.Where(b => b.IdSandFirst.Contains(SandFirst));

            if (!string.IsNullOrWhiteSpace(SandEnd))
                query = query.Where(b => b.IdSandEnd.Contains(SandEnd));

            // 🔹 فیلتر توضیحات
            if (!string.IsNullOrWhiteSpace(Description))
                query = query.Where(b => b.Description.Contains(Description));

            // 🔹 انتخاب خروجی ساده
            var result = await query
                .OrderByDescending(b => b.Date)
                .Select(b => new BusinessEntity.Bank.BankToBankListDto
                {
                    Id = b.Id,
                    Source = b.BankFirst.Name + " (" + b.BankAccountFirst.AccountNumber + ")",
                    Destination = b.BankEnd.Name + " (" + b.BankAccountEnd.AccountNumber + ")",
                    Amount = b.Amount,
                    Date = b.Date,
                    Description = b.Description
                })
                .ToListAsync();

            return result;
        }
        //******READ*******
        public async Task<IEnumerable<BankToBankListDto>> GetAll()
        {
            var result = await _context.Bank_To_Bank
                .Include(b => b.BankFirst)
                .Include(b => b.BankEnd)
                .Include(b => b.BankAccountFirst)
                .Include(b => b.BankAccountEnd)
                .Select(b => new BankToBankListDto
                {
                    Id = b.Id,
                    Source = b.BankFirst.Name + " (" + b.BankAccountFirst.AccountNumber + ")",
                    Destination = b.BankEnd.Name + " (" + b.BankAccountEnd.AccountNumber + ")",
                    Amount = b.Amount,
                    Date = b.Date,
                    Description = b.Description
                })
                .OrderByDescending(b => b.Date)
                .ToListAsync();

            return result;
        }
        public async Task<BankToBankListDto?> GetById(int Id)
        {
            var record = await _context.Bank_To_Bank
                .Include(b => b.BankFirst)
                .Include(b => b.BankEnd)
                .Include(b => b.BankAccountFirst)
                .Include(b => b.BankAccountEnd)
                .Where(b => b.Id == Id)
                .Select(b => new BankToBankListDto
                {
                    Id = b.Id,
                    Source = b.BankFirst.Name + " (" + b.BankAccountFirst.AccountNumber + ")",
                    Destination = b.BankEnd.Name + " (" + b.BankAccountEnd.AccountNumber + ")",
                    Amount = b.Amount,
                    Date = b.Date,
                    Description = b.Description
                })
                .FirstOrDefaultAsync();

            return record;
        }
        //******CRUD*******
        public async Task<string> Create(int userId, BusinessEntity.Bank.Bank_To_Bank bankToBank)
        {
            if (bankToBank == null)
                return "داده ارسال نشده است.";

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("در حال ثبت انتقال بین بانکی جدید: {@BankToBank}", bankToBank);

                // 🔹 بررسی حساب‌های مبدا و مقصد
                var bankAccountFirst = await _context.Definition_Bank_Account
                    .Include(i => i.Bank)
                    .Include(i => i.Account)
                    .FirstOrDefaultAsync(i => i.Id == bankToBank.BankAccountFirstId);

                var bankAccountEnd = await _context.Definition_Bank_Account
                    .Include(i => i.Bank)
                    .Include(i => i.Account)
                    .FirstOrDefaultAsync(i => i.Id == bankToBank.BankAccountIdEnd);

                if (bankAccountFirst == null || bankAccountEnd == null)
                    return "حساب بانکی مبدا یا مقصد یافت نشد.";

                if (bankAccountFirst.Id == bankAccountEnd.Id)
                    return "حساب مبدا و مقصد نمی‌تواند یکسان باشد.";

                if (bankAccountFirst.Inventory < bankToBank.Amount)
                    return "موجودی حساب مبدا کافی نیست.";

                // 🔹 ثبت سند انتقال
                bankToBank.Date = DateTime.UtcNow;
                await _context.Bank_To_Bank.AddAsync(bankToBank);
                await _context.SaveChangesAsync();

                // 🔹 ایجاد تراکنش کاهش از حساب مبدا
                var decreaseTransaction = new BusinessEntity.Financial_Operations.Transaction
                {
                    AccountId = bankAccountFirst.AccountId,  // ✅ درست است (AccountId)
                    Amount = bankToBank.Amount,
                    Type = "Decrease",
                    Description = $"انتقال به بانک {bankAccountEnd.Bank?.Name} ({bankAccountEnd.AccountNumber})",
                    Date = bankToBank.Date,
                    RelatedDocumentType = "BankToBank",
                    RelatedDocumentId = bankToBank.Id,
                    RelatedAccountId = bankAccountEnd.AccountId, // ✅ حساب مقصد
                    PaymentMethod = "BankTransfer"
                };

                // 🔹 ایجاد تراکنش افزایش در حساب مقصد
                var increaseTransaction = new BusinessEntity.Financial_Operations.Transaction
                {
                    AccountId = bankAccountEnd.AccountId,  // ✅ درست است
                    Amount = bankToBank.Amount,
                    Type = "Increase",
                    Description = $"دریافت از بانک {bankAccountFirst.Bank?.Name} ({bankAccountFirst.AccountNumber})",
                    Date = bankToBank.Date,
                    RelatedDocumentType = "BankToBank",
                    RelatedDocumentId = bankToBank.Id,
                    RelatedAccountId = bankAccountFirst.AccountId, // ✅ حساب مبدا
                    PaymentMethod = "BankTransfer"
                };

                await _context.Transaction.AddAsync(decreaseTransaction);
                await _context.Transaction.AddAsync(increaseTransaction);

                // 🔹 بروزرسانی موجودی‌ها
                bankAccountFirst.Inventory -= bankToBank.Amount;
                bankAccountEnd.Inventory += bankToBank.Amount;

                // 🔹 بروزرسانی مانده حساب‌ها در جدول Account
                if (bankAccountFirst.Account != null)
                    bankAccountFirst.Account.Balance -= bankToBank.Amount;

                if (bankAccountEnd.Account != null)
                    bankAccountEnd.Account.Balance += bankToBank.Amount;

                _context.Definition_Bank_Account.Update(bankAccountFirst);
                _context.Definition_Bank_Account.Update(bankAccountEnd);

                // 🔹 ثبت لاگ کاربر
                await _context.LogUser.AddAsync(new BusinessEntity.Settings.LogUser
                {
                    Description = $"انتقال مبلغ {bankToBank.Amount:N0} از حساب {bankAccountFirst.Bank?.Name} ({bankAccountFirst.AccountNumber}) به {bankAccountEnd.Bank?.Name} ({bankAccountEnd.AccountNumber})",
                    UserId = userId,
                    Date = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("✅ انتقال بین بانکی با موفقیت انجام شد: {@BankToBank}", bankToBank);
                return "انتقال با موفقیت انجام شد.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "❌ خطا در ثبت انتقال بین بانکی: {@BankToBank}", bankToBank);
                return $"خطایی در ثبت انتقال بین بانکی رخ داد: {ex.Message}";
            }
        }

        public async Task<string> Update(int userId, BusinessEntity.Bank.Bank_To_Bank updatedModel)
        {
            if (updatedModel == null)
                return "داده ارسال نشده است.";

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1️⃣ لود رکورد اصلی و تراکنش‌های مرتبط
                var existing = await _context.Bank_To_Bank
                    .Include(b => b.BankAccountFirst).ThenInclude(a => a.Account)
                    .Include(b => b.BankAccountEnd).ThenInclude(a => a.Account)
                    .FirstOrDefaultAsync(b => b.Id == updatedModel.Id);

                if (existing == null)
                    return "رکورد مورد نظر یافت نشد.";

                var transactions = await _context.Transaction
                    .Where(t => t.RelatedDocumentType == "BankToBank" && t.RelatedDocumentId == existing.Id)
                    .ToListAsync();

                if (transactions.Count != 2)
                    return "تراکنش‌های مرتبط ناقص است.";

                // 2️⃣ برگرداندن موجودی قبلی (لغو انتقال قبلی)
                existing.BankAccountFirst.Inventory += existing.Amount;
                existing.BankAccountEnd.Inventory -= existing.Amount;

                // بروزرسانی تراز حساب‌ها (در Account)
                if (existing.BankAccountFirst.Account != null)
                    existing.BankAccountFirst.Account.Balance += existing.Amount;
                if (existing.BankAccountEnd.Account != null)
                    existing.BankAccountEnd.Account.Balance -= existing.Amount;

                // 3️⃣ بررسی تغییر حساب مبدا یا مقصد
                if (existing.BankAccountFirstId != updatedModel.BankAccountFirstId)
                {
                    existing.BankAccountFirst = await _context.Definition_Bank_Account
                        .Include(a => a.Bank)
                        .Include(a => a.Account)
                        .FirstAsync(a => a.Id == updatedModel.BankAccountFirstId);
                }

                if (existing.BankAccountIdEnd != updatedModel.BankAccountIdEnd)
                {
                    existing.BankAccountEnd = await _context.Definition_Bank_Account
                        .Include(a => a.Bank)
                        .Include(a => a.Account)
                        .FirstAsync(a => a.Id == updatedModel.BankAccountIdEnd);
                }

                // 4️⃣ بررسی موجودی جدید قبل از انجام عملیات
                if (existing.BankAccountFirst.Inventory < updatedModel.Amount)
                    return "موجودی حساب مبدا کافی نیست.";

                // 5️⃣ اعمال موجودی جدید
                existing.BankAccountFirst.Inventory -= updatedModel.Amount;
                existing.BankAccountEnd.Inventory += updatedModel.Amount;

                // بروزرسانی تراز حساب‌ها (در Account)
                if (existing.BankAccountFirst.Account != null)
                    existing.BankAccountFirst.Account.Balance -= updatedModel.Amount;
                if (existing.BankAccountEnd.Account != null)
                    existing.BankAccountEnd.Account.Balance += updatedModel.Amount;

                // 6️⃣ به‌روزرسانی تراکنش‌ها
                foreach (var t in transactions)
                {
                    if (t.Type == "Decrease")
                    {
                        t.AccountId = existing.BankAccountFirst.AccountId;
                        t.Amount = updatedModel.Amount;
                        t.Description = $"انتقال به بانک {existing.BankAccountEnd.Bank?.Name} ({existing.BankAccountEnd.AccountNumber})";
                    }
                    else if (t.Type == "Increase")
                    {
                        t.AccountId = existing.BankAccountEnd.AccountId;
                        t.Amount = updatedModel.Amount;
                        t.Description = $"دریافت از بانک {existing.BankAccountFirst.Bank?.Name} ({existing.BankAccountFirst.AccountNumber})";
                    }

                    t.Date = updatedModel.Date;
                }

                _context.Transaction.UpdateRange(transactions);

                // 7️⃣ به‌روزرسانی فیلدهای انتقال بین بانکی
                existing.Amount = updatedModel.Amount;
                existing.Date = updatedModel.Date;
                existing.Description = updatedModel.Description;
                existing.IdSandFirst = updatedModel.IdSandFirst;
                existing.IdSandEnd = updatedModel.IdSandEnd;
                existing.BankAccountFirstId = updatedModel.BankAccountFirstId;
                existing.BankAccountIdEnd = updatedModel.BankAccountIdEnd;

                _context.Bank_To_Bank.Update(existing);

                // 8️⃣ ثبت لاگ کاربر
                await _context.LogUser.AddAsync(new BusinessEntity.Settings.LogUser
                {
                    UserId = userId,
                    Date = DateTime.UtcNow,
                    Description = $"ویرایش انتقال بین بانکی (Id={existing.Id}) مبلغ {updatedModel.Amount:N0} از {existing.BankAccountFirst.Bank?.Name} ({existing.BankAccountFirst.AccountNumber}) به {existing.BankAccountEnd.Bank?.Name} ({existing.BankAccountEnd.AccountNumber})"
                });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return "ویرایش با موفقیت انجام شد.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "خطا در ویرایش انتقال بین بانکی: {@BankToBank}", updatedModel);
                return $"خطا در ویرایش رکورد رخ داد: {ex.Message}";
            }
        }


    }
}
