using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class PurchaseInvoiceCreateDto
    {
        public DateTime Date { get; set; }
        public int PeopleId { get; set; }          // تأمین‌کننده
        public int? CustomerId { get; set; }        // (اختیاری) اگر تأمین‌کننده عضو باشگاه باشد – در خرید معمولاً کاربردی ندارد
        public int UserId { get; set; }
        public BusinessEntity.Invoices.Type_Pay TypePay { get; set; }       // نوع پرداخت اصلی (برای سازگاری، اما از Payments استفاده می‌کنیم)
        public int StoreId { get; set; }
        public List<PurchaseInvoiceItemCreateDto> Items { get; set; } = new();
        public List<PaymentDetailDto> Payments { get; set; } = new(); // همان DTO قبلی
    }
}
