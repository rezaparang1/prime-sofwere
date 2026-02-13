using DataAccessLayer;
using BusinessEntity.Product;

namespace BusinessLogicLayer.Interface.Producr
{
    public interface IStoreroomProductService
    {
        Task<IEnumerable<Storeroom_Product>> GetAll();
        Task<Storeroom_Product?> GetById(int id);
        Task<Result> Create(Storeroom_Product storeroom, int userId);
        Task<Result> Update(Storeroom_Product storeroom, int userId);
        Task<Result> Delete(int id, int userId);

        Task<List<Storeroom_Product>> Search(
            string? name = null,
            int? sectionProductId = null,
            int? peopleId = null);
    }
}
