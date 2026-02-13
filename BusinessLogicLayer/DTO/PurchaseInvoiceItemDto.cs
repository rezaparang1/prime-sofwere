using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class PurchaseInvoiceItemDto
    {
        public string Barcode { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string UnitName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int BuyPrice { get; set; }
        public int SalePrice { get; set; }
        public int TotalPrice { get; set; }
    }
}
