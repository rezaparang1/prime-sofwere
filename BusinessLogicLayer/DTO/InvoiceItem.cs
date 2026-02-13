using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class InvoiceItemDto
    {
        public string Barcode { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string UnitName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public int OriginalPrice { get; set; }
        public int PublicDiscount { get; set; }
        public int ClubDiscount { get; set; }
        public int LevelDiscount { get; set; }
        public int FinalPrice { get; set; }
    }
}
