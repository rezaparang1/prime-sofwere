using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class InvoiceItemDto
    {
        public string Barcode { get; set; }
        public int Quantity { get; set; }
    }
}
