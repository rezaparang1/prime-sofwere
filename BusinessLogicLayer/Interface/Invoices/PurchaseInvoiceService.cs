using BusinessLogicLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface.Invoices
{
    public interface IPurchaseInvoiceService
    {
        Task<Result<PurchaseInvoiceDto>> CreatePurchaseInvoiceAsync(PurchaseInvoiceCreateDto dto);
        Task<Result<PurchaseInvoiceDto>> GetPurchaseInvoiceByIdAsync(int id);
    }
}
