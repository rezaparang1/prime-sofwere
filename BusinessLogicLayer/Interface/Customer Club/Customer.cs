using BusinessLogicLayer.DTO;


namespace BusinessLogicLayer.Interface.Customer_Club
{
    public interface ICustomerService
    {
        Task<Result<List<CustomerDto>>> SearchCustomersAsync(
     string? firstName = null,
     string? lastName = null,
     int? customerLevelId = null,
     int? minPoints = null,
     int? maxPoints = null,
     string? barcode = null);
        Task<Result<List<CustomerDto>>> GetAllCustomersAsync();
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
