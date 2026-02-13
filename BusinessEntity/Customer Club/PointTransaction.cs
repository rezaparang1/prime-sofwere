using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Customer_Club
{
    public class PointTransaction
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int Points { get; set; }
        public PointTransactionType Type { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Description { get; set; }
        public int? InvoiceId { get; set; }
        public DateTime? ExpireDate { get; set; } // تاریخ انقضای امتیاز

        public virtual Customer? Customer { get; set; }
        public virtual Invoices.Invoices? Invoice { get; set; }
    }
}
