using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class PurchaseInvoiceItemCreateDto
    {
        public string Barcode { get; set; } = string.Empty;   // بارکد واحد کالا
        public int Quantity { get; set; }
        public int BuyPrice { get; set; }                      // قیمت خرید جدید (تومان)
        public int SalePrice { get; set; }                     // قیمت فروش جدید (اختیاری – می‌توانید nullable کنید)
    }
}
