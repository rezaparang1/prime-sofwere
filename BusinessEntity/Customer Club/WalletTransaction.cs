using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Customer_Club
{
    public class WalletTransaction
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public decimal Amount { get; set; } // مثبت: واریز، منفی: برداشت
        public TransactionType Type { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Description { get; set; }
        public int? InvoiceId { get; set; }
        public int? ClubDiscountId { get; set; }

        public virtual ClubDiscount? ClubDiscount { get; set; }
        public virtual Wallet? Wallet { get; set; }
        public virtual Invoices.Invoices? Invoice { get; set; }
    }
}
