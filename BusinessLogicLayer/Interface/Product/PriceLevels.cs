using BusinessEntity.Product;

namespace BusinessLogicLayer.Interface.Product
{
    public interface IPriceLevelsService
    {
        Task<IEnumerable<PriceLevels>> GetAll();
        Task<PriceLevels?> GetById(int id);
        Task<Result> Create(PriceLevels PriceLevels, int UserId);
        Task<Result> Update(PriceLevels PriceLevels, int UserId);
        Task<Result> Delete(int Id, int UserId);
    }
}
