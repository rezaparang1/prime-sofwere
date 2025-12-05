
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.DTO.Product
{
    public class UnitsLevel
    {
        public Guid TempId { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public int UnitProductId { get; set; }
        public decimal ConversionFactor { get; set; }
        public int ProductId { get; set; }
        public List<ProductBarcodes> Barcodes { get; set; } = new List<ProductBarcodes>();
        public List<ProductPrices> Prices { get; set; } = new List<ProductPrices>();
    }
}
