using BusinessEntity.Customer_Club;
using DataAccessLayer.Interface.Customer_Club;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Customer_Club
{
    public class ClubDiscountRepository : Repository<ClubDiscount>, IClubDiscountRepository
    {
        public ClubDiscountRepository(Database context) : base(context)
        {
        }

        // ========== متدهای قدیمی (با اصلاح نام DbSet) ==========

        public async Task<IEnumerable<ClubDiscount>> GetActiveDiscountsAsync(int storeId)
        {
            var now = DateTime.Now;
            return await _dbSet
                .Where(cd => cd.StoreId == storeId &&
                             cd.IsActive &&
                             cd.StartDate <= now &&
                             cd.EndDate >= now)
                .OrderByDescending(cd => cd.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ClubDiscount>> GetActiveDiscountsWithProductsAsync(int storeId)
        {
            var now = DateTime.Now;
            return await _dbSet
                .Include(cd => cd.Products)
                    .ThenInclude(p => p.Product)
                .Where(cd => cd.StoreId == storeId &&
                             cd.IsActive &&
                             cd.StartDate <= now &&
                             cd.EndDate >= now)
                .OrderByDescending(cd => cd.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ClubDiscount>> GetDiscountsByProductAsync(int productId, DateTime date)
        {
            return await _dbSet
                .Include(cd => cd.Products)
                .Where(cd => cd.Products.Any(p => p.ProductId == productId) &&
                             cd.IsActive &&
                             cd.StartDate <= date &&
                             cd.EndDate >= date)
                .ToListAsync();
        }

        public async Task<ClubDiscount?> GetDiscountWithProductsAsync(int id)
        {
            return await _dbSet
                .Include(cd => cd.Products)
                    .ThenInclude(p => p.Product)
                .Include(cd => cd.Store)
                .FirstOrDefaultAsync(cd => cd.Id == id);
        }

        public async Task<ClubDiscountProduct?> GetClubDiscountProductAsync(int discountId, int productId)
        {
            return await _context.ClubDiscountProduct  // ✅ اصلاح نام DbSet (جمع)
                .Include(cdp => cdp.ClubDiscount)
                .FirstOrDefaultAsync(cdp => cdp.ClubDiscountId == discountId &&
                                            cdp.ProductId == productId);
        }

        public async Task<bool> HasActiveDiscountForProductAsync(int productId, DateTime date, int storeId)
        {
            return await _dbSet
                .AnyAsync(cd => cd.StoreId == storeId &&
                                cd.IsActive &&
                                cd.StartDate <= date &&
                                cd.EndDate >= date &&
                                cd.Products.Any(p => p.ProductId == productId));
        }

        public async Task<IEnumerable<ClubDiscount>> GetDiscountsByDateRangeAsync(int storeId, DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(cd => cd.StoreId == storeId &&
                             cd.IsActive &&
                             ((cd.StartDate >= startDate && cd.StartDate <= endDate) ||
                              (cd.EndDate >= startDate && cd.EndDate <= endDate) ||
                              (cd.StartDate <= startDate && cd.EndDate >= endDate)))
                .OrderBy(cd => cd.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ClubDiscount>> GetExpiringDiscountsAsync(int daysBeforeExpiration)
        {
            var targetDate = DateTime.Now.AddDays(daysBeforeExpiration);
            return await _dbSet
                .Where(cd => cd.IsActive &&
                             cd.EndDate <= targetDate &&
                             cd.EndDate >= DateTime.Now)
                .ToListAsync();
        }

        // ========== متدهای جدید برای پشتیبانی از واحد کالا ==========

        /// <summary>
        /// دریافت تخفیف‌های فعال برای یک واحد خاص (UnitLevelId)
        /// </summary>
        public async Task<IEnumerable<ClubDiscount>> GetActiveDiscountsByUnitAsync(int unitLevelId, DateTime date)
        {
            return await _dbSet
                .Include(cd => cd.Products)
                .Where(cd => cd.IsActive &&
                             cd.StartDate <= date &&
                             cd.EndDate >= date &&
                             cd.Products.Any(p => p.UnitLevelId == unitLevelId))
                .ToListAsync();
        }

        /// <summary>
        /// دریافت رکورد تخفیف-محصول بر اساس شناسه واحد
        /// </summary>
        public async Task<ClubDiscountProduct?> GetClubDiscountProductByUnitAsync(int discountId, int unitLevelId)
        {
            return await _context.ClubDiscountProduct
                .Include(cdp => cdp.ClubDiscount)
                .FirstOrDefaultAsync(cdp => cdp.ClubDiscountId == discountId &&
                                            cdp.UnitLevelId == unitLevelId);
        }

        /// <summary>
        /// بررسی وجود تخفیف فعال برای یک واحد خاص
        /// </summary>
        public async Task<bool> HasActiveDiscountForUnitAsync(int unitLevelId, DateTime date, int storeId)
        {
            return await _dbSet
                .AnyAsync(cd => cd.StoreId == storeId &&
                                cd.IsActive &&
                                cd.StartDate <= date &&
                                cd.EndDate >= date &&
                                cd.Products.Any(p => p.UnitLevelId == unitLevelId));
        }
    }
}
