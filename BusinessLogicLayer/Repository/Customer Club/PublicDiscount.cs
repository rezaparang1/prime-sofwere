using BusinessEntity.Customer_Club;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Interface.Customer_Club;
using DataAccessLayer.Interface.Customer_Club;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repository.Customer_Club
{
    public class PublicDiscountService : IPublicDiscountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PublicDiscountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PublicDiscountDto>> CreatePublicDiscountAsync(PublicDiscountCreateDto dto)
        {
            if (dto.StartDate >= dto.EndDate)
                return Result<PublicDiscountDto>.Failure("تاریخ شروع باید قبل از تاریخ پایان باشد");
            if (dto.StartTime >= dto.EndTime)
                return Result<PublicDiscountDto>.Failure("ساعت شروع باید قبل از ساعت پایان باشد");

            var discount = new PublicDiscount
            {
                Title = dto.Title,
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                IsActive = true,
                Type = dto.Type,
                Value = dto.Value,
                StoreId = dto.StoreId,
                Saturday = dto.Saturday,
                Sunday = dto.Sunday,
                Monday = dto.Monday,
                Tuesday = dto.Tuesday,
                Wednesday = dto.Wednesday,
                Thursday = dto.Thursday,
                Friday = dto.Friday
            };

            await _unitOfWork.PublicDiscounts.AddAsync(discount);
            await _unitOfWork.SaveChangesAsync();

            foreach (var prodDto in dto.Products)
            {
                if (!prodDto.ProductId.HasValue && !prodDto.UnitLevelId.HasValue)
                    continue;

                int originalPrice = 0;
                if (prodDto.ProductId.HasValue)
                {
                    var product = await _unitOfWork.Products.GetByIdAsync(prodDto.ProductId.Value);
                    originalPrice = product != null ? (int)product.SalePrice : 0;
                }
                else if (prodDto.UnitLevelId.HasValue)
                {
                    var unit = await _unitOfWork.UnitsLevels.GetByIdAsync(prodDto.UnitLevelId.Value);
                    if (unit != null)
                    {
                        var defaultPrice = unit.Prices.FirstOrDefault(p => p.PriceLevelId == 1);
                        originalPrice = defaultPrice != null ? (int)defaultPrice.SalePrice : (int)(unit.Product?.SalePrice ?? 0);
                    }
                }

                var discountProduct = new PublicDiscountProduct
                {
                    PublicDiscountId = discount.Id,
                    ProductId = prodDto.ProductId,
                    UnitLevelId = prodDto.UnitLevelId,
                    DiscountedPrice = prodDto.DiscountedPrice,
                    OriginalPrice = originalPrice
                };
                await _unitOfWork.PublicDiscountProducts.AddAsync(discountProduct);
            }

            await _unitOfWork.SaveChangesAsync();
            var dtoResult = await MapToDto(discount);
            return Result<PublicDiscountDto>.SuccessResult(dtoResult, "تخفیف عمومی با موفقیت ایجاد شد");
        }

        public async Task<Result<PublicDiscountCalculationResult>> CalculatePublicDiscountAsync(string barcode, DateTime purchaseTime, int storeId)
        {
            var result = new PublicDiscountCalculationResult { DiscountAmount = 0 };

            var barcodeEntity = await _unitOfWork.ProductBarcodes.GetByBarcodeAsync(barcode);
            if (barcodeEntity == null)
                return Result<PublicDiscountCalculationResult>.SuccessResult(result);

            var dayOfWeek = purchaseTime.DayOfWeek;
            var timeOfDay = purchaseTime.TimeOfDay;

            var activeDiscounts = await _unitOfWork.PublicDiscounts
                .GetActivePublicDiscountsWithProductsAsync(storeId);

            foreach (var discount in activeDiscounts)
            {
                if (!IsDayActive(discount, dayOfWeek))
                    continue;
                if (discount.StartTime > timeOfDay || discount.EndTime < timeOfDay)
                    continue;

                var discountProduct = discount.Products?.FirstOrDefault(p =>
                    (p.UnitLevelId == barcodeEntity.UnitLevelId) ||
                    (p.ProductId == barcodeEntity.ProductId && p.UnitLevelId == null));

                if (discountProduct != null)
                {
                    if (discount.Type == DiscountType.Percentage)
                    {
                        result.DiscountAmount = (int)(discountProduct.OriginalPrice * discount.Value / 100);
                    }
                    else // FixedAmount
                    {
                        result.DiscountAmount = discountProduct.OriginalPrice - discountProduct.DiscountedPrice;
                    }
                    result.DiscountId = discount.Id;
                    result.DiscountedPrice = discountProduct.DiscountedPrice;
                    break;
                }
            }

            return Result<PublicDiscountCalculationResult>.SuccessResult(result);
        }

        public async Task<Result<PublicDiscountDto>> GetDiscountByIdAsync(int id)
        {
            var discount = await _unitOfWork.PublicDiscounts.GetPublicDiscountWithProductsAsync(id);
            if (discount == null)
                return Result<PublicDiscountDto>.Failure("تخفیف یافت نشد");

            var dto = await MapToDto(discount);
            return Result<PublicDiscountDto>.SuccessResult(dto);
        }

        public async Task<Result<IEnumerable<PublicDiscountDto>>> GetActiveDiscountsAsync(int storeId)
        {
            var discounts = await _unitOfWork.PublicDiscounts.GetActivePublicDiscountsWithProductsAsync(storeId);
            var dtos = new List<PublicDiscountDto>();
            foreach (var d in discounts)
                dtos.Add(await MapToDto(d));
            return Result<IEnumerable<PublicDiscountDto>>.SuccessResult(dtos);
        }

        public async Task<Result> DeactivateDiscountAsync(int discountId)
        {
            var discount = await _unitOfWork.PublicDiscounts.GetByIdAsync(discountId);
            if (discount == null)
                return Result.Failure("تخفیف یافت نشد");

            discount.IsActive = false;
            _unitOfWork.PublicDiscounts.Update(discount);
            await _unitOfWork.SaveChangesAsync();
            return Result.SuccessResult("تخفیف غیرفعال شد");
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

        private async Task<PublicDiscountDto> MapToDto(PublicDiscount discount)
        {
            var dto = new PublicDiscountDto
            {
                Id = discount.Id,
                Title = discount.Title,
                Description = discount.Description,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate,
                StartTime = discount.StartTime,
                EndTime = discount.EndTime,
                IsActive = discount.IsActive,
                Type = discount.Type.ToString(),
                Value = discount.Value,
                StoreId = discount.StoreId,
                DaysOfWeek = GetActiveDays(discount),
                Products = new List<PublicDiscountProductDto>()
            };

            if (discount.Products != null)
            {
                foreach (var p in discount.Products)
                {
                    var prodDto = new PublicDiscountProductDto
                    {
                        ProductId = p.ProductId,
                        UnitLevelId = p.UnitLevelId,
                        DiscountedPrice = p.DiscountedPrice,
                        OriginalPrice = p.OriginalPrice
                    };

                    if (p.ProductId != null)
                    {
                        var product = await _unitOfWork.Products.GetByIdAsync(p.ProductId.Value);
                        prodDto.ProductName = product?.Name;
                    }
                    if (p.UnitLevelId != null)
                    {
                        var unit = await _unitOfWork.UnitsLevels.GetByIdAsync(p.UnitLevelId.Value);
                        prodDto.UnitName = unit?.Title;
                    }

                    dto.Products.Add(prodDto);
                }
            }

            return dto;
        }

        private List<string> GetActiveDays(PublicDiscount discount)
        {
            var days = new List<string>();
            if (discount.Saturday) days.Add("Saturday");
            if (discount.Sunday) days.Add("Sunday");
            if (discount.Monday) days.Add("Monday");
            if (discount.Tuesday) days.Add("Tuesday");
            if (discount.Wednesday) days.Add("Wednesday");
            if (discount.Thursday) days.Add("Thursday");
            if (discount.Friday) days.Add("Friday");
            return days;
        }
    }
}
