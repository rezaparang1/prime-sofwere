using BusinessEntity.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface.Fund
{
    public interface IFundService
    {
        Task<List<BusinessEntity.Fund.Fund>> Search(string? name = null);
        Task<List<InventoryItemDto>> GetInventoryDetails();
        Task<IEnumerable<BusinessEntity.Fund.Fund>> GetAll();
        Task<BusinessEntity.Fund.Fund?> GetById(int id);
        Task<string> Create(int UserId, BusinessEntity.Fund.Fund Fund);
        Task<string> Update(int UserId, BusinessEntity.Fund.Fund Fund);
      
        Task<string> Delete(int UserId ,int id);
    }
}
