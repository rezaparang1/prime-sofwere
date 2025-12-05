using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public class UnitsLevelDtoForApi
    {
        public string Title { get; set; } = string.Empty;
        public int UnitProductId { get; set; }
        public decimal ConversionFactor { get; set; }
        public int ProductId { get; set; }
        public List<ProductBarcodeDtoForApi> Barcodes { get; set; } = new();
        public List<ProductPriceDtoForApi> Prices { get; set; } = new();
    }
}
