using BusinessEntity.Financial_Operations;
using BusinessEntity.Fund;
using BusinessEntity.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Fund
{
    public class FundRepository : Interface.Fund.IFundRepository
    {
        private readonly Database _context;
        private readonly ILogger<FundRepository> _logger;

        public FundRepository(Database context, ILogger<FundRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        //*****SEARCH*****
        public async Task<List<BusinessEntity.Fund.Fund>> Search(string? name = null)
        {
            var query = _context.Fund.AsQueryable();
            if (!string.IsNullOrEmpty(name))
                query = query.Where(r => r.Name.Contains(name));

            return await query.OrderBy(f => f.Name).ToListAsync();
        }
        //******READ*******
        public async Task<List<InventoryItemDto>> GetInventoryDetailsAsync()
        {
            var banks = await _context.Definition_Bank_Account
                .Select(b => new InventoryItemDto
                {
                    Type = "Bank",
                    Id = b.Id,
                    Name = b.Bank.Name,
                    AccountNumber = b.AccountNumber,
                    Inventory = b.Inventory
                })
                .ToListAsync();

            var funds = await _context.Fund
                .Where(x => !x.IsDelete)
                .Select(f => new InventoryItemDto
                {
                    Type = "Fund",
                    Id = f.Id,
                    Name = f.Name,
                    AccountNumber = null,
                    Inventory = f.Inventory
                })
                .ToListAsync();

            return banks.Concat(funds).ToList();
        }
        public async Task<IEnumerable<BusinessEntity.Fund.Fund>> GetAll()
        {
            return await _context.Fund.OrderBy(f => f.Name).ToListAsync();
        }
        public async Task<BusinessEntity.Fund.Fund?> GetById(int id)
        {
            return await _context.Fund.FindAsync(id);
        }
        //****** CREATE *****
        public async Task<string> Create(int UserId, BusinessEntity.Fund.Fund fund)
        {
            if (fund == null)
                return "داده ارسال نشده است.";

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("Starting creation of fund: {@fund}", fund);

                bool nameExists = await _context.Fund
                    .AnyAsync(b => b.Name.Trim().ToLower() == fund.Name.Trim().ToLower());
                if (nameExists)
                    return "نام وارد شده تکراری است.";

                bool accountExists = await _context.Account
                    .AnyAsync(a => a.AccountName.Trim().ToLower() == fund.Name.Trim().ToLower());
                if (accountExists)
                    return "حساب مالی با این نام قبلاً ثبت شده است.";

                var account = new BusinessEntity.Financial_Operations.Account
                {
                    AccountName = fund.Name,
                    AccountType = "CashBox",
                    Balance = fund.Inventory
                };

                fund.Account = account;

                await _context.Fund.AddAsync(fund);
                await _context.SaveChangesAsync();

                var log = new BusinessEntity.Settings.LogUser
                {
                    Description = $"ثبت صندوق با نام {fund.Name} و ایجاد حساب مالی مرتبط",
                    UserId = UserId,
                    Date = DateTime.UtcNow
                };

                await _context.LogUser.AddAsync(log);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return "عملیات با موفقیت انجام شد.";
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync();
                _logger.LogError(dbEx, "EF Core update error while adding Fund: {@fund}", fund);
                return "خطایی در ذخیره اطلاعات رخ داد (DB Update Exception). لطفاً اطلاعات را بررسی کنید.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Unexpected error while adding Fund: {@fund}", fund);
                return $"خطایی در ذخیره اطلاعات رخ داد: {ex.Message}";
            }
        }
        public async Task<string> Update(int UserId, BusinessEntity.Fund.Fund fund)
        {
            try
            {
                if (fund == null)
                    throw new ArgumentNullException(nameof(fund));

                if (await _context.Fund.AnyAsync(i => i.Name == fund.Name && i.Id != fund.Id))
                    return "نام وارد شده تکراری است.";

                var existing = await _context.Fund
                    .Include(f => f.Account)
                    .FirstOrDefaultAsync(f => f.Id == fund.Id);

                if (existing == null)
                    return "شناسه وارد شده با شناسه ذخیره در سیستم مطابقت ندارد.";

                existing.Name = fund.Name;
                existing.Description = fund.Description;
                existing.Inventory = fund.Inventory;
                existing.NegativeBalancePolicy = fund.NegativeBalancePolicy;

                if (existing.Account != null)
                {
                    existing.Account.AccountName = fund.Name;
                    existing.Account.Balance = fund.Inventory;
                }

                await _context.LogUser.AddAsync(new BusinessEntity.Settings.LogUser
                {
                    Description = $"ویرایش صندوق با نام {fund.Name}",
                    UserId = UserId,
                    Date = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
                return "عملیات با موفقیت انجام شد.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Fund: {@Fund}", fund);
                return $"خطایی در ذخیره تغییرات رخ داد.{ex.Message}";
            }
        }
        public async Task<string> Delete(int UserId, int id)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var entity = await _context.Fund
                    .Include(f => f.Account)
                    .FirstOrDefaultAsync(f => f.Id == id);

                if (entity == null)
                    return "درخواست مورد نظر شما یافت نشد.";

                // بررسی وجود تراکنش برای حساب صندوق
                bool hasTransaction = await _context.Transaction
                    .AnyAsync(t => t.AccountId == entity.AccountId);
                if (hasTransaction)
                    return "امکان حذف صندوق وجود ندارد، زیرا تراکنش‌هایی برای آن ثبت شده است.";

                await _context.LogUser.AddAsync(new BusinessEntity.Settings.LogUser
                {
                    Description = $"حذف صندوق با نام {entity.Name}",
                    UserId = UserId,
                    Date = DateTime.UtcNow
                });

                if (entity.Account != null)
                    _context.Account.Remove(entity.Account);

                _context.Fund.Remove(entity);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return "عملیات با موفقیت انجام شد.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error deleting Fund with ID: {Id}", id);
                return $"خطایی در عملیات حذف رخ داد: {ex.Message}";
            }
        }
    }
}
