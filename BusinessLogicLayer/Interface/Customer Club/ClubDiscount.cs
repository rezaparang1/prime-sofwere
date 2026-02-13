using BusinessLogicLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface.Customer_Club
{
    public interface IClubDiscountService
    {
        Task<Result<ClubDiscountDto>> CreateClubDiscountAsync(ClubDiscountCreateDto dto);
        Task<Result<ClubDiscountDto>> GetDiscountByIdAsync(int id);
        Task<Result<IEnumerable<ClubDiscountDto>>> GetActiveDiscountsAsync(int storeId);
        Task<Result<ClubDiscountCalculationResult>> CalculateClubDiscountAsync(string barcode, int customerId, int originalPrice);
        Task<Result<bool>> HasActiveDiscountForProductAsync(int productId, int storeId);
        Task<Result<bool>> HasActiveDiscountForUnitAsync(int unitLevelId, int storeId);
    }
}
