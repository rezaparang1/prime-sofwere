using BusinessEntity.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessEntity.Product
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [JsonIgnore]
        public int TypeProductId { get; set; }
        public Type_Product? TypeProduct { get; set; } = null!;
        [JsonIgnore]
        public int UnitProductId { get; set; }
        public Unit_Product? Unit_Product { get; set; } = null!;
        [JsonIgnore]
        public int SectionProductId { get; set; }
        public Section_Product? SectionProduct { get; set; } = null!;
        [JsonIgnore]
        public int StoreroomProductId { get; set; }
        public Storeroom_Product? StoreroomProduct { get; set; } = null!;
        [JsonIgnore]
        public int GroupProductId { get; set; }
        public Group_Product? GroupProduct { get; set; } = null!;

        public decimal BuyPrice { get; set; }
        public decimal Profit { get; set; }
        public decimal SalePrice { get; set; }
        public bool IsDelete { get; set; }
        public bool IsActive { get; set; }
        public bool IsTax { get; set; }
        public decimal Tax { get; set; }
        public bool IsWeighty { get; set; }
        public bool IsIsButton { get; set; }
        public bool IsBarcode { get; set; }

        public int Inventory { get; set; }
        public int MinInventory { get; set; }
        public int MaxInventory { get; set; }
        public DateTime Date {  get; set; }
        public string Description { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public string ShortcutKey { get; set; } = string.Empty;

        public ICollection<BusinessEntity.Invoices.Invoices_Item> Invoices_Items { get; set; } = new List<BusinessEntity.Invoices.Invoices_Item>();
        public ICollection<UnitsLevel> Units { get; set; } = new List<UnitsLevel>();
    }
}
