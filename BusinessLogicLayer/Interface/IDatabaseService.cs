using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface
{
    public interface IDatabaseService
    {
        Task<string> BackupDatabaseAsync(string backupPath);
        Task<string> RestoreDatabaseAsync(string backupFile);
    }
}
