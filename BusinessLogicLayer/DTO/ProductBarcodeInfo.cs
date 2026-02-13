using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class ProductBarcodeInfoDto
    {
        public string Barcode { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int UnitId { get; set; }
        public string UnitName { get; set; } = string.Empty;
        public decimal ConversionFactor { get; set; }
        public int BaseProductId { get; set; }
        public int BaseUnitId { get; set; }
        public int OriginalPrice { get; set; }
        public int FinalPrice { get; set; }
        public DiscountDetailDto? Discounts { get; set; }
        public int Stock { get; set; }
        public string ImagePath { get; set; } = string.Empty;
    }
}
