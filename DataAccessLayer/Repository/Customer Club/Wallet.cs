using BusinessEntity.Customer_Club;
using DataAccessLayer.Interface.Customer_Club;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Customer_Club
{
    public class WalletRepository : Repository<Wallet>, IWalletRepository
    {
        public WalletRepository(Database context) : base(context)
        {
        }

        public async Task<IEnumerable<WalletTransaction>> SearchTransactionsAsync(
    string? customerName = null,
    DateTime? fromDate = null,
    DateTime? toDate = null,
    int? customerId = null)
        {
            var query = _context.WalletTransaction
                .Include(t => t.Wallet)
                    .ThenInclude(w => w.Customer)
                .Include(t => t.Invoice)
                .AsQueryable();

            // فیلتر بر اساس شناسه مشتری (دقیق)
            if (customerId.HasValue)
            {
                query = query.Where(t => t.Wallet.CustomerId == customerId.Value);
            }
            // فیلتر بر اساس نام مشتری (جزئی)
            else if (!string.IsNullOrWhiteSpace(customerName))
            {
                query = query.Where(t =>
                    (t.Wallet.Customer.FirstName + " " + t.Wallet.Customer.LastName).Contains(customerName));
            }

            // فیلتر بر اساس بازه تاریخ
            if (fromDate.HasValue)
                query = query.Where(t => t.TransactionDate >= fromDate.Value);
            if (toDate.HasValue)
                query = query.Where(t => t.TransactionDate <= toDate.Value);

            return await query.OrderByDescending(t => t.TransactionDate).ToListAsync();
        }
        public async Task<Wallet?> GetByCustomerIdAsync(int customerId)
        {
            return await _dbSet
                .Include(w => w.Customer)
                .FirstOrDefaultAsync(w => w.CustomerId == customerId);
        }

        public async Task<Wallet?> GetWithTransactionsAsync(int id)
        {
            return await _dbSet
                .Include(w => w.Customer)
                .Include(w => w.Transactions)
                    .ThenInclude(t => t.Invoice)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<decimal> GetBalanceAsync(int customerId)
        {
            var wallet = await GetByCustomerIdAsync(customerId);
            return wallet?.Balance ?? 0;
        }

        public async Task<IEnumerable<WalletTransaction>> GetTransactionsByWalletIdAsync(int walletId, int? count = null)
        {
            IQueryable<WalletTransaction> query = _context.WalletTransaction  // دقت: اینجا DbSet شما باید WalletTransactions باشد
                .Include(t => t.Invoice)
                .Include(t => t.ClubDiscount)
                .Where(t => t.WalletId == walletId)
                .OrderByDescending(t => t.TransactionDate);   // اینجا IOrderedQueryable است اما به IQueryable نسبت داده می‌شود (کاملاً مجاز)

            if (count.HasValue)
            {
                query = query.Take(count.Value);   // حالا query از نوع IQueryable است و مشکلی نیست
            }

            return await query.ToListAsync();
        }
        public async Task<IEnumerable<WalletTransaction>> GetTransactionsByDateRangeAsync(int walletId, DateTime startDate, DateTime endDate)
        {
            return await _context.WalletTransaction
                .Include(t => t.Invoice)
                .Where(t => t.WalletId == walletId &&
                           t.TransactionDate >= startDate &&
                           t.TransactionDate <= endDate)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<WalletTransaction>> GetTransactionsByTypeAsync(int walletId, TransactionType type)
        {
            return await _context.WalletTransaction
                .Include(t => t.Invoice)
                .Where(t => t.WalletId == walletId && t.Type == type)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalDepositAsync(int walletId)
        {
            return await _context.WalletTransaction
                .Where(t => t.WalletId == walletId && t.Amount > 0)
                .SumAsync(t => t.Amount);
        }

        public async Task<decimal> GetTotalWithdrawAsync(int walletId)
        {
            var withdrawals = await _context.WalletTransaction
                .Where(t => t.WalletId == walletId && t.Amount < 0)
                .SumAsync(t => t.Amount);

            return Math.Abs(withdrawals);
        }

        public async Task<int> GetTransactionCountAsync(int walletId)
        {
            return await _context.WalletTransaction
                .CountAsync(t => t.WalletId == walletId);
        }
    }
}
