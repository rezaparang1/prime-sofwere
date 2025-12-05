using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessEntity.Product
{
    public class ProductPrices
    {
        public int Id { get; set; }
        [JsonIgnore]
        public int UnitLevelId { get; set; }
        public UnitsLevel? ProductUnit { get; set; } = null!;
        [JsonIgnore]
        public int PriceLevelId { get; set; }
        public PriceLevels? PriceLevel { get; set; } = null!;

        public decimal BuyPrice { get; set; }
        public decimal Profit { get; set; }
        public decimal SalePrice { get; set; }
    }
}
