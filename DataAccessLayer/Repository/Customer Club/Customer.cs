using BusinessEntity.Customer_Club;
using BusinessEntity.Fund;
using DataAccessLayer.Interface.Customer_Club;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Customer_Club
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(Database context) : base(context)
        {
        }

        public async Task<Customer?> GetByBarcodeAsync(string barcode)
        {
            return await _dbSet
                .Include(c => c.Wallet)
                .Include(c => c.CustomerLevel)
                .Include(c => c.Store)
                .FirstOrDefaultAsync(c => c.Barcode == barcode);
        }

        public async Task<Customer?> GetByMobileAsync(string mobile)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.Mobile == mobile);
        }

        public async Task<Customer?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<Customer?> GetWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(c => c.Wallet)
                    .ThenInclude(w => w.Transactions)
                .Include(c => c.CustomerLevel)
                .Include(c => c.Store)
                .Include(c => c.LevelHistories)
                    .ThenInclude(lh => lh.CustomerLevel)
                .Include(c => c.PointTransactions)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Customer>> GetCustomersByLevelAsync(int levelId)
        {
            return await _dbSet
                .Where(c => c.CustomerLevelId == levelId && c.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Customer>> GetCustomersByStoreAsync(int storeId)
        {
            return await _dbSet
                .Where(c => c.StoreId == storeId && c.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Customer>> GetActiveCustomersAsync()
        {
            return await _dbSet
                .Where(c => c.IsActive)
                .OrderByDescending(c => c.RegisterDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Customer>> GetClubMembersAsync()
        {
            return await _dbSet
                .Where(c => c.IsClubMember && c.IsActive)
                .Include(c => c.CustomerLevel)
                .OrderByDescending(c => c.TotalPurchaseAmount)
                .ToListAsync();
        }

        public async Task<decimal> GetCustomerTotalPurchaseAsync(int customerId)
        {
            return await _context.Invoices
                .Where(i => i.CustomerId == customerId &&
                           i.TypeInvoices == BusinessEntity.Invoices.Type_Invices.Sales_Invoice)
                .SumAsync(i => i.TotalSum);
        }

        public async Task<int> GetCustomerPurchaseCountAsync(int customerId)
        {
            return await _context.Invoices
                .CountAsync(i => i.CustomerId == customerId &&
                                i.TypeInvoices == BusinessEntity.Invoices.Type_Invices.Sales_Invoice);
        }

        public async Task<int> GetCustomerPointsAsync(int customerId)
        {
            var earned = await _context.PointTransaction
                .Where(pt => pt.CustomerId == customerId && pt.Type == PointTransactionType.Earn)
                .SumAsync(pt => pt.Points);

            var used = await _context.PointTransaction
                .Where(pt => pt.CustomerId == customerId && pt.Type == PointTransactionType.Redeem)
                .SumAsync(pt => pt.Points);

            return earned - used;
        }

        public async Task<bool> IsMobileExistsAsync(string mobile, int? excludeCustomerId = null)
        {
            var query = _dbSet.Where(c => c.Mobile == mobile);

            if (excludeCustomerId.HasValue)
            {
                query = query.Where(c => c.Id != excludeCustomerId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<bool> IsEmailExistsAsync(string email, int? excludeCustomerId = null)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var query = _dbSet.Where(c => c.Email == email);

            if (excludeCustomerId.HasValue)
            {
                query = query.Where(c => c.Id != excludeCustomerId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<bool> IsBarcodeExistsAsync(string barcode, int? excludeCustomerId = null)
        {
            var query = _dbSet.Where(c => c.Barcode == barcode);

            if (excludeCustomerId.HasValue)
            {
                query = query.Where(c => c.Id != excludeCustomerId.Value);
            }

            return await query.AnyAsync();
        }
    }
}
