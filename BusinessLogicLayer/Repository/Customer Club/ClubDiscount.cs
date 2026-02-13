using BusinessEntity.Customer_Club;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Interface.Customer_Club;
using DataAccessLayer.Interface;


namespace BusinessLogicLayer.Repository.Customer_Club
{
    public class ClubDiscountService : IClubDiscountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClubDiscountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ClubDiscountDto>> CreateClubDiscountAsync(ClubDiscountCreateDto dto)
        {
            if (dto.StartDate >= dto.EndDate)
                return Result<ClubDiscountDto>.Failure("تاریخ شروع باید قبل از تاریخ پایان باشد");

            var discount = new ClubDiscount
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
                RefundToWallet = dto.RefundToWallet,
                StoreId = dto.StoreId
            };

            await _unitOfWork.ClubDiscounts.AddAsync(discount);
            await _unitOfWork.SaveChangesAsync();

            foreach (var prodDto in dto.Products)
            {
                // اعتبارسنجی: حتماً یکی از دو فیلد مقدار داشته باشد
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

                var discountProduct = new ClubDiscountProduct
                {
                    ClubDiscountId = discount.Id,
                    ProductId = prodDto.ProductId,
                    UnitLevelId = prodDto.UnitLevelId,
                    ClubPrice = prodDto.ClubPrice,
                    OriginalPrice = originalPrice
                };
                await _unitOfWork.ClubDiscountProducts.AddAsync(discountProduct);
            }

            await _unitOfWork.SaveChangesAsync();
            var resultDto = await MapToDto(discount);
            // ✅ اصلاح: SuccessResult به Success تغییر یافت
            return Result<ClubDiscountDto>.Success(resultDto, "تخفیف باشگاه با موفقیت ایجاد شد");
        }

        public async Task<Result<ClubDiscountCalculationResult>> CalculateClubDiscountAsync(string barcode, int customerId, int originalPrice)
        {
            var result = new ClubDiscountCalculationResult { DiscountAmount = 0 };

            // 1. بررسی عضویت مشتری در باشگاه
            var customer = await _unitOfWork.Customers.GetByIdAsync(customerId);
            if (customer == null || !customer.IsClubMember)
                return Result<ClubDiscountCalculationResult>.Success(result);

            // 2. یافتن واحد کالا با بارکد
            var barcodeEntity = await _unitOfWork.ProductBarcodes.GetByBarcodeAsync(barcode);
            if (barcodeEntity == null)
                return Result<ClubDiscountCalculationResult>.Success(result);

            var now = DateTime.Now;

            // 3. جستجوی تخفیف‌های مخصوص این واحد
            var discounts = new List<ClubDiscount>();
            var unitDiscounts = await _unitOfWork.ClubDiscounts
                .GetActiveDiscountsByUnitAsync(barcodeEntity.ProductUnitId, now);
            discounts.AddRange(unitDiscounts);

            // 4. جستجوی تخفیف‌های مخصوص کل محصول – اصلاح نام متد
            var productDiscounts = await _unitOfWork.ClubDiscounts
                .GetDiscountsByProductAsync(barcodeEntity.ProductUnit.ProductId, now); // دسترسی به ProductId از طریق ProductUnit
            discounts.AddRange(productDiscounts);

            if (!discounts.Any())
                return Result<ClubDiscountCalculationResult>.Success(result);

            // 5. انتخاب اولین تخفیف
            var discount = discounts.First();
            var discountProduct = discount.Products.FirstOrDefault(p =>
                (p.UnitLevelId == barcodeEntity.ProductUnitId) ||
                (p.ProductId == barcodeEntity.ProductUnit.ProductId && p.UnitLevelId == null));

            if (discountProduct == null)
                return Result<ClubDiscountCalculationResult>.Success(result);

            int discountAmount = 0;
            if (discount.Type == DiscountType.Percentage)
            {
                discountAmount = (int)(originalPrice * discount.Value / 100);
            }
            else // FixedAmount
            {
                discountAmount = discountProduct.OriginalPrice - discountProduct.ClubPrice;
            }

            result.DiscountAmount = discountAmount;
            result.DiscountId = discount.Id;
            result.ClubPrice = discountProduct.ClubPrice;

            return Result<ClubDiscountCalculationResult>.Success(result);
        }

        public async Task<Result<ClubDiscountDto>> GetDiscountByIdAsync(int id)
        {
            var discount = await _unitOfWork.ClubDiscounts.GetDiscountWithProductsAsync(id);
            if (discount == null)
                return Result<ClubDiscountDto>.Failure("تخفیف یافت نشد");

            var dto = await MapToDto(discount);
            return Result<ClubDiscountDto>.Success(dto);
        }

        public async Task<Result<IEnumerable<ClubDiscountDto>>> GetActiveDiscountsAsync(int storeId)
        {
            var discounts = await _unitOfWork.ClubDiscounts.GetActiveDiscountsWithProductsAsync(storeId);
            var dtos = new List<ClubDiscountDto>();
            foreach (var d in discounts)
                dtos.Add(await MapToDto(d));
            return Result<IEnumerable<ClubDiscountDto>>.Success(dtos);
        }

        public async Task<Result<bool>> HasActiveDiscountForProductAsync(int productId, int storeId)
        {
            var has = await _unitOfWork.ClubDiscounts.HasActiveDiscountForProductAsync(productId, DateTime.Now, storeId);
            return Result<bool>.Success(has);
        }

        public async Task<Result<bool>> HasActiveDiscountForUnitAsync(int unitLevelId, int storeId)
        {
            var has = await _unitOfWork.ClubDiscounts.HasActiveDiscountForUnitAsync(unitLevelId, DateTime.Now, storeId);
            return Result<bool>.Success(has);
        }

        // ============ متدهای کمکی ============
        private async Task<ClubDiscountDto> MapToDto(ClubDiscount discount)
        {
            var dto = new ClubDiscountDto
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
                RefundToWallet = discount.RefundToWallet,
                StoreId = discount.StoreId,
                Products = new List<ClubDiscountProductDto>()
            };

            if (discount.Products != null)
            {
                foreach (var p in discount.Products)
                {
                    var prodDto = new ClubDiscountProductDto
                    {
                        ProductId = p.ProductId,
                        UnitLevelId = p.UnitLevelId,
                        ClubPrice = p.ClubPrice,
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
    }
}
