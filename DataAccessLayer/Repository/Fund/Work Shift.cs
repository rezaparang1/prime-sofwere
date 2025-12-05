using BusinessEntity.Settings;
using DataAccessLayer.Interface.Fund;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity.Fund;
namespace DataAccessLayer.Repository.Fund
{
    public class WorkShiftRepository : IWorkShiftRepository
    {
        private readonly Database _context;
        private readonly ILogger<WorkShiftRepository> _logger;

        public WorkShiftRepository(Database context, ILogger<WorkShiftRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Work_Shift>> Search(int? fundId = null, int? userId = null, DateTime? dateFirst = null, DateTime? dateEnd = null)
        {
            var query = _context.Work_Shift
                .Include(x => x.CashRegisterToUser)
                .ThenInclude(c => c.User)
                .AsQueryable();

            if (fundId.HasValue)
                query = query.Where(r => r.CashRegisterToUser.FundId == fundId);

            if (userId.HasValue)
                query = query.Where(r => r.CashRegisterToUser.UserId == userId);

            if (dateFirst.HasValue)
                query = query.Where(r => r.StartTime >= dateFirst);

            if (dateEnd.HasValue)
                query = query.Where(r => r.EndTime <= dateEnd);

            return await query.ToListAsync();
        }
        public async Task<List<WorkShiftDto>> GetAll()
        {
            return await _context.Work_Shift
                .Include(x => x.CashRegisterToUser)
                .ThenInclude(c => c.User)
                .Select(ws => new WorkShiftDto
                {
                    Id = ws.Id,
                    CashRegisterToUserId = ws.CashRegisterToUserId,
                    UserName = ws.CashRegisterToUser.User.UserName,
                    ClosingAmount = ws.ClosingAmount,
                    IsClosed = ws.IsClosed,
                    StartTime = ws.StartTime,
                    EndTime = ws.EndTime
                })
                .ToListAsync();
        }

        public async Task<WorkShiftDto?> GetById(int id)
        {
            var ws = await _context.Work_Shift
                .Include(x => x.CashRegisterToUser)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (ws == null)
                return null;

            return new WorkShiftDto
            {
                Id = ws.Id,
                CashRegisterToUserId = ws.CashRegisterToUserId,
                UserName = ws.CashRegisterToUser.User.UserName,
                ClosingAmount = ws.ClosingAmount,
                IsClosed = ws.IsClosed,
                StartTime = ws.StartTime,
                EndTime = ws.EndTime
            };
        }
        public async Task<List<ActiveShiftDto>> GetActiveShifts()
        {
            return await _context.Work_Shift
                .Include(ws => ws.CashRegisterToUser)
                    .ThenInclude(cr => cr.User)
                .Include(ws => ws.CashRegisterToUser)
                    .ThenInclude(cr => cr.Fund)
                .Where(ws => !ws.IsClosed || (ws.IsClosed && ws.EndTime == null))
                .Select(ws => new ActiveShiftDto
                {
                    ShiftId = ws.Id,
                    UserName = ws.CashRegisterToUser.User.UserName,
                    FundName = ws.CashRegisterToUser.Fund.Name,
                    StartTime = ws.StartTime,
                    EndTime = ws.EndTime
                })
                .ToListAsync();
        }

        public async Task<string> Create(int operatorUserId, Work_Shift model)
        {
            try
            {
                // لود صندوق مربوطه
                var cashReg = await _context.Cash_Register_To_The_User
                    .Include(x => x.User)
                    .FirstOrDefaultAsync(x => x.Id == model.CashRegisterToUserId);

                if (cashReg == null)
                    return "صندوق معتبر نیست.";

                // ✔ شیفت دستی همیشه IsAuto = false و IsClosed = false
                model.IsAuto = false;
                model.IsClosed = false;

                // تنظیم تاریخ‌ها به UTC
                model.StartTime = DateTime.UtcNow; // شروع شیفت همان لحظه
                if (model.EndTime != default)
                    model.EndTime = DateTime.SpecifyKind(model.EndTime, DateTimeKind.Utc);

                // ثبت لاگ
                var log = new LogUser
                {
                    UserId = operatorUserId,
                    Description = $"شروع شیفت دستی توسط کاربر {cashReg.User?.UserName}",
                    Date = DateTime.UtcNow
                };

                await _context.LogUser.AddAsync(log);
                await _context.Work_Shift.AddAsync(model);

                await _context.SaveChangesAsync();
                return "شیفت دستی با موفقیت ایجاد شد.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Create WorkShift");
                return "خطای غیرمنتظره رخ داد.";
            }
        }
        public async Task<string> Update(int operatorUserId, Work_Shift model)
        {
            try
            {
                var existing = await _context.Work_Shift
                    .Include(x => x.CashRegisterToUser)
                    .ThenInclude(c => c.User)
                    .FirstOrDefaultAsync(x => x.Id == model.Id);

                if (existing == null)
                    return "شیفت یافت نشد.";

                // ثبت لاگ
                var log = new LogUser
                {
                    UserId = operatorUserId,
                    Description = $"ویرایش شیفت کاربر {existing.CashRegisterToUser.User?.UserName}",
                    Date = DateTime.UtcNow
                };
                await _context.LogUser.AddAsync(log);

                // فقط فیلدهای مجاز را آپدیت کن
                if (model.EndTime != default)
                {
                    // تبدیل به UTC
                    existing.EndTime = DateTime.SpecifyKind(model.EndTime, DateTimeKind.Local).ToUniversalTime();
                }

                existing.ClosingAmount = model.ClosingAmount;
                existing.IsClosed = model.IsClosed;

                await _context.SaveChangesAsync();
                return "شیفت با موفقیت ویرایش شد.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating WorkShift");
                return "خطای غیرمنتظره رخ داد.";
            }
        }

    }

}
