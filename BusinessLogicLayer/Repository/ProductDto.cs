using DataAccessLayer.Interface.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repository
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;

        public int UnitId { get; set; }
        public int UnitProductId { get; set; }
        public decimal UnitConversionFactor { get; set; }

        public List<string> Barcodes { get; set; } = new List<string>();
        public List<PriceDto> Prices { get; set; } = new List<PriceDto>();
    }
}
