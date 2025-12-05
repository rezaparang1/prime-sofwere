namespace Prime_Software.Controllers.Fund
{
    public interface IAutoShiftService
    {
        Task RegisterActivity(int userId);         // ثبت فعالیت کاربر + Auto Start Shift
        Task AutoStartShift(int userId);           // شروع خودکار شیفت
        Task ManualCloseShift(int userId);         // بستن دستی شیفت
        Task AutoCloseIdleShifts();                // بستن شیفتهای بلااستفاده (۳۰ دقیقه Idle)
    }
}
