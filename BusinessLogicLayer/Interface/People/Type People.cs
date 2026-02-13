using BusinessEntity.People;

namespace BusinessLogicLayer.Interface.People
{
    public interface ITypePeopleService
    {
        Task<IEnumerable<Type_People>> GetAll();
        Task<Type_People?> GetById(int id);
        Task<Result> Create(Type_People Type_People, int UserId);
        Task<Result> Update(Type_People Type_People, int UserId);
        Task<Result> Delete(int Id, int UserId);
    }
}
