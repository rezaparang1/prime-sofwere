using BusinessEntity.Fund;

namespace BusinessLogicLayer.Interface.Fund
{
    public interface IDefinitionBankService
    {
        Task<IEnumerable<Definition_Bank>> GetAll();
        Task<Definition_Bank?> GetById(int id);
        Task<Result> Create(Definition_Bank Definition_Bank , int UserId);
        Task<Result> Update(Definition_Bank Definition_Bank ,int UserId);
        Task<Result> Delete(int Id ,int UserId);
    }
}
