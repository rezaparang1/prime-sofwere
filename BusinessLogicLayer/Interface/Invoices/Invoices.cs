using BusinessLogicLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface.Invoices
{
    public interface IInvoiceService
    {
        Task<Result<InvoiceDto>> CreateInvoiceWithAllDiscountsAsync(InvoiceCreateDto dto);
        Task<Result<InvoiceDto>> GetInvoiceByIdAsync(int id);
        Task<Result> UseWalletForPaymentAsync(int invoiceId, int customerId, int walletAmount);
    }
}
