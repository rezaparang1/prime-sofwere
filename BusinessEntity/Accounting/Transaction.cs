using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Invoices
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        public int AccountId { get; set; }
        public Account Account { get; set; } = null!;

        public long Amount { get; set; }
        public string Type { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string RelatedDocumentType { get; set; } = string.Empty;
        public int? RelatedDocumentId { get; set; }
        public string? PaymentMethod { get; set; } = null;

        public int? RelatedAccountId { get; set; } = null;

        public int? WorkShiftId { get; set; }
        public Fund.Work_Shift? WorkShift { get; set; }
    }
}
