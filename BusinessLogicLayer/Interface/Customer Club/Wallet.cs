using BusinessEntity.Customer_Club;
using BusinessLogicLayer.DTO;

namespace BusinessLogicLayer.Interface.Customer_Club
{
    public interface IWalletService
    {
        Task<Result<IEnumerable<WalletTransactionDto>>> SearchTransactionsAsync(
     string? customerName = null,
     DateTime? fromDate = null,
     DateTime? toDate = null,
     int? customerId = null);
        Task<Result<WalletDto>> GetWalletByCustomerIdAsync(int customerId);
        Task<Result<decimal>> GetBalanceAsync(int customerId);
        Task<Result> DepositAsync(int customerId, decimal amount, string description, int? invoiceId = null);
        Task<Result> WithdrawAsync(int customerId, decimal amount, string description, int? invoiceId = null);
        Task<Result> RefundClubDiscountAsync(int customerId, int amount, string description, int clubDiscountId, int invoiceId);
        Task<Result<IEnumerable<WalletTransactionDto>>> GetTransactionsAsync(int customerId, int? count = null);
    }

}
