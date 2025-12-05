using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public class ProductDtoForApi
    {
        public string Name { get; set; } = string.Empty;
        public int TypeProductId { get; set; }
        public int UnitProductId { get; set; }
        public int SectionProductId { get; set; }
        public int StoreroomProductId { get; set; }
        public int GroupProductId { get; set; }

        public decimal BuyPrice { get; set; }
        public decimal Profit { get; set; }
        public decimal SalePrice { get; set; }

        public bool IsActive { get; set; }
        public bool IsTax { get; set; }
        public decimal Tax { get; set; }
        public bool IsWeighty { get; set; }
        public bool IsIsButton { get; set; }
        public bool IsBarcode { get; set; }

        public int Inventory { get; set; }
        public int MinInventory { get; set; }
        public int MaxInventory { get; set; }

        public string Description { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public string ShortcutKey { get; set; } = string.Empty;

        public List<UnitsLevelDtoForApi> Units { get; set; } = new();
    }
}
