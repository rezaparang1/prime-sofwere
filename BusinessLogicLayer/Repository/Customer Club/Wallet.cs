using BusinessEntity.Customer_Club;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Interface.Customer_Club;
using DataAccessLayer.Interface.Customer_Club;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repository.Customer_Club
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WalletService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<WalletDto>> GetWalletByCustomerIdAsync(int customerId)
        {
            var wallet = await _unitOfWork.Wallets.GetByCustomerIdAsync(customerId);
            if (wallet == null)
                return Result<WalletDto>.Failure("کیف پول یافت نشد");
            return Result<WalletDto>.SuccessResult(MapToDto(wallet));
        }

        public async Task<Result<decimal>> GetBalanceAsync(int customerId)
        {
            var balance = await _unitOfWork.Wallets.GetBalanceAsync(customerId);
            return Result<decimal>.SuccessResult(balance);
        }

        public async Task<Result> DepositAsync(int customerId, decimal amount, string description, int? invoiceId = null)
        {
            if (amount <= 0)
                return Result.Failure("مبلغ واریز باید بزرگتر از صفر باشد");

            var wallet = await _unitOfWork.Wallets.GetByCustomerIdAsync(customerId);
            if (wallet == null)
                return Result.Failure("کیف پول یافت نشد");

            wallet.Balance += amount;
            wallet.LastUpdate = DateTime.Now;

            var transaction = new WalletTransaction
            {
                WalletId = wallet.Id,
                Amount = amount,
                Type = TransactionType.Deposit,
                TransactionDate = DateTime.Now,
                Description = description,
                InvoiceId = invoiceId
            };

            await _unitOfWork.WalletTransactions.AddAsync(transaction);
            _unitOfWork.Wallets.Update(wallet);
            await _unitOfWork.SaveChangesAsync();

            return Result.SuccessResult("واریز با موفقیت انجام شد");
        }

        public async Task<Result> WithdrawAsync(int customerId, decimal amount, string description, int? invoiceId = null)
        {
            if (amount <= 0)
                return Result.Failure("مبلغ برداشت باید بزرگتر از صفر باشد");

            var wallet = await _unitOfWork.Wallets.GetByCustomerIdAsync(customerId);
            if (wallet == null)
                return Result.Failure("کیف پول یافت نشد");

            if (wallet.Balance < amount)
                return Result.Failure("موجودی کیف پول کافی نیست");

            wallet.Balance -= amount;
            wallet.LastUpdate = DateTime.Now;

            var transaction = new WalletTransaction
            {
                WalletId = wallet.Id,
                Amount = -amount,
                Type = TransactionType.Withdraw,
                TransactionDate = DateTime.Now,
                Description = description,
                InvoiceId = invoiceId
            };

            await _unitOfWork.WalletTransactions.AddAsync(transaction);
            _unitOfWork.Wallets.Update(wallet);
            await _unitOfWork.SaveChangesAsync();

            return Result.SuccessResult("برداشت با موفقیت انجام شد");
        }

        public async Task<Result> RefundClubDiscountAsync(int customerId, int amount, string description, int clubDiscountId, int invoiceId)
        {
            var wallet = await _unitOfWork.Wallets.GetByCustomerIdAsync(customerId);
            if (wallet == null)
                return Result.Failure("کیف پول یافت نشد");

            wallet.Balance += amount;
            wallet.LastUpdate = DateTime.Now;

            var transaction = new WalletTransaction
            {
                WalletId = wallet.Id,
                Amount = amount,
                Type = TransactionType.Refund,
                TransactionDate = DateTime.Now,
                Description = description,
                InvoiceId = invoiceId,
                ClubDiscountId = clubDiscountId
            };

            await _unitOfWork.WalletTransactions.AddAsync(transaction);
            _unitOfWork.Wallets.Update(wallet);
            await _unitOfWork.SaveChangesAsync();

            return Result.SuccessResult("تخفیف باشگاه به کیف پول برگشت داده شد");
        }

        public async Task<Result<IEnumerable<WalletTransactionDto>>> GetTransactionsAsync(int customerId, int? count = null)
        {
            var wallet = await _unitOfWork.Wallets.GetByCustomerIdAsync(customerId);
            if (wallet == null)
                return Result<IEnumerable<WalletTransactionDto>>.Failure("کیف پول یافت نشد");

            var transactions = await _unitOfWork.Wallets.GetTransactionsByWalletIdAsync(wallet.Id, count);
            var dtos = transactions.Select(t => new WalletTransactionDto
            {
                Id = t.Id,
                Amount = t.Amount,
                Type = t.Type.ToString(),
                TransactionDate = t.TransactionDate,
                Description = t.Description,
                InvoiceNumber = t.Invoice?.InvoiceNumber
            });

            return Result<IEnumerable<WalletTransactionDto>>.SuccessResult(dtos);
        }

        private WalletDto MapToDto(Wallet wallet)
        {
            return new WalletDto
            {
                Id = wallet.Id,
                CustomerId = wallet.CustomerId,
                Balance = wallet.Balance,
                LastUpdate = wallet.LastUpdate
            };
        }
    }
}
