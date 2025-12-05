using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface.Invoices
{
    public interface IInvoicesService
    {

        Task<string> Create(BusinessEntity.Invoices.Invoices invoice);
    }
}
