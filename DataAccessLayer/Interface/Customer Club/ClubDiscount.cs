using BusinessEntity.Customer_Club;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interface.Customer_Club
{
    public interface IClubDiscountRepository : IRepository<ClubDiscount>
    {
        // متدهای موجود (قدیمی) - دست‌نخورده
        Task<IEnumerable<ClubDiscount>> GetActiveDiscountsAsync(int storeId);
        Task<IEnumerable<ClubDiscount>> GetActiveDiscountsWithProductsAsync(int storeId);
        Task<IEnumerable<ClubDiscount>> GetDiscountsByProductAsync(int productId, DateTime date);
        Task<ClubDiscount?> GetDiscountWithProductsAsync(int id);
        Task<ClubDiscountProduct?> GetClubDiscountProductAsync(int discountId, int productId);
        Task<bool> HasActiveDiscountForProductAsync(int productId, DateTime date, int storeId);
        Task<IEnumerable<ClubDiscount>> GetDiscountsByDateRangeAsync(int storeId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<ClubDiscount>> GetExpiringDiscountsAsync(int daysBeforeExpiration);

        // ✅ متد جدید: دریافت تخفیف‌های فعال برای یک واحد خاص (UnitLevelId)
        Task<IEnumerable<ClubDiscount>> GetActiveDiscountsByUnitAsync(int unitLevelId, DateTime date);

        // ✅ متد جدید: دریافت رکورد تخفیف-محصول بر اساس شناسه واحد
        Task<ClubDiscountProduct?> GetClubDiscountProductByUnitAsync(int discountId, int unitLevelId);

        // ✅ متد جدید: بررسی وجود تخفیف فعال برای یک واحد خاص
        Task<bool> HasActiveDiscountForUnitAsync(int unitLevelId, DateTime date, int storeId);
    }
}
