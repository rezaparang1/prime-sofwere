using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interface.Invoices
{
    public interface IInvoicesRepository
    {
       // Task<List<BusinessEntity.Invoices.Invoices>> Search(int? InvoicesId = null);
        //Task<IEnumerable<BusinessEntity.Invoices.Invoices>> GetAll();
      //  Task<BusinessEntity.Invoices.Invoices?> GetById(int id);
        Task<string> AddInvoice(BusinessEntity.Invoices.Invoices invoice);
       // Task<string> Update(BusinessEntity.Invoices.Invoices Invoices);
    }
}
