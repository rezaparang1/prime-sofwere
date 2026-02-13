using BusinessEntity.DTO.Fund;
using BusinessEntity.Fund;
using DataAccessLayer;
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
        Task<Result> Create(BusinessEntity.Fund.Fund fund, int userId);
        Task<Result> Update(BusinessEntity.Fund.Fund fund, int userId);
        Task<Result> Delete(int id, int userId);
    }
}
