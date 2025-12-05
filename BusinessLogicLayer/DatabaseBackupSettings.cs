using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class DatabaseBackupSettings
    {
        public string PgDumpPath { get; set; } = string.Empty;
        public string PgRestorePath { get; set; } = string.Empty;
        public string DbName { get; set; } = string.Empty;
        public string DbUser { get; set; } = string.Empty;
        public string DbPassword { get; set; } = string.Empty;
    }

}
