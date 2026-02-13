using BusinessEntity.Product;

namespace BusinessLogicLayer.Interface.Producr
{
    public interface ITypeProductService
    {
        Task<IEnumerable<Type_Product>> GetAll();
        Task<Type_Product?> GetById(int id);
        Task<Result> Create(Type_Product Type_Product, int UserId);
        Task<Result> Update(Type_Product Type_Product, int UserId);
        Task<Result> Delete(int Id, int UserId);
    }
}
