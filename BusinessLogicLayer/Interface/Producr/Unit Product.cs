using DataAccessLayer;
using BusinessEntity.Product;

namespace BusinessLogicLayer.Interface.Producr
{
    public interface IUnitProductService
    {
        Task<IEnumerable<Unit_Product>> GetAll();
        Task<Unit_Product?> GetById(int id);
        Task<Result> Create(Unit_Product Unit_Product, int UserId);
        Task<Result> Update(Unit_Product Unit_Product, int UserId);
        Task<Result> Delete(int Id, int UserId);
    }
}
