using BusinessLogicLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface.Invoices
{
    public interface ISalesReturnService
    {
        Task<Result<SalesReturnDto>> CreateSalesReturnAsync(SalesReturnCreateDto dto);
        Task<Result<SalesReturnDto>> GetSalesReturnByIdAsync(int id);
    }
}
