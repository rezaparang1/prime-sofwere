using BusinessEntity.People;

namespace BusinessLogicLayer.Interface.People
{
    public interface IGroupPeopleService
    {
        Task<IEnumerable<Group_People>> GetAll();
        Task<Group_People?> GetById(int id);
        Task<Result> Create(Group_People Group_People, int UserId);
        Task<Result> Update(Group_People Group_People, int UserId);
        Task<Result> Delete(int Id, int UserId);
    }
}
