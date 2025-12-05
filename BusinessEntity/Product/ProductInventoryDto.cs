using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Product
{
    public class ProductInventoryDto
    {
        public string Barcode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int Inventory { get; set; }
        public int MinInventory { get; set; }
        public int MaxInventory { get; set; }
    }
}
