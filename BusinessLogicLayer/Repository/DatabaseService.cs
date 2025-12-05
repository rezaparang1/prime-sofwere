using BusinessLogicLayer.Interface;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repository
{
    public class DatabaseService : IDatabaseService
    {
        private readonly DatabaseBackupSettings _settings;

        public DatabaseService(IOptions<DatabaseBackupSettings> settings)
        {
            _settings = settings.Value;
        }

        private async Task<string> ExecuteProcessAsync(string fileName, string arguments)
        {
            var process = new Process();
            process.StartInfo.FileName = fileName;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.EnvironmentVariables["PGPASSWORD"] = _settings.DbPassword;

            process.Start();

            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();

            process.WaitForExit();

            return process.ExitCode == 0 ? "عملیات با موفقیت انجام شد." : $"خطا: {error}";
        }

        public async Task<string> BackupDatabaseAsync(string backupPath)
        {
            string args =
                $"-U {_settings.DbUser} -F c -b -v -f \"{backupPath}\" {_settings.DbName}";
            return await ExecuteProcessAsync(_settings.PgDumpPath, args);
        }

        public async Task<string> RestoreDatabaseAsync(string backupFile)
        {
            string args =
                $"-U {_settings.DbUser} -d {_settings.DbName} -c \"{backupFile}\"";
            return await ExecuteProcessAsync(_settings.PgRestorePath, args);
        }
    }
}
