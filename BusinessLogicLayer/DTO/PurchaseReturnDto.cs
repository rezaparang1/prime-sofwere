using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class PurchaseReturnDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public int OriginalInvoiceId { get; set; }
        public string OriginalInvoiceNumber { get; set; } = string.Empty;
        public int PeopleId { get; set; }
        public string PeopleName { get; set; } = string.Empty;   // تأمین‌کننده
        public int? CustomerId { get; set; }                       // (در خرید معمولاً null)
        public int TotalRefundAmount { get; set; }                // کل مبلغ دریافتی
        public List<PurchaseReturnItemDto> Items { get; set; } = new();
    }
}
