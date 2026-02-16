using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class PurchaseReturnCreateDto
    {
        public int OriginalInvoiceId { get; set; }      // شناسه فاکتور خرید اصلی
        public DateTime Date { get; set; }               // تاریخ برگشت
        public int UserId { get; set; }                  // کاربر انجام‌دهنده
        public List<PurchaseReturnItemDto> Items { get; set; } = new();
        public List<PaymentDetailDto> RefundPayments { get; set; } = new(); // روش‌های دریافت وجه
    }

    public class PurchaseReturnItemDto
    {
        public string Barcode { get; set; } = string.Empty;   // بارکد واحد کالا
        public int Quantity { get; set; }                      // تعداد برگشتی
    }
}
