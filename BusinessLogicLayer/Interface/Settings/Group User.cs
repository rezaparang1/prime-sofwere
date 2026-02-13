using BusinessEntity.Settings;

namespace BusinessLogicLayer.Interface.Settings
{
    public interface IGroupUserService
    {
        Task<IEnumerable<Group_User>> GetAll();
        Task<Group_User?> GetById(int id);
        Task<Result> Create(Group_User Group_User, int UserId);
        Task<Result> Update(Group_User Group_User, int UserId);
        Task<Result> Delete(int Id, int UserId);
    }
}
