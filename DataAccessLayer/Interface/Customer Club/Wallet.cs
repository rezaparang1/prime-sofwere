using BusinessEntity.Customer_Club;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interface.Customer_Club
{
    public interface IWalletRepository : IRepository<Wallet>
    {
        Task<IEnumerable<WalletTransaction>> SearchTransactionsAsync(
    string? customerName = null,
    DateTime? fromDate = null,
    DateTime? toDate = null,
    int? customerId = null);
        Task<Wallet?> GetByCustomerIdAsync(int customerId);
        Task<Wallet?> GetWithTransactionsAsync(int id);
        Task<decimal> GetBalanceAsync(int customerId);
        Task<IEnumerable<WalletTransaction>> GetTransactionsByWalletIdAsync(int walletId, int? count = null);
        Task<IEnumerable<WalletTransaction>> GetTransactionsByDateRangeAsync(int walletId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<WalletTransaction>> GetTransactionsByTypeAsync(int walletId, TransactionType type);
        Task<decimal> GetTotalDepositAsync(int walletId);
        Task<decimal> GetTotalWithdrawAsync(int walletId);
        Task<int> GetTransactionCountAsync(int walletId);
    }
}
