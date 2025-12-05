using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class PriceDto
    {
        public int PriceLevelId { get; set; }
        public string PriceLevelName { get; set; } = string.Empty;
        public decimal BuyPrice { get; set; }
        public decimal Profit { get; set; }
        public decimal SalePrice { get; set; }
    }
}
