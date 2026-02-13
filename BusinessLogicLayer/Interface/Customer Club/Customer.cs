using BusinessLogicLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface.Customer_Club
{
    public interface ICustomerService
    {
        Task<Result<CustomerDto>> RegisterCustomerAsync(CustomerRegisterDto dto);
        Task<Result<CustomerDto>> GetCustomerByBarcodeAsync(string barcode);
        Task<Result<CustomerDto>> GetCustomerByIdAsync(int id);
        Task<Result<string>> GenerateCustomerBarcodeAsync();
        Task<Result> UpdateCustomerPointsAsync(int customerId, int points, string description, int? invoiceId = null);
        Task<Result<CustomerLevelDto?>> GetCustomerCurrentLevelAsync(int customerId);
        Task<Result> UpgradeCustomerLevelAsync(int customerId);
        Task<Result<CustomerDto>> GetCustomerByPeopleIdAsync(int peopleId);
    }
}
