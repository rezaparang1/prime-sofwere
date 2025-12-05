using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface.Settings
{
    public interface IAccessLevelService
    {
        Task<IEnumerable<BusinessEntity.Settings.Access_Level>> GetAll();
        Task<BusinessEntity.Settings.Access_Level?> GetById(int id);
        Task<string> Create(BusinessEntity.Settings.Access_Level Access_Level);
        Task<string> Update(BusinessEntity.Settings.Access_Level Access_Level);
        Task<string> Delete(int id);
    }
}
