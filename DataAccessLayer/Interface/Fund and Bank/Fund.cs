
using BusinessEntity.DTO.Fund;

namespace DataAccessLayer.Interface.Fund
{
    public interface IFundRepository
    {
        Task<List<BusinessEntity.Fund.Fund>> Search(string? name = null);
        Task<List<InventoryItemDto>> GetInventoryDetails();
        Task<IEnumerable<BusinessEntity.Fund.Fund>> GetAll();
        Task<BusinessEntity.Fund.Fund?> GetById(int id);
        Task<Result> Create(BusinessEntity.Fund.Fund Fund);
        Task<Result> Update(BusinessEntity.Fund.Fund Fund);
        Task<Result> Delete(int id);
    }
}
