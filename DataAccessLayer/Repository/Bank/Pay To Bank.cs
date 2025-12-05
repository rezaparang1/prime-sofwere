using BusinessEntity.Bank;
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
    public class PayToBankRepository : Interface.Bank.IPayToBankRepository
    {
        private readonly Database _context;
        private readonly ILogger<PayToBankRepository> _logger;

        public PayToBankRepository(Database context, ILogger<PayToBankRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        //*****SEARCH*****
        public async Task<IEnumerable<PayToBankListDto>> Search(
     DateTime? DateFirst = null,
     DateTime? DateEnd = null,
     long? AmountFirst = null,
     long? AmountEnd = null,
     int? FundId = null,
     int? BankId = null,
     string? Sand = null,
     int? PeopleId = null,
     string? Description = null)
        {
            var query = _context.Pay_To_Bank
                .AsNoTracking()
                .Include(p => p.Fund)
                .Include(p => p.BankAccunt).ThenInclude(b => b.Bank)
                .AsQueryable();

            if (DateFirst.HasValue)
                query = query.Where(p => p.Date >= DateFirst.Value);
            if (DateEnd.HasValue)
                query = query.Where(p => p.Date <= DateEnd.Value);

            if (AmountFirst.HasValue)
                query = query.Where(p => p.Amount >= AmountFirst.Value);
            if (AmountEnd.HasValue)
                query = query.Where(p => p.Amount <= AmountEnd.Value);

            if (FundId.HasValue)
                query = query.Where(p => p.FundId == FundId.Value);
            if (BankId.HasValue)
                query = query.Where(p => p.BankId == BankId.Value);

            if (!string.IsNullOrWhiteSpace(Sand))
                query = query.Where(p => p.IdSand.Contains(Sand));

            if (PeopleId.HasValue)
                query = query.Where(p => p.PeopleId == PeopleId.Value);

            if (!string.IsNullOrWhiteSpace(Description))
                query = query.Where(p => p.Description.Contains(Description));

            var result = await query
                .OrderByDescending(p => p.Date)
                .Select(p => new PayToBankListDto
                {
                    Id = p.Id,
                    Source = p.Fund != null ? p.Fund.Name : "نامشخص",
                    Destination = (p.BankAccunt != null && p.BankAccunt.Bank != null)
                        ? $"{p.BankAccunt.Bank.Name} ({p.BankAccunt.AccountNumber})"
                        : "نامشخص (-)",
                    Amount = p.Amount,
                    Date = p.Date,
                    Description = p.Description
                })
                .ToListAsync();

            return result;
        }

        //******READ*******
        public async Task<IEnumerable<PayToBankListDto>> GetAll()
        {
            var result = await _context.Pay_To_Bank
                .AsNoTracking()
                .Include(p => p.Fund)
                .Include(p => p.BankAccunt).ThenInclude(b => b.Bank)
                .OrderByDescending(p => p.Date)
                .Select(p => new PayToBankListDto
                {
                    Id = p.Id,
                    Source = p.Fund != null ? p.Fund.Name : "نامشخص",
                    Destination = (p.BankAccunt != null && p.BankAccunt.Bank != null)
                        ? $"{p.BankAccunt.Bank.Name} ({p.BankAccunt.AccountNumber})"
                        : "نامشخص (-)",
                    Amount = p.Amount,
                    Date = p.Date,
                    Description = p.Description
                })
                .ToListAsync();

            return result;
        }
        public async Task<Pay_To_Bank?> GetById(int id)
        {
            var record = await _context.Pay_To_Bank
                .Include(p => p.Fund)
                .ThenInclude(f => f.Account)
                .Include(p => p.People)
                .Include(p => p.Bank)
                .Include(p => p.BankAccunt)
                .ThenInclude(b => b.Account)
                .FirstOrDefaultAsync(p => p.Id == id);

            return record;
        }
        //******CRUD*******
        public async Task<string> Create(int userId, Pay_To_Bank model)
        {
            if (model == null)
                return "داده ارسال نشده است.";

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // تاریخ پیش‌فرض
                model.Date = model.Date == default ? DateTime.UtcNow : model.Date;

                // صندوق
                var fund = await _context.Fund
                    .Include(f => f.Account)
                    .FirstOrDefaultAsync(f => f.Id == model.FundId);
                if (fund == null) return "صندوق یافت نشد.";

                // حساب بانک
                var bankAccount = await _context.Definition_Bank_Account
                    .Include(b => b.Account)
                    .Include(b => b.Bank)
                    .FirstOrDefaultAsync(b => b.Id == model.BankAccountId);
                if (bankAccount == null) return "حساب بانکی یافت نشد.";

                // موجودی
                if (fund.Inventory < model.Amount)
                    return "موجودی صندوق کافی نیست.";

                // بروزرسانی موجودی
                fund.Inventory -= model.Amount;
                bankAccount.Inventory += model.Amount;

                // ثبت سند و ذخیره برای گرفتن Id
                await _context.Pay_To_Bank.AddAsync(model);
                await _context.SaveChangesAsync();

                // تراکنش صندوق
                var fundTransaction = new BusinessEntity.Financial_Operations.Transaction
                {
                    AccountId = fund.AccountId,
                    Amount = model.Amount,
                    Type = "Decrease",
                    Date = model.Date,
                    Description = $"واریز به بانک: {bankAccount.Bank?.Name} ({bankAccount.AccountNumber})",
                    RelatedDocumentType = "PayToBank",
                    RelatedDocumentId = model.Id,
                    RelatedAccountId = bankAccount.AccountId,
                    PaymentMethod = "Cash"
                };

                // تراکنش بانک
                var bankTransaction = new BusinessEntity.Financial_Operations.Transaction
                {
                    AccountId = bankAccount.AccountId,
                    Amount = model.Amount,
                    Type = "Increase",
                    Date = model.Date,
                    Description = $"دریافت از صندوق: {fund.Name}",
                    RelatedDocumentType = "PayToBank",
                    RelatedDocumentId = model.Id,
                    RelatedAccountId = fund.AccountId,
                    PaymentMethod = "Cash"
                };

                await _context.Transaction.AddRangeAsync(fundTransaction, bankTransaction);

                // لاگ
                await _context.LogUser.AddAsync(new LogUser
                {
                    Description = $"ثبت واریز از صندوق ({fund.Name}) به بانک ({bankAccount.Bank?.Name}) به مبلغ {model.Amount:N0}",
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
                _logger.LogError(ex, "خطا در ثبت واریز به بانک: {@PayToBank}", model);
                return $"خطایی رخ داد: {ex.Message}";
            }
        }
        public async Task<string> Update(int userId, Pay_To_Bank updatedModel)
        {
            if (updatedModel == null)
                return "داده ارسال نشده است.";

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1️⃣ لود رکورد اصلی و صندوق و بانک و تراکنش‌ها
                var existing = await _context.Pay_To_Bank
                    .Include(p => p.Fund).ThenInclude(f => f.Account)
                    .Include(p => p.BankAccunt).ThenInclude(b => b.Account)
                    .FirstOrDefaultAsync(p => p.Id == updatedModel.Id);

                if (existing == null)
                    return "رکورد مورد نظر یافت نشد.";

                var transactions = await _context.Transaction
                    .Where(t => t.RelatedDocumentType == "PayToBank" && t.RelatedDocumentId == existing.Id)
                    .ToListAsync();

                if (transactions.Count != 2)
                    return "تراکنش‌های مرتبط ناقص است.";

                // 2️⃣ برگرداندن موجودی قبلی
                existing.Fund.Inventory += existing.Amount;
                if (existing.Fund.Account != null)
                    existing.Fund.Account.Balance += existing.Amount;

                existing.BankAccunt.Inventory -= existing.Amount;
                if (existing.BankAccunt.Account != null)
                    existing.BankAccunt.Account.Balance -= existing.Amount;

                // 3️⃣ بررسی تغییر صندوق یا بانک
                if (existing.FundId != updatedModel.FundId)
                {
                    existing.Fund = await _context.Fund
                        .Include(f => f.Account)
                        .FirstAsync(f => f.Id == updatedModel.FundId);
                }

                if (existing.BankAccountId != updatedModel.BankAccountId)
                {
                    existing.BankAccunt = await _context.Definition_Bank_Account
                        .Include(b => b.Account)
                        .Include(b => b.Bank)
                        .FirstAsync(b => b.Id == updatedModel.BankAccountId);
                }

                // 4️⃣ اعمال موجودی جدید
                if (existing.Fund.Inventory < updatedModel.Amount)
                    return "موجودی صندوق کافی نیست.";

                existing.Fund.Inventory -= updatedModel.Amount;
                if (existing.Fund.Account != null)
                    existing.Fund.Account.Balance -= updatedModel.Amount;

                existing.BankAccunt.Inventory += updatedModel.Amount;
                if (existing.BankAccunt.Account != null)
                    existing.BankAccunt.Account.Balance += updatedModel.Amount;

                // 5️⃣ بروزرسانی تراکنش‌ها
                foreach (var t in transactions)
                {
                    if (t.Type == "Decrease")
                    {
                        t.AccountId = existing.Fund.AccountId;
                        t.Amount = updatedModel.Amount;
                        t.Description = $"واریز به بانک {existing.BankAccunt.Bank?.Name} ({existing.BankAccunt.AccountNumber})";
                    }
                    else if (t.Type == "Increase")
                    {
                        t.AccountId = existing.BankAccunt.AccountId;
                        t.Amount = updatedModel.Amount;
                        t.Description = $"دریافت از صندوق {existing.Fund.Name}";
                    }
                    t.Date = updatedModel.Date;
                }
                _context.Transaction.UpdateRange(transactions);

                // 6️⃣ بروزرسانی سایر فیلدها
                existing.Amount = updatedModel.Amount;
                existing.Date = updatedModel.Date;
                existing.Description = updatedModel.Description;
                existing.IdSand = updatedModel.IdSand;
                existing.FundId = updatedModel.FundId;
                existing.BankId = updatedModel.BankId;
                existing.BankAccountId = updatedModel.BankAccountId;
                existing.PeopleId = updatedModel.PeopleId;

                _context.Pay_To_Bank.Update(existing);

                // 7️⃣ ثبت لاگ
                await _context.LogUser.AddAsync(new BusinessEntity.Settings.LogUser
                {
                    UserId = userId,
                    Date = DateTime.UtcNow,
                    Description = $"ویرایش واریز به بانک (Id={existing.Id}) مبلغ {updatedModel.Amount:N0} از صندوق {existing.Fund.Name} به بانک {existing.BankAccunt.Bank?.Name} ({existing.BankAccunt.AccountNumber})"
                });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return "ویرایش با موفقیت انجام شد.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "خطا در ویرایش واریز به بانک: {@PayToBank}", updatedModel);
                return $"خطا در ویرایش رکورد رخ داد: {ex.Message}";
            }
        }
    }
}
