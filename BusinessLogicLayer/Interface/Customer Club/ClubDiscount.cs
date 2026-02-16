using BusinessLogicLayer.DTO;

namespace BusinessLogicLayer.Interface.Customer_Club
{
    public interface IClubDiscountService
    {
        Task<Result<IEnumerable<ClubDiscountDto>>> SearchDiscountsAsync(ClubDiscountSearchDto searchDto);
        Task<Result<ClubDiscountDto>> CreateClubDiscountAsync(ClubDiscountCreateDto dto);
        Task<Result<ClubDiscountDto>> UpdateClubDiscountAsync(ClubDiscountUpdateDto dto);
        Task<Result<ClubDiscountDto>> GetDiscountByIdAsync(int id);
        Task<Result<IEnumerable<ClubDiscountDto>>> GetActiveDiscountsAsync(int storeId);
        Task<Result<ClubDiscountCalculationResult>> CalculateClubDiscountAsync(string barcode, int customerId, int originalPrice);
        Task<Result<bool>> HasActiveDiscountForProductAsync(int productId, int storeId);
        Task<Result<bool>> HasActiveDiscountForUnitAsync(int unitLevelId, int storeId);
    }
}
