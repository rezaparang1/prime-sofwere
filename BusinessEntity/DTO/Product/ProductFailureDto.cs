using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.DTO.Product
{
    public class ProductFailureItemDto
    {
        public int FailureId { get; set; }           // شماره مرجوعی
        public DateTime Date { get; set; }           // تاریخ مرجوعی
        public string StoreroomName { get; set; } = string.Empty; // نام انبار
        public string Barcode { get; set; } = string.Empty;       // بارکد کالا
        public string ProductName { get; set; } = string.Empty;  // نام کالا
        public int Quantity { get; set; }                        // مقدار کالا
    }
}
