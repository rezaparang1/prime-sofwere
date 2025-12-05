namespace Prime_Software.DTO.Product
{
    public class Product
    {
        public string Name { get; set; } = string.Empty;
        public int TypeProductId { get; set; }
        public int UnitProductId { get; set; }
        public int SectionProductId { get; set; }
        public int StoreroomProductId { get; set; }
        public int GroupProductId { get; set; }

        public decimal BuyPrice { get; set; }
        public decimal Profit { get; set; }
        public decimal SalePrice { get; set; }

        public bool IsActive { get; set; }
        public bool IsTax { get; set; }
        public decimal Tax { get; set; }
        public bool IsWeighty { get; set; }
        public bool IsIsButton { get; set; }
        public bool IsBarcode { get; set; }

        public int Inventory { get; set; }
        public int MinInventory { get; set; }
        public int MaxInventory { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<UnitLevel> Units { get; set; } = new();
    }
}
