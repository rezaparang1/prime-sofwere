using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class InvoiceItem
    {
        public string Barcode { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public int OriginalPrice { get; set; }
        public int PublicDiscount { get; set; }
        public int ClubDiscount { get; set; }
        public int LevelDiscount { get; set; }
        public int FinalPrice { get; set; }
    }
}
