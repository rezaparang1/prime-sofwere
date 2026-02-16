using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class SalesReturnDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public int OriginalInvoiceId { get; set; }
        public string OriginalInvoiceNumber { get; set; } = string.Empty;
        public int PeopleId { get; set; }
        public string PeopleName { get; set; } = string.Empty;
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public int TotalRefundAmount { get; set; }          // کل مبلغ برگشتی
        public List<SalesReturnItemDto> Items { get; set; } = new();
        // در صورت نیاز می‌توانید تراکنش‌های مالی را نیز اضافه کنید
    }
}
