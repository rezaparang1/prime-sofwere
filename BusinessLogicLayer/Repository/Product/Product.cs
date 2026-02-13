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
    //public class ProductService : Interface.Producr.IProductService
    //{
    //    private readonly DataAccessLayer.Interface.Product.IProductRepository _ProductRepository;
    //    private readonly ILogger<ProductService> _logger;

    //    public ProductService(DataAccessLayer.Interface.Product.IProductRepository ProductRepository, ILogger<ProductService> logger)
    //    {
    //        _ProductRepository = ProductRepository;
    //        _logger = logger;
    //    }
    //    //*******SEARCH*******


    //    public async Task<BusinessEntity.ProductDto?> GetProductByBarcodeAsync(string barcode)
    //    {
    //        _logger.LogInformation("Request to receive Product with ID: {Id}", barcode);
    //        var entity = await _ProductRepository.GetProductByBarcodeAsync(barcode);
    //        if (entity == null)
    //            _logger.LogWarning("Product with ID {barcode} not found", barcode);
    //        else
    //            _logger.LogInformation("Product with ID {barcode} was successfully found", barcode);

    //        return entity;
    //    }
    //    public async Task<List<BusinessEntity.Product.Product>> Search(string? Name = null, string? Barcode = null, int? TypeProduct = null, bool? IsActive = null, string? Description = null, bool? IsTax = null, int? GroupId = null, int? StoreroomId = null, int? UnitId = null, int? SectionId = null)
    //    {
    //        _logger.LogInformation("Request Group_Product search with Async: {Name}{Barcode}{TypeProduct}{IsActive}{Description}{IsTax}{GroupId}{StoreroomId}{UnitId}{SectionId}",
    //            Name, Barcode, TypeProduct, IsActive, Description, IsTax, GroupId, StoreroomId, UnitId, SectionId);
    //        var result = await _ProductRepository.Search(Name,Barcode,TypeProduct,IsActive,Description,IsTax,GroupId,StoreroomId,UnitId,SectionId);
    //        _logger.LogInformation("{Count} results found", result.Count);
    //        return result;
    //    }
    //    public async Task<List<BusinessEntity.Product.Product>> SearchbyweightedProducts(bool? Isweighty, bool? IsActive)
    //    {
    //        _logger.LogInformation("Request Group_Product search with Async: {Isweighty}{IsActive}",
    //           IsActive, Isweighty);
    //        var result = await _ProductRepository.SearchbyweightedProducts( IsActive, Isweighty);
    //        _logger.LogInformation("{Count} results found", result.Count);
    //        return result;
    //    }
    //    public async Task<List<BusinessEntity.Product.Product>> SearchbyButtonProducts()
    //    {
    //        _logger.LogInformation("Request Group_Product search with Async: Isweighty and IsActive");
    //        var result = await _ProductRepository.SearchbyButtonProducts();
    //        _logger.LogInformation("{Count} results found", result.Count);
    //        return result;
    //    }
    //    public async Task<BusinessEntity.Product.Product?> GetProductByBarcode(string? Barcode)
    //    {
    //        _logger.LogInformation("Request to receive Product with ID: {Id}", Barcode);
    //        var entity = await _ProductRepository.GetProductByBarcode(Barcode);
    //        if (entity == null)
    //            _logger.LogWarning("Product with ID {Barcode} not found", Barcode);
    //        else
    //            _logger.LogInformation("Product with ID {Barcode} was successfully found", Barcode);

    //        return entity;
    //    }
    //    public async Task<BusinessEntity.Product.Product?> GetByShortcutKey(string? ShortcutKey)
    //    {
    //        _logger.LogInformation("Request to receive Product with ID: {Id}", ShortcutKey);
    //        var entity = await _ProductRepository.GetByShortcutKey(ShortcutKey);
    //        if (entity == null)
    //            _logger.LogWarning("Product with ID {ShortcutKey} not found", ShortcutKey);
    //        else
    //            _logger.LogInformation("Product with ID {ShortcutKey} was successfully found", ShortcutKey);

    //        return entity;
    //    }
    //    //*******READ*********
    //    public async Task<IEnumerable<BusinessEntity.Product.Product>> GetAll()
    //    {
    //        _logger.LogInformation("Request to receive all Group_Product");
    //        var result = await _ProductRepository.GetAll();
    //        _logger.LogInformation("{Count} items received", result.Count());
    //        return result;
    //    }
    //    public async Task<IEnumerable<BusinessEntity.Product.ProductReportDto>> GetProductReport()
    //    {
    //        _logger.LogInformation("Request to receive all Group_Product");
    //        var result = await _ProductRepository.GetProductReport();
    //        _logger.LogInformation("{Count} items received", result.Count());
    //        return result;
    //    }
    //    public async Task<BusinessEntity.Product.Product?> GetById(int id)
    //    {
    //        _logger.LogInformation("Request to receive Group_Product with ID: {Id}", id);
    //        var entity = await _ProductRepository.GetById(id);
    //        if (entity == null)
    //            _logger.LogWarning("Group_Product with ID {Id} not found", id);
    //        else
    //            _logger.LogInformation("Group_Product with ID {Id} was successfully found", id);

    //        return entity;
    //    }
    //    //*****CRUD**********     
    //    public async Task<ServiceResult> Create(int userId, BusinessEntity.Product.Product product)
    //    {
    //        _logger.LogInformation("Request to add new Product: {@Product}", product);

    //        var repoResult = await _ProductRepository.Create(userId, product);

    //        var result = new ServiceResult(repoResult.Success, repoResult.Message);

    //        _logger.LogInformation("Add result: {@Result}", result);
    //        return result;
    //    }
    //    public async Task<ServiceResult> Update(int userId, BusinessEntity.Product.Product product)
    //    {
    //        _logger.LogInformation("Request to update Product: {@Product}", product);

    //        var existing = await _ProductRepository.GetById(product.Id);
    //        if (existing == null)
    //        {
    //            _logger.LogWarning("Product with ID: {Id} not found for update.", product.Id);
    //            return new ServiceResult(false, "کالا پیدا نشد.");
    //        }

    //        var repoResult = await _ProductRepository.Update(userId, product);
    //        var result = new ServiceResult(repoResult.Success, repoResult.Message);

    //        _logger.LogInformation("Update result: {@Result}", result);
    //        return result;
    //    }
    //    public async Task<ServiceResult> Delete(int userId, int productId)
    //    {
    //        _logger.LogInformation("Request to delete Product with ID: {Id}", productId);

    //        var repoResult = await _ProductRepository.Delete(userId, productId);
    //        var result = new ServiceResult(repoResult.Success, repoResult.Message);

    //        _logger.LogInformation("Delete result: {@Result}", result);
    //        return result;
    //    }
    //}
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
                    DateTime.Now,
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
