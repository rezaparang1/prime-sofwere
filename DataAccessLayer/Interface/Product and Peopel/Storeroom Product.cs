using BusinessEntity.DTO.Product;
using BusinessEntity.Product;

namespace DataAccessLayer.Interface.Product
{
        public interface IStoreroomProductRepository
        {
            Task<IEnumerable<Storeroom_Product>> GetAll();
            Task<Storeroom_Product?> GetById(int id);
            Task<Result> Create(Storeroom_Product storeroom);
            Task<Result> Update(Storeroom_Product storeroom);
            Task<Result> Delete(int id);

            Task<List<Storeroom_Product>> Search(
                string? name = null,
                int? sectionProductId = null,
                int? peopleId = null);
        }
}
