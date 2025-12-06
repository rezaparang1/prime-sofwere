using BusinessEntity.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interface.Customer_Club
{
    public interface ICustomerRepository
    {
        Task<List<BusinessEntity.Customer_Club.Customer>> Search(string? name = null);
        Task<IEnumerable<BusinessEntity.Customer_Club.Customer>> GetAll();
        Task<BusinessEntity.Customer_Club.Customer?> GetById(int id);
        Task<string> Create(int UserId, BusinessEntity.Customer_Club.Customer Customer);
        Task<string> Update(int UserId, BusinessEntity.Customer_Club.Customer Customer);
        Task<string> Delete(int UserId, int id);
    }
}
