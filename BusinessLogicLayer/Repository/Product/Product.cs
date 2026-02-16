using BusinessEntity.Product;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Interface;
using BusinessLogicLayer.Interface.Customer_Club;
using BusinessLogicLayer.Interface.Producr;
using DataAccessLayer;
using DataAccessLayer.Interface;
using DataAccessLayer.Interface.Customer_Club;
using DataAccessLayer.Interface.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.Repository.Product
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublicDiscountService _publicDiscountService;
        private readonly IClubDiscountService _clubDiscountService;
        private readonly IProductRepository _productRepository;
        private readonly ILogService _logService;
        private readonly Database _context;
        private readonly ILogger<ProductService> _logger;

        public ProductService(
            IProductRepository productRepository,
            ILogService logService,
            Database context,
            ILogger<ProductService> logger,
            IUnitOfWork unitOfWork,
            IPublicDiscountService publicDiscountService,
            IClubDiscountService clubDiscountService)
        {
            _productRepository = productRepository;
            _logService = logService;
            _context = context;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _publicDiscountService = publicDiscountService;
            _clubDiscountService = clubDiscountService;
        }

        public async Task<Result<ProductBarcodeInfoDto>> GetProductInfoForInvoiceAsync(
    string barcode,
    int? peopleId = null,
    int? customerId = null,
    int? storeId = null)
        {
            // 1. اعتبارسنجی اولیه
            if (string.IsNullOrWhiteSpace(barcode))
                return Result<ProductBarcodeInfoDto>.Failure("بارکد نمی‌تواند خالی باشد");

            // 2. یافتن واحد کالا با بارکد
            var barcodeEntity = await _unitOfWork.ProductBarcodes.GetByBarcodeAsync(barcode);
            if (barcodeEntity == null)
                return Result<ProductBarcodeInfoDto>.Failure("بارکد یافت نشد");

            // 3. یافتن واحد کالا
            var unitLevel = await _unitOfWork.UnitsLevels.GetByIdAsync(barcodeEntity.ProductUnitId);
            if (unitLevel == null)
                return Result<ProductBarcodeInfoDto>.Failure("واحد کالا یافت نشد");

            // 4. یافتن محصول
            var product = await _unitOfWork.Products.GetByIdAsync(unitLevel.ProductId);
            if (product == null)
                return Result<ProductBarcodeInfoDto>.Failure("محصول یافت نشد");

            // 5. تعیین سطح قیمتی
            int priceLevelId = 1; // پیش‌فرض
            if (peopleId.HasValue)
            {
                var people = await _unitOfWork.People.GetByIdAsync(peopleId.Value);
                if (people != null)
                    priceLevelId = people.PriceLevelID;
            }
            else if (customerId.HasValue)
            {
                var customer = await _unitOfWork.Customers.GetByIdAsync(customerId.Value);
                if (customer?.PeopleId != null)
                {
                    var people = await _unitOfWork.People.GetByIdAsync(customer.PeopleId.Value);
                    if (people != null)
                        priceLevelId = people.PriceLevelID;
                }
            }

            // 6. محاسبه قیمت اصلی بر اساس سطح قیمتی
            int originalPrice = 0;
            var price = unitLevel.Prices.FirstOrDefault(p => p.PriceLevelId == priceLevelId);
            originalPrice = price != null ? (int)price.SalePrice : (int)product.SalePrice;

            // 7. محاسبه تخفیف عمومی
            int publicDiscount = 0;
            if (storeId.HasValue)
            {
                var publicResult = await _publicDiscountService.CalculatePublicDiscountAsync(
                    barcode, DateTime.Now, storeId.Value);
                if (publicResult.IsSuccess)
                    publicDiscount = publicResult.Data.DiscountAmount;
            }

            // 8. محاسبه تخفیف باشگاه (فقط در صورت وجود customerId)
            int clubDiscount = 0;
            if (customerId.HasValue)
            {
                var clubResult = await _clubDiscountService.CalculateClubDiscountAsync(
                    barcode, customerId.Value, originalPrice - publicDiscount);
                if (clubResult.IsSuccess)
                    clubDiscount = clubResult.Data.DiscountAmount;
            }

            // 9. قیمت نهایی
            int finalPrice = originalPrice - publicDiscount - clubDiscount;

            // 10. ساخت DTO
            var dto = new ProductBarcodeInfoDto
            {
                Barcode = barcode,
                ProductId = product.Id,
                ProductName = product.Name,
                UnitId = unitLevel.Id,
                UnitName = unitLevel.Title,
                ConversionFactor = unitLevel.ConversionFactor,
                BaseProductId = product.Id,
                BaseUnitId = unitLevel.UnitProductId != 0 ? unitLevel.UnitProductId : product.UnitProductId,
                OriginalPrice = originalPrice,
                FinalPrice = finalPrice,
                Discounts = new DiscountDetailDto
                {
                    PublicDiscount = publicDiscount,
                    ClubDiscount = clubDiscount,
                    LevelDiscount = 0
                },
                Stock = product.Inventory,
                ImagePath = product.ImagePath
            };

            return Result<ProductBarcodeInfoDto>.Success(dto);
        }

        public async Task<List<BusinessEntity.Product.Product>> GetActiveProductsWithShortcutKeyAsync()
        {
            return await _productRepository.GetActiveProductsWithShortcutKeyAsync();
        }
        public async Task<List<BusinessEntity.Product.Product>> GetActiveButtonProductsAsync()
        {
            return await _productRepository.GetActiveButtonProductsAsync();
        }

        public async Task<List<BusinessEntity.Product.Product>> GetActiveWeightyProductsAsync()
        {
            return await _productRepository.GetActiveWeightyProductsAsync();
        }

        public async Task<List<BusinessEntity.Product.Product>> GetActiveBarcodeProductsAsync()
        {
            return await _productRepository.GetActiveBarcodeProductsAsync();
        }
        public async Task<Result<ProductBarcodeInfoDto>> GetProductInfoByBarcodeAsync(
     string barcode,
     int? customerId = null,
     int? storeId = null)          // ✅ پارامتر storeId اضافه شد
        {
            // 1. یافتن بارکد
            var barcodeEntity = await _unitOfWork.ProductBarcodes.GetByBarcodeAsync(barcode);
            if (barcodeEntity == null)
                return Result<ProductBarcodeInfoDto>.Failure("بارکد یافت نشد");

            // 2. یافتن واحد کالا
            var unitLevel = await _unitOfWork.UnitsLevels.GetByIdAsync(barcodeEntity.ProductUnitId);
            if (unitLevel == null)
                return Result<ProductBarcodeInfoDto>.Failure("واحد کالا یافت نشد");

            // 3. یافتن محصول
            var product = await _unitOfWork.Products.GetByIdAsync(unitLevel.ProductId);
            if (product == null)
                return Result<ProductBarcodeInfoDto>.Failure("محصول یافت نشد");

            // 4. تعیین قیمت اصلی بر اساس سطح قیمتی مشتری (در صورت وجود)
            int originalPrice = 0;
            if (customerId.HasValue)
            {
                var customer = await _unitOfWork.Customers.GetByIdAsync(customerId.Value);
                if (customer?.PeopleId != null)
                {
                    var people = await _unitOfWork.People.GetByIdAsync(customer.PeopleId.Value);
                    if (people?.PriceLevelID != null)
                    {
                        var price = unitLevel.Prices
                            .FirstOrDefault(p => p.PriceLevelId == people.PriceLevelID);
                        if (price != null)
                            originalPrice = (int)price.SalePrice;
                    }
                }
            }

            if (originalPrice == 0)
            {
                var defaultPrice = unitLevel.Prices.FirstOrDefault(p => p.PriceLevelId == 1);
                originalPrice = defaultPrice != null
                    ? (int)defaultPrice.SalePrice
                    : (int)product.SalePrice;
            }

            // 5. محاسبه تخفیف‌ها
            int publicDiscount = 0;
            int clubDiscount = 0;

            // تخفیف عمومی (با استفاده از storeId ورودی)
            if (storeId.HasValue)
            {
                var publicResult = await _publicDiscountService.CalculatePublicDiscountAsync(
                    barcode,
                    DateTime.UtcNow,
                    storeId.Value);                        // ✅ استفاده از storeId ورودی
                if (publicResult.IsSuccess)
                    publicDiscount = publicResult.Data.DiscountAmount;
            }

            // تخفیف باشگاه
            if (customerId.HasValue)
            {
                var clubResult = await _clubDiscountService.CalculateClubDiscountAsync(
                    barcode,
                    customerId.Value,
                    originalPrice - publicDiscount);
                if (clubResult.IsSuccess)
                    clubDiscount = clubResult.Data.DiscountAmount;
            }

            int finalPrice = originalPrice - publicDiscount - clubDiscount;

            // ✅ رفع خطای `??` با استفاده از شرط ساده
            int baseUnitId = unitLevel.UnitProductId != 0
                ? unitLevel.UnitProductId
                : product.UnitProductId;

            var dto = new ProductBarcodeInfoDto
            {
                Barcode = barcode,
                ProductId = product.Id,
                ProductName = product.Name,
                UnitId = unitLevel.Id,
                UnitName = unitLevel.Title,
                ConversionFactor = unitLevel.ConversionFactor,
                BaseProductId = product.Id,
                BaseUnitId = baseUnitId,                     // ✅ اصلاح شده
                OriginalPrice = originalPrice,
                FinalPrice = finalPrice,
                Discounts = new DiscountDetailDto
                {
                    PublicDiscount = publicDiscount,
                    ClubDiscount = clubDiscount,
                    LevelDiscount = 0
                },
                Stock = product.Inventory,
                ImagePath = product.ImagePath
            };

            return Result<ProductBarcodeInfoDto>.Success(dto);
        }
        public async Task<IEnumerable<BusinessEntity.DTO.Product.ProductSalesByDateDto>> GetProductSalesReportByDateAsync(DateTime startDate, DateTime endDate, string? barcode = null)
        {
            _logger.LogInformation("Request to receive Product with ID: {startDate}{endDate}", startDate, endDate);
            var entity = await _productRepository.GetProductSalesReportByDateAsync(startDate, endDate, barcode);
            if (entity == null)
                _logger.LogWarning("Product with ID {startDate},{endDate} not found", startDate, endDate);
            else
                _logger.LogInformation("Product with ID {startDate},{endDate} was successfully found", startDate, endDate);

            return entity;
        }

        public async Task<IEnumerable<BusinessEntity.DTO.Product.ProductInventoryDto>> GetProductInventoryAsync(string? barcode = null)
        {
            _logger.LogInformation("Request to receive Product GetProductInventoryAsync ");
            var entity = await _productRepository.GetProductInventoryAsync(barcode);
            if (entity == null)
                _logger.LogWarning("Product with ID {barcode} not found", barcode);
            else
                _logger.LogInformation("Product with ID {barcode} was successfully found", barcode);

            return entity;
        }

        public async Task<IEnumerable<BusinessEntity.Product.Product>> GetAll()
        {
            return await _productRepository.GetAll();
        }

        public async Task<BusinessEntity.Product.Product?> GetById(int id)
        {
            return await _productRepository.GetById(id);
        }

        public async Task<List<BusinessEntity.Product.Product>> Search(
            string? name = null,
            string? barcode = null,
            int? typeProductId = null,
            bool? isActive = null,
            string? description = null,
            bool? isTax = null,
            int? groupId = null,
            int? storeroomId = null,
            int? unitId = null,
            int? sectionId = null)
        {
            return await _productRepository.Search(
                name, barcode, typeProductId, isActive,
                description, isTax, groupId, storeroomId,
                unitId, sectionId);
        }

        public async Task<Result> Create(BusinessEntity.Product.Product product, int userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // اعتبارسنجی
                var validationResult = ValidateProduct(product);
                if (!validationResult.IsSuccess)
                    return validationResult;

                // ایجاد محصول
                var repoResult = await _productRepository.Create(product);
                if (!repoResult.IsSuccess)
                    return Result.Failure(repoResult.Message);   // تبدیل به BusinessLogicLayer.Result

                // ثبت لاگ
                await _logService.CreateLogAsync(
                    $"ایجاد کالای جدید: {product.Name} (شناسه: {product.Id})",
                    userId);

                await transaction.CommitAsync();
                return Result.Success("کالا با موفقیت ایجاد شد.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "خطا در ایجاد کالا: {@Product}", product);
                return Result.Failure($"خطا در ایجاد کالا: {ex.Message}");
            }
        }

        public async Task<Result> Update(BusinessEntity.Product.Product product, int userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // اعتبارسنجی
                var validationResult = ValidateProduct(product);
                if (!validationResult.IsSuccess)
                    return validationResult;

                // به‌روزرسانی محصول
                var repoResult = await _productRepository.Update(product);
                if (!repoResult.IsSuccess)
                    return Result.Failure(repoResult.Message);

                // ثبت لاگ
                await _logService.CreateLogAsync(
                    $"به‌روزرسانی کالا: {product.Name} (شناسه: {product.Id})",
                    userId);

                await transaction.CommitAsync();
                return Result.Success("کالا با موفقیت به‌روزرسانی شد.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "خطا در به‌روزرسانی کالا: {@Product}", product);
                return Result.Failure($"خطا در به‌روزرسانی کالا: {ex.Message}");
            }
        }

        public async Task<Result> Delete(int id, int userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // دریافت اطلاعات محصول
                var product = await _productRepository.GetById(id);
                if (product == null)
                    return Result.Failure("کالا یافت نشد.");

                // حذف محصول
                var repoResult = await _productRepository.Delete(id);
                if (!repoResult.IsSuccess)
                    return Result.Failure(repoResult.Message);

                // ثبت لاگ
                await _logService.CreateLogAsync(
                    $"حذف کالا: {product.Name} (شناسه: {id})",
                    userId);

                await transaction.CommitAsync();
                return Result.Success("کالا با موفقیت حذف شد.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "خطا در حذف کالا با شناسه: {Id}", id);
                return Result.Failure($"خطا در حذف کالا: {ex.Message}");
            }
        }

        public async Task<List<BusinessEntity.Product.Product>> GetProductsForCombo()
        {
            return await _productRepository.GetProductsForCombo();
        }

        private Result ValidateProduct(BusinessEntity.Product.Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
                return Result.Failure("نام کالا الزامی است.");

            if (product.TypeProductId <= 0)
                return Result.Failure("نوع کالا باید انتخاب شود.");

            if (product.UnitProductId <= 0)
                return Result.Failure("واحد کالا باید انتخاب شود.");

            if (product.GroupProductId <= 0)
                return Result.Failure("گروه کالا باید انتخاب شود.");

            if (product.BuyPrice < 0)
                return Result.Failure("قیمت خرید نمی‌تواند منفی باشد.");

            if (product.SalePrice < 0)
                return Result.Failure("قیمت فروش نمی‌تواند منفی باشد.");

            if (product.Inventory < 0)
                return Result.Failure("موجودی نمی‌تواند منفی باشد.");

            // اعتبارسنجی سطوح واحد
            if (product.Units == null || !product.Units.Any())
                return Result.Failure("حداقل یک سطح واحد باید تعریف شود.");

            foreach (var unit in product.Units)
            {
                if (string.IsNullOrWhiteSpace(unit.Title))
                    return Result.Failure("عنوان سطح واحد الزامی است.");

                if (unit.ConversionFactor <= 0)
                    return Result.Failure("ضریب تبدیل باید بزرگتر از صفر باشد.");
            }

            return Result.Success("عملیات با موفقیت انجام شد.");
        }
    }
}
