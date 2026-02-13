using DataAccessLayer;
using BusinessEntity.Product;

namespace BusinessLogicLayer.Interface.Producr
{
    public interface ISectionProductService
    {
        Task<IEnumerable<Section_Product>> GetAll();
        Task<Section_Product?> GetById(int id);
        Task<Result> Create(Section_Product Section_Product, int UserId);
        Task<Result> Update(Section_Product Section_Product, int UserId);
        Task<Result> Delete(int Id, int UserId);
    }
}
