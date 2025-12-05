using DataAccessLayer.Interface.Fund;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime_Software.Controllers.Fund
{
    public class AutoShiftService : IAutoShiftService
    {
        private readonly DataAccessLayer.Database _context;

        public AutoShiftService(DataAccessLayer.Database context)
        {
            _context = context;
        }

        public async Task RegisterActivity(int userId)
        {
            var user = await _context.User.FindAsync(userId);
            if (user == null) return;

            user.LastActivity = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            await AutoStartShift(userId);
        }

        public async Task AutoStartShift(int userId)
        {
            var cashReg = await _context.Cash_Register_To_The_User
                .FirstOrDefaultAsync(x => x.UserId == userId && x.IsActive);

            if (cashReg == null) return;

            var openShift = await _context.Work_Shift
                .AnyAsync(x => x.CashRegisterToUserId == cashReg.Id && !x.IsClosed);

            if (openShift) return;

            var shift = new BusinessEntity.Fund.Work_Shift
            {
                CashRegisterToUserId = cashReg.Id,
                StartTime = DateTime.UtcNow,
                OpeningAmount = cashReg.InitialAmount,
                IsClosed = false,
                IsAuto = true
            };

            await _context.Work_Shift.AddAsync(shift);
            await _context.SaveChangesAsync();
        }

        public async Task ManualCloseShift(int userId)
        {
            var cashReg = await _context.Cash_Register_To_The_User
                .FirstOrDefaultAsync(x => x.UserId == userId && x.IsActive);

            if (cashReg == null) return;

            var shift = await _context.Work_Shift
                .FirstOrDefaultAsync(x => x.CashRegisterToUserId == cashReg.Id && !x.IsClosed);

            if (shift == null) return;

            shift.IsClosed = true;
            shift.IsAuto = false;
            shift.EndTime = DateTime.UtcNow;
            shift.ClosingAmount = shift.OpeningAmount;

            await _context.SaveChangesAsync();
        }

        public async Task AutoCloseIdleShifts()
        {
            var users = await _context.User
                .Where(x => DateTime.UtcNow - x.LastActivity > TimeSpan.FromMinutes(30))
                .ToListAsync();

            foreach (var u in users)
            {
                var cashReg = await _context.Cash_Register_To_The_User
                    .FirstOrDefaultAsync(x => x.UserId == u.Id && x.IsActive);

                if (cashReg == null) continue;

                var shift = await _context.Work_Shift
                    .FirstOrDefaultAsync(x => x.CashRegisterToUserId == cashReg.Id && !x.IsClosed);

                if (shift == null || !shift.IsAuto) continue;

                shift.IsClosed = true;
                shift.EndTime = DateTime.UtcNow;
                shift.ClosingAmount = shift.OpeningAmount;

                await _context.SaveChangesAsync();
            }
        }
    }
}
