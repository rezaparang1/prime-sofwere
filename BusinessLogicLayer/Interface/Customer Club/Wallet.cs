using BusinessLogicLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface.Customer_Club
{
    public interface IWalletService
    {
        Task<Result<WalletDto>> GetWalletByCustomerIdAsync(int customerId);
        Task<Result<decimal>> GetBalanceAsync(int customerId);
        Task<Result> DepositAsync(int customerId, decimal amount, string description, int? invoiceId = null);
        Task<Result> WithdrawAsync(int customerId, decimal amount, string description, int? invoiceId = null);
        Task<Result> RefundClubDiscountAsync(int customerId, int amount, string description, int clubDiscountId, int invoiceId);
        Task<Result<IEnumerable<WalletTransactionDto>>> GetTransactionsAsync(int customerId, int? count = null);
    }

}
