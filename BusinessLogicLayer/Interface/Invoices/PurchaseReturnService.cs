using BusinessLogicLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface.Invoices
{
    public interface IPurchaseReturnService
    {
        Task<Result<PurchaseReturnDto>> CreatePurchaseReturnAsync(PurchaseReturnCreateDto dto);
        Task<Result<PurchaseReturnDto>> GetPurchaseReturnByIdAsync(int id);
    }
}
