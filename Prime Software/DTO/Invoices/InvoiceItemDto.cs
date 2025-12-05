namespace Prime_Software.DTO.Invoices
{
    public class InvoiceItemDto
    {
        public int ProductId { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Number { get; set; }
        public int Price { get; set; }
        public int OFF { get; set; }
        public int AllPrice { get; set; }
    }
}
