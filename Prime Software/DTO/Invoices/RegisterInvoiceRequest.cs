namespace Prime_Software.DTO.Invoices
{
    public class RegisterInvoiceRequest
    {
        public InvoiceDto Invoice { get; set; } = null!;
        public List<PaymentDto> Payments { get; set; } = new();
    }
}
