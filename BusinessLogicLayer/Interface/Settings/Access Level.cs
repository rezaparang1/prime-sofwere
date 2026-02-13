
namespace BusinessLogicLayer.Interface.Settings
{
    public interface IAccessLevelService
    {
        Task<IEnumerable<BusinessEntity.Settings.Access_Level>> GetAll();
        Task<BusinessEntity.Settings.Access_Level?> GetById(int id);
        Task<Result> Create(BusinessEntity.Settings.Access_Level Access_Level);
        Task<Result> Update(BusinessEntity.Settings.Access_Level Access_Level);
        Task<Result> Delete(int id);
    }
}
