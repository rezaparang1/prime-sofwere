using BusinessEntity.Fund;
using BusinessEntity.Settings;
using DataAccessLayer.Interface.Fund;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity.DTO.Fund;
namespace DataAccessLayer.Repository.Fund
{
    public class CashRegisterToUserRepository : Interface.Fund.ICashRegisterToTheUserRepository
    {
        private readonly Database _context;
        private readonly ILogger<CashRegisterToUserRepository> _logger;

        public CashRegisterToUserRepository(Database context, ILogger<CashRegisterToUserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<List<CashRegisterComboItem>> GetActiveCashRegistersForCombo()
        {
            var activeCashRegisters = await _context.Cash_Register_To_The_User
                .Include(c => c.User)
                .Include(c => c.Fund)
                .Where(c => c.IsActive)
                .ToListAsync();

            return activeCashRegisters.Select(c => new CashRegisterComboItem
            {
                Id = c.Id,
                DisplayName = $"{c.User.UserName} ({c.Fund?.Name ?? "نامشخص"})"
            }).ToList();
        }

        public async Task<List<CashRegisterDto>> GetAll()
        {
            return await _context.Cash_Register_To_The_User
                .Include(x => x.User)
                .Include(x => x.Fund)
                .Include(x => x.WorkShifts)
                .Select(x => new CashRegisterDto
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    UserName = x.User!.UserName,
                    FundId = x.FundId,
                    FundName = x.Fund!.Name,
                    InitialAmount = x.InitialAmount,
                    IsActive = x.IsActive,
                    Date = x.Date
                }).ToListAsync();
        }
        public async Task<Cash_Register_To_The_User?> GetActiveByUser(int userId)
        {
            return await _context.Cash_Register_To_The_User
                .FirstOrDefaultAsync(x => x.UserId == userId && x.IsActive);
        }
        public async Task<CashRegisterDto?> GetById(int id)
        {
            var entity = await _context.Cash_Register_To_The_User
                .Include(x => x.User)
                .Include(x => x.Fund)
                .Include(x => x.WorkShifts)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null) return null;

            return new CashRegisterDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                UserName = entity.User!.UserName,
                FundId = entity.FundId,
                FundName = entity.Fund!.Name,
                InitialAmount = entity.InitialAmount,
                IsActive = entity.IsActive,
                Date = entity.Date
            };
        }
        public async Task<string> Create(int operatorUserId, Cash_Register_To_The_User model)
        {
            try
            {
                // بررسی اینکه صندوق هم‌اکنون فعال نباشد
                var activeAssignment = await _context.Cash_Register_To_The_User
                    .AnyAsync(c => c.FundId == model.FundId && c.IsActive);
                if (activeAssignment)
                    return "این صندوق در حال حاضر به کاربر دیگری اختصاص دارد.";

                // تنظیم تاریخ به UTC و فعال کردن تحویل
                model.Date = DateTime.SpecifyKind(model.Date, DateTimeKind.Local).ToUniversalTime();
                model.IsActive = true;

                // ثبت لاگ
                var log = new LogUser
                {
                    UserId = operatorUserId,
                    Description = $"تحویل صندوق {model.FundId} به کاربر {model.UserId}",
                    Date = DateTime.UtcNow
                };
                await _context.LogUser.AddAsync(log);

                // ثبت تحویل صندوق
                await _context.Cash_Register_To_The_User.AddAsync(model);
                await _context.SaveChangesAsync();

                return "عملیات با موفقیت انجام شد.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create CashRegister Error");
                return "خطای غیرمنتظره رخ داد.";
            }
        }

        public async Task<string> Update(int operatorUserId, Cash_Register_To_The_User model)
        {
            try
            {
                var existing = await _context.Cash_Register_To_The_User.FindAsync(model.Id);
                if (existing == null)
                    return "رکورد مورد نظر یافت نشد.";

                // بررسی اینکه صندوق برای کاربر دیگری فعال نباشد
                if (model.IsActive)
                {
                    var activeAssignment = await _context.Cash_Register_To_The_User
                        .AnyAsync(c => c.FundId == model.FundId && c.IsActive && c.Id != model.Id);
                    if (activeAssignment)
                        return "این صندوق در حال حاضر به کاربر دیگری اختصاص دارد.";
                }

                // ثبت لاگ
                var log = new LogUser
                {
                    UserId = operatorUserId,
                    Description = model.IsActive
                        ? $"ویرایش صندوق {model.FundId} مربوط به کاربر {model.UserId}"
                        : $"ابطال تحویل صندوق {model.FundId} مربوط به کاربر {model.UserId}",
                    Date = DateTime.UtcNow
                };
                await _context.LogUser.AddAsync(log);

                // بروزرسانی رکورد
                existing.UserId = model.UserId;
                existing.FundId = model.FundId;
                existing.InitialAmount = model.InitialAmount;
                existing.IsActive = model.IsActive;
                existing.Date = DateTime.SpecifyKind(model.Date, DateTimeKind.Local).ToUniversalTime();

                // بستن شیفت‌های باز اگر تحویل ابطال شد
                if (!existing.IsActive)
                {
                    var openShifts = await _context.Work_Shift
                        .Where(w => w.CashRegisterToUserId == existing.Id && !w.IsClosed)
                        .ToListAsync();

                    foreach (var shift in openShifts)
                    {
                        shift.IsClosed = true;
                        shift.EndTime = DateTime.UtcNow;
                    }
                }

                await _context.SaveChangesAsync();
                return "عملیات با موفقیت انجام شد.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update CashRegister Error");
                return "خطای غیرمنتظره رخ داد.";
            }
        }

    }

}
