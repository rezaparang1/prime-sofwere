using BusinessLogicLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface.Invoices
{
    public interface ITempInvoiceService
    {
        Task SaveItemsAsync(int invoiceId, List<InvoiceItemDto> items);
        Task<List<InvoiceItemDto>> GetItemsAsync(int invoiceId);
        Task RemoveAsync(int invoiceId);
    }
}
