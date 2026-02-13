using BusinessEntity.Settings;
using BusinessLogicLayer.Interface;
using DataAccessLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repository
{
    public class LogService : ILogService
    {
        private readonly IRepository<LogUser> _logRepo;
        private readonly DataAccessLayer.Database _context;

        public LogService(IRepository<LogUser> logRepo, DataAccessLayer.Database context)
        {
            _logRepo = logRepo;
            _context = context;
        }

        public async Task CreateLogAsync(string description, int userId)
        {
            var log = new LogUser
            {
                Description = description,
                Date = DateTime.Now,
                UserId = userId
            };

            await _logRepo.AddAsync(log);          // ✅ استفاده از AddAsync به جای Add
            await _context.SaveChangesAsync();     // ✅ ذخیره‌سازی تغییرات در دیتابیس
        }
    }
}
