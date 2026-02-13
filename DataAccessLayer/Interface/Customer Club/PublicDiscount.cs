using BusinessEntity.Customer_Club;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interface.Customer_Club
{
    public interface IPublicDiscountRepository : IRepository<PublicDiscount>
    {
        Task<IEnumerable<PublicDiscount>> GetActivePublicDiscountsAsync(int storeId);
        Task<IEnumerable<PublicDiscount>> GetActivePublicDiscountsWithProductsAsync(int storeId);
        Task<IEnumerable<PublicDiscount>> GetPublicDiscountsByProductAsync(int productId, DateTime date);
        Task<PublicDiscount?> GetPublicDiscountWithProductsAsync(int id);
        Task<bool> HasActiveDiscountForProductAsync(int productId, DateTime date, int storeId);
        Task<IEnumerable<PublicDiscount>> GetPublicDiscountsByDateRangeAsync(int storeId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<PublicDiscount>> GetDiscountsByDayOfWeekAsync(int storeId, DayOfWeek dayOfWeek);
    }
}
