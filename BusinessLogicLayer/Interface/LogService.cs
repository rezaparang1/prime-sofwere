using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface
{
    public interface ILogService
    {
        Task CreateLogAsync(string description, int userId);
    }

}
