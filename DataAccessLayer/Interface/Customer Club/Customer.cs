using BusinessEntity.Customer_Club;
using BusinessEntity.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interface.Customer_Club
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer?> GetByBarcodeAsync(string barcode);
        Task<Customer?> GetByMobileAsync(string mobile);
        Task<Customer?> GetByEmailAsync(string email);
        Task<Customer?> GetWithDetailsAsync(int id);
        Task<IEnumerable<Customer>> GetCustomersByLevelAsync(int levelId);
        Task<IEnumerable<Customer>> GetCustomersByStoreAsync(int storeId);
        Task<IEnumerable<Customer>> GetActiveCustomersAsync();
        Task<IEnumerable<Customer>> GetClubMembersAsync();
        Task<decimal> GetCustomerTotalPurchaseAsync(int customerId);
        Task<int> GetCustomerPurchaseCountAsync(int customerId);
        Task<int> GetCustomerPointsAsync(int customerId);
        Task<bool> IsMobileExistsAsync(string mobile, int? excludeCustomerId = null);
        Task<bool> IsEmailExistsAsync(string email, int? excludeCustomerId = null);
        Task<bool> IsBarcodeExistsAsync(string barcode, int? excludeCustomerId = null);
    }
}
