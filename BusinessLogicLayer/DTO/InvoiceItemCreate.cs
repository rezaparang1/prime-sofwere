using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class InvoiceItemCreateDto
    {
        public string Barcode { get; set; } = string.Empty;  // بارکد واحد کالا
        public int Quantity { get; set; }
    }
}
