using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.DTO.Product
{
    public class ProductPrices
    {
        public Guid TempId { get; set; } = Guid.NewGuid();
        public int PriceLevelId { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal Profit { get; set; }
        public decimal SalePrice { get; set; }
        public int UnitLevelId { get; set; } = 0;

    }
}
