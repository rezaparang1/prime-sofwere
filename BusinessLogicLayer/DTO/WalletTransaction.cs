using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class WalletTransactionDto
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }           // شناسه مشتری (اختیاری)
        public string? CustomerName { get; set; }      // نام کامل مشتری
        public decimal Amount { get; set; }
        public string Type { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
        public string? Description { get; set; }
        public string? InvoiceNumber { get; set; }     // شماره فاکتور مرتبط (در صورت وجود)
    }
}
