using BusinessLogicLayer.DTO;


namespace BusinessLogicLayer.Interface.Invoices
{
    public interface IInvoiceService
    {
        Task<Result<InvoiceDto>> CreateInvoiceWithAllDiscountsAsync(InvoiceCreateDto dto);
        Task<Result<InvoiceDto>> GetInvoiceByIdAsync(int id);
        Task<Result> UseWalletForPaymentAsync(int invoiceId, int customerId, int walletAmount);
    }
}
