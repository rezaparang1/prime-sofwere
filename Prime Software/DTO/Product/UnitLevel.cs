using DataAccessLayer.Interface.Product;

namespace Prime_Software.DTO.Product
{
    public class UnitLevel
    {
        public string Title { get; set; } = string.Empty;
        public int UnitProductId { get; set; }
        public decimal ConversionFactor { get; set; }
        public List<Price> Prices { get; set; } = new();
        public List<Barcodes> Barcodes { get; set; } = new();
    }
}
