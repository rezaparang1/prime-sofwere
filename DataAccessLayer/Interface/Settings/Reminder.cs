using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interface.Settings
{
    public interface IReminderRepository
    {
        Task<List<BusinessEntity.Settings.Reminder>> Search(int? UserId = null, DateTime? Date = null, string? Description = null);
        Task<List<BusinessEntity.Settings.Reminder>> SearchByUserId(int? userId = null);
        Task<IEnumerable<BusinessEntity.Settings.Reminder>> GetAll();
        Task<BusinessEntity.Settings.Reminder?> GetById(int id);
        Task<string> Create(BusinessEntity.Settings.Reminder Reminder);
        Task<string> Update(BusinessEntity.Settings.Reminder Reminder);
        Task<string> Delete(int id);
    }
}
