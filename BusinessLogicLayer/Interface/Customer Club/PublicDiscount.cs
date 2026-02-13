using BusinessLogicLayer.DTO;

namespace BusinessLogicLayer.Interface.Customer_Club
{
    public interface IPublicDiscountService
    {
        Task<Result<PublicDiscountDto>> CreatePublicDiscountAsync(PublicDiscountCreateDto dto);
        Task<Result<PublicDiscountDto>> GetDiscountByIdAsync(int id);
        Task<Result<IEnumerable<PublicDiscountDto>>> GetActiveDiscountsAsync(int storeId);
        Task<Result<PublicDiscountCalculationResult>> CalculatePublicDiscountAsync(string barcode, DateTime purchaseTime, int storeId);
        Task<Result> DeactivateDiscountAsync(int discountId);
    }
}
