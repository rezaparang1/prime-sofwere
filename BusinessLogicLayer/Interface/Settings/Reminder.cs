

namespace BusinessLogicLayer.Interface.Settings
{
    public interface IReminderService
    {
        Task<List<BusinessEntity.Settings.Reminder>> Search(int? UserId, DateTime? Date = null, string? Description = null);
        Task<List<BusinessEntity.Settings.Reminder>> SearchByUserId(int? userId);
        Task<IEnumerable<BusinessEntity.Settings.Reminder>> GetAll();
        Task<BusinessEntity.Settings.Reminder?> GetById(int id);
        Task<Result> Create(BusinessEntity.Settings.Reminder Reminder);
        Task<Result> Update(BusinessEntity.Settings.Reminder Reminder);
        Task<Result> Delete(int id);
    }
}
