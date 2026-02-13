using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class PurchaseInvoiceDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int PeopleId { get; set; }
        public string PeopleName { get; set; } = string.Empty;
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public int TotalAmount { get; set; }                   // جمع کل (بر اساس BuyPrice * Quantity)
        public List<PurchaseInvoiceItemDto> Items { get; set; } = new();
        public List<WalletTransactionDto> WalletTransactions { get; set; } = new(); // در صورت استفاده از کیف پول (اختیاری)
        // در صورت نیاز می‌توانید تراکنش‌های مالی را نیز اضافه کنید
    }

}
