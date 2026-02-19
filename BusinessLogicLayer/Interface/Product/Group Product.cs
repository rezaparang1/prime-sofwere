using BusinessEntity.Product;

namespace BusinessLogicLayer.Interface.Producr
{
    public interface IGroupProductService
    {
        Task<IEnumerable<Group_Product>> GetAll();
        Task<Group_Product?> GetById(int id);
        Task<Result> Create(Group_Product Group_People, int UserId);
        Task<Result> Update(Group_Product Group_People, int UserId);
        Task<Result> Delete(int Id, int UserId);
    }
}
