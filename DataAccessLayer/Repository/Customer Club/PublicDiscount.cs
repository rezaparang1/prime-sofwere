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
    public class PublicDiscountRepository : Repository<PublicDiscount>, IPublicDiscountRepository
    {
        public PublicDiscountRepository(Database context) : base(context)
        {
        }

        public async Task<IEnumerable<PublicDiscount>> GetActivePublicDiscountsAsync(int storeId)
        {
            var now = DateTime.Now;
            return await _dbSet
                .Where(pd => pd.StoreId == storeId &&
                            pd.IsActive &&
                            pd.StartDate <= now &&
                            pd.EndDate >= now)
                .OrderByDescending(pd => pd.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<PublicDiscount>> GetActivePublicDiscountsWithProductsAsync(int storeId)
        {
            var now = DateTime.Now;
            return await _dbSet
                .Include(pd => pd.Products)
                    .ThenInclude(p => p.Product)
                .Where(pd => pd.StoreId == storeId &&
                            pd.IsActive &&
                            pd.StartDate <= now &&
                            pd.EndDate >= now)
                .OrderByDescending(pd => pd.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<PublicDiscount>> GetPublicDiscountsByProductAsync(int productId, DateTime date)
        {
            var timeOfDay = date.TimeOfDay;
            var dayOfWeek = date.DayOfWeek;

            return await _dbSet
                .Include(pd => pd.Products)
                .Where(pd => pd.Products.Any(p => p.ProductId == productId) &&
                            pd.IsActive &&
                            pd.StartDate <= date &&
                            pd.EndDate >= date &&
                            pd.StartTime <= timeOfDay &&
                            pd.EndTime >= timeOfDay &&
                            IsDayActive(pd, dayOfWeek))
                .ToListAsync();
        }

        public async Task<PublicDiscount?> GetPublicDiscountWithProductsAsync(int id)
        {
            return await _dbSet
                .Include(pd => pd.Products)
                    .ThenInclude(p => p.Product)
                .Include(pd => pd.Store)
                .FirstOrDefaultAsync(pd => pd.Id == id);
        }

        public async Task<bool> HasActiveDiscountForProductAsync(int productId, DateTime date, int storeId)
        {
            var timeOfDay = date.TimeOfDay;
            var dayOfWeek = date.DayOfWeek;

            return await _dbSet
                .AnyAsync(pd => pd.StoreId == storeId &&
                               pd.IsActive &&
                               pd.StartDate <= date &&
                               pd.EndDate >= date &&
                               pd.StartTime <= timeOfDay &&
                               pd.EndTime >= timeOfDay &&
                               IsDayActive(pd, dayOfWeek) &&
                               pd.Products.Any(p => p.ProductId == productId));
        }

        public async Task<IEnumerable<PublicDiscount>> GetPublicDiscountsByDateRangeAsync(int storeId, DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(pd => pd.StoreId == storeId &&
                            pd.IsActive &&
                            ((pd.StartDate >= startDate && pd.StartDate <= endDate) ||
                             (pd.EndDate >= startDate && pd.EndDate <= endDate) ||
                             (pd.StartDate <= startDate && pd.EndDate >= endDate)))
                .OrderBy(pd => pd.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<PublicDiscount>> GetDiscountsByDayOfWeekAsync(int storeId, DayOfWeek dayOfWeek)
        {
            var now = DateTime.Now;
            return await _dbSet
                .Where(pd => pd.StoreId == storeId &&
                            pd.IsActive &&
                            pd.StartDate <= now &&
                            pd.EndDate >= now &&
                            IsDayActive(pd, dayOfWeek))
                .ToListAsync();
        }

        private bool IsDayActive(PublicDiscount discount, DayOfWeek dayOfWeek)
        {
            return dayOfWeek switch
            {
                DayOfWeek.Saturday => discount.Saturday,
                DayOfWeek.Sunday => discount.Sunday,
                DayOfWeek.Monday => discount.Monday,
                DayOfWeek.Tuesday => discount.Tuesday,
                DayOfWeek.Wednesday => discount.Wednesday,
                DayOfWeek.Thursday => discount.Thursday,
                DayOfWeek.Friday => discount.Friday,
                _ => false
            };
        }
    }
}
