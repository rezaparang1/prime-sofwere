namespace Prime_Software.DTO.Invoices
{
    public class InvoiceDto
    {
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime Date { get; set; }

        public int PeopleId { get; set; }
        public int UserId { get; set; }

        public Type_Invices TypeInvoices { get; set; }
        public Type_Pay TyepPay { get; set; }

        public int NumberofAllItems { get; set; }
        public int OffAll { get; set; }
        public int TotalSum { get; set; }
        public bool IsUpdate { get; set; }

        public List<InvoiceItemDto> Invoices_Item { get; set; } = new();
    }
}
