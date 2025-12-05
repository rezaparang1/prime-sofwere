using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface.Settings
{
    public interface IReminderService
    {
        Task<List<BusinessEntity.Settings.Reminder>> Search(int? UserId, DateTime? Date = null, string? Description = null);
        Task<List<BusinessEntity.Settings.Reminder>> SearchByUserId(int? userId);
        Task<IEnumerable<BusinessEntity.Settings.Reminder>> GetAll();
        Task<BusinessEntity.Settings.Reminder?> GetById(int id);
        Task<string> Create(BusinessEntity.Settings.Reminder Reminder);
        Task<string> Update(BusinessEntity.Settings.Reminder Reminder);
        Task<string> Delete(int id);
    }
}
