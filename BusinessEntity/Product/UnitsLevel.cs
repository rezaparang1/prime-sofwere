using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessEntity.Product
{
    public class UnitsLevel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        [JsonIgnore]
        public int ProductId { get; set; }
        public Product? Product { get; set; } = null!;
        [JsonIgnore]
        public int UnitProductId { get; set; }
        public Unit_Product? UnitProduct { get; set; } = null!;

        public decimal ConversionFactor { get; set; }

        public ICollection<ProductBarcodes> Barcodes { get; set; } = new List<ProductBarcodes>();
        public ICollection<ProductPrices> Prices { get; set; } = new List<ProductPrices>();
    }
}
