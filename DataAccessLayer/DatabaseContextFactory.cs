using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.Configuration.Json;

namespace DataAccessLayer
{
    public class DatabaseContextFactory : IDesignTimeDbContextFactory<Database>
    {
        public Database CreateDbContext(string[] args)
        {
            // نام فایل تنظیمات
            const string configFileName = "appsettings.json";

            // شروع از دایرکتوری جاری و جستجو تا چند سطح بالاتر برای پیدا کردن appsettings.json
            string currentDir = Directory.GetCurrentDirectory();
            string? foundDir = null;
            var dirInfo = new DirectoryInfo(currentDir);
            for (int i = 0; i < 6 && dirInfo != null; i++)
            {
                var candidate = Path.Combine(dirInfo.FullName, configFileName);
                if (File.Exists(candidate))
                {
                    foundDir = dirInfo.FullName;
                    break;
                }
                dirInfo = dirInfo.Parent;
            }

            var builder = new ConfigurationBuilder();

            if (!string.IsNullOrEmpty(foundDir))
            {
                // اگر پیدا شد، از مسیر کامل فایل استفاده کن (نه SetBasePath)
                builder.AddJsonFile(Path.Combine(foundDir, configFileName), optional: false, reloadOnChange: true);
            }
            else
            {
                // اگر پیدا نشد، تلاش کن از دایرکتوری جاری استفاده کنی (ممکنه startup project فایل را داشته باشد)
                builder.AddJsonFile(configFileName, optional: false, reloadOnChange: true);
            }

            IConfiguration configuration = builder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<Database>();
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                                   ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            optionsBuilder.UseNpgsql(connectionString);

            return new Database(optionsBuilder.Options);
        }
    }
    }
