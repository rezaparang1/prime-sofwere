using BusinessEntity.DTO.Product;
using BusinessEntity.Product;
using DataAccessLayer.Interface.Product;
using DataAccessLayer.Repository.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccessLayer.Repository.Product
{
   
    public class ProductRepository : IProductRepository
    {
        private readonly Database _context;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(Database context, ILogger<ProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<BusinessEntity.Product.Product>> GetActiveProductsWithShortcutKeyAsync()
        {
            return await _context.Product
                .Include(p => p.TypeProduct)
                .Include(p => p.Unit_Product)
                .Include(p => p.SectionProduct)
                .Include(p => p.GroupProduct)
                .Include(p => p.Units)
                    .ThenInclude(u => u.UnitProduct)
                .Include(p => p.Units)
                    .ThenInclude(u => u.Barcodes)
                .Include(p => p.Units)
                    .ThenInclude(u => u.Prices)
                .Where(p => !p.IsDelete && p.IsActive && !string.IsNullOrEmpty(p.ShortcutKey))
                .OrderBy(p => p.Name)
                .ToListAsync();
        }
        public async Task<List<BusinessEntity.Product.Product>> GetActiveButtonProductsAsync()
        {
            return await _context.Product
                .Include(p => p.TypeProduct)
                .Include(p => p.Unit_Product)
                .Include(p => p.SectionProduct)
                .Include(p => p.GroupProduct)
                .Include(p => p.Units)
                    .ThenInclude(u => u.UnitProduct)
                .Include(p => p.Units)
                    .ThenInclude(u => u.Barcodes)
                .Include(p => p.Units)
                    .ThenInclude(u => u.Prices)
                .Where(p => !p.IsDelete && p.IsActive && p.IsIsButton)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<List<BusinessEntity.Product.Product>> GetActiveWeightyProductsAsync()
        {
            return await _context.Product
                .Include(p => p.TypeProduct)
                .Include(p => p.Unit_Product)
                .Include(p => p.SectionProduct)
                .Include(p => p.GroupProduct)
                .Include(p => p.Units)
                    .ThenInclude(u => u.UnitProduct)
                .Include(p => p.Units)
                    .ThenInclude(u => u.Barcodes)
                .Include(p => p.Units)
                    .ThenInclude(u => u.Prices)
                .Where(p => !p.IsDelete && p.IsActive && p.IsWeighty)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<List<BusinessEntity.Product.Product>> GetActiveBarcodeProductsAsync()
        {
            return await _context.Product
                .Include(p => p.TypeProduct)
                .Include(p => p.Unit_Product)
                .Include(p => p.SectionProduct)
                .Include(p => p.GroupProduct)
                .Include(p => p.Units)
                    .ThenInclude(u => u.UnitProduct)
                .Include(p => p.Units)
                    .ThenInclude(u => u.Barcodes)
                .Include(p => p.Units)
                    .ThenInclude(u => u.Prices)
                .Where(p => !p.IsDelete && p.IsActive && p.IsBarcode)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }
        public async Task<IEnumerable<ProductSalesByDateDto>> GetProductSalesReportByDateAsync(DateTime startDate, DateTime endDate, string? barcode = null)
        {
            _logger.LogInformation(
                "Sales report generation started for range {Start} to {End}, barcode: {Barcode}.",
                startDate, endDate, barcode);

            // اطمینان از اینکه تاریخ‌ها UTC هستند
            startDate = DateTime.SpecifyKind(startDate, DateTimeKind.Utc);
            endDate = DateTime.SpecifyKind(endDate, DateTimeKind.Utc);

            var query = _context.Invoices_Item
                .Where(ii => ii.Invoices != null &&
                             ii.Invoices.Date >= startDate &&
                             ii.Invoices.Date <= endDate);

            if (!string.IsNullOrEmpty(barcode))
            {
                query = query.Where(ii => ii.Product.Units
                                             .SelectMany(u => u.Barcodes)
                                             .Any(b => b.Barcode == barcode));
            }

            var groupedData = await query
                .GroupBy(ii => ii.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    Sales = g.GroupBy(x => x.Invoices.Date)
                             .Select(dg => new
                             {
                                 Date = dg.Key,
                                 Qty = dg.Sum(x => x.Number)
                             })
                             .ToList()
                })
                .ToListAsync();

            var productIds = groupedData.Select(g => g.ProductId).ToList();
            var products = await _context.Product
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id, p => p.Name);

            var result = groupedData.Select(g => new ProductSalesByDateDto
            {
                ProductId = g.ProductId,
                ProductName = products.ContainsKey(g.ProductId) ? products[g.ProductId] : "",
                Sales = g.Sales
                         .Select(s => new SaleDto
                         {
                             Date = ToPersianDate(s.Date),
                             Qty = s.Qty
                         })
                         .OrderBy(s => s.Date)
                         .ToList()
            }).ToList();

            _logger.LogInformation(
                "{Count} product sales records retrieved between {Start} and {End}, barcode: {Barcode}.",
                result.Count, startDate, endDate, barcode);

            return result;
        }
        private string ToPersianDate(DateTime date)
        {
            var pc = new System.Globalization.PersianCalendar();
            return $"{pc.GetYear(date):0000}/{pc.GetMonth(date):00}/{pc.GetDayOfMonth(date):00}";
        }
        public async Task<IEnumerable<ProductInventoryDto>> GetProductInventoryAsync(string? barcode = null)
        {
            var query = _context.Product
                .SelectMany(p => p.Units
                                  .SelectMany(u => u.Barcodes)
                                  .DefaultIfEmpty(), (p, b) => new { Product = p, Barcode = b })
                .AsQueryable();

            if (!string.IsNullOrEmpty(barcode))
            {
                query = query.Where(x => x.Barcode != null && x.Barcode.Barcode == barcode);
            }

            var result = await query
                .Select(x => new ProductInventoryDto
                {
                    Barcode = x.Barcode != null ? x.Barcode.Barcode : "",
                    ProductName = x.Product.Name,
                    Inventory = x.Product.Inventory,
                    MinInventory = x.Product.MinInventory,
                    MaxInventory = x.Product.MaxInventory
                })
                .ToListAsync();

            return result;
        }
        // ***** GetAll *****
        public async Task<IEnumerable<BusinessEntity.Product.Product>> GetAll()
        {
            return await _context.Product
                .Include(p => p.TypeProduct)
                .Include(p => p.Unit_Product)
                .Include(p => p.SectionProduct)
                .Include(p => p.GroupProduct)
                .Include(p => p.Units)
                    .ThenInclude(u => u.UnitProduct)
                .Include(p => p.Units)
                    .ThenInclude(u => u.Barcodes)
                .Include(p => p.Units)
                    .ThenInclude(u => u.Prices)
                        .ThenInclude(p => p.PriceLevel)
                .Where(p => !p.IsDelete)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        // ***** GetById *****
        public async Task<BusinessEntity.Product.Product?> GetById(int id)
        {
            return await _context.Product
                .Include(p => p.TypeProduct)
                .Include(p => p.Unit_Product)
                .Include(p => p.SectionProduct)
                .Include(p => p.GroupProduct)
                .Include(p => p.Units)
                    .ThenInclude(u => u.UnitProduct)
                .Include(p => p.Units)
                    .ThenInclude(u => u.Barcodes)
                .Include(p => p.Units)
                    .ThenInclude(u => u.Prices)
                        .ThenInclude(p => p.PriceLevel)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDelete);
        }

        // ***** Search *****
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
            var query = _context.Product
                .Include(p => p.TypeProduct)
                .Include(p => p.Unit_Product)
                .Include(p => p.SectionProduct)
                .Include(p => p.GroupProduct)
                .Include(p => p.Units)
                    .ThenInclude(u => u.UnitProduct)
                .Include(p => p.Units)
                    .ThenInclude(u => u.Barcodes)
                .Include(p => p.Units)
                    .ThenInclude(u => u.Prices)
                        .ThenInclude(p => p.PriceLevel)
                .Where(p => !p.IsDelete)
                .AsQueryable();

            // اعمال فیلترها
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(p => p.Name.Contains(name));

            if (!string.IsNullOrWhiteSpace(barcode))
                query = query.Where(p => p.Units.Any(u =>
                    u.Barcodes.Any(b => b.Barcode.Contains(barcode))));

            if (typeProductId.HasValue && typeProductId > 0)
                query = query.Where(p => p.TypeProductId == typeProductId.Value);

            if (isActive.HasValue)
                query = query.Where(p => p.IsActive == isActive.Value);

            if (!string.IsNullOrWhiteSpace(description))
                query = query.Where(p => p.Description.Contains(description));

            if (isTax.HasValue)
                query = query.Where(p => p.IsTax == isTax.Value);

            if (groupId.HasValue && groupId > 0)
                query = query.Where(p => p.GroupProductId == groupId.Value);

            if (storeroomId.HasValue && storeroomId > 0)
                query = query.Where(p => p.StoreroomProductId == storeroomId.Value);

            if (unitId.HasValue && unitId > 0)
                query = query.Where(p => p.UnitProductId == unitId.Value);

            if (sectionId.HasValue && sectionId > 0)
                query = query.Where(p => p.SectionProductId == sectionId.Value);

            return await query.OrderBy(p => p.Name).ToListAsync();
        }

        // ***** Create *****
        public async Task<Result> Create(BusinessEntity.Product.Product product)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // بررسی تکراری بودن نام
                if (await _context.Product.AnyAsync(p => p.Name == product.Name && !p.IsDelete))
                    return Result.Failure("نام کالا تکراری است.");

                // بررسی تکراری بودن کلید میانبر
                if (!string.IsNullOrEmpty(product.ShortcutKey) &&
                    await _context.Product.AnyAsync(p => p.ShortcutKey == product.ShortcutKey && !p.IsDelete))
                    return Result.Failure("کلید میانبر تکراری است.");

                // بررسی بارکدهای تکراری
                var allBarcodes = product.Units
                    .SelectMany(u => u.Barcodes)
                    .Select(b => b.Barcode)
                    .Where(b => !string.IsNullOrWhiteSpace(b))
                    .ToList();

                if (allBarcodes.Any())
                {
                    var existingBarcodes = await _context.ProductBarcodes
                        .Where(b => allBarcodes.Contains(b.Barcode))
                        .Select(b => b.Barcode)
                        .ToListAsync();

                    if (existingBarcodes.Any())
                        return Result.Failure($"بارکدهای تکراری: {string.Join(", ", existingBarcodes)}");
                }

                // تنظیم تاریخ
                product.Date = DateTime.UtcNow;
                product.IsDelete = false;

                // تنظیم ارتباطات
                foreach (var unit in product.Units)
                {
                    unit.Product = product;

                    foreach (var barcode in unit.Barcodes)
                        barcode.ProductUnit = unit;

                    foreach (var price in unit.Prices)
                        price.ProductUnit = unit;
                }

                // ذخیره
                await _context.Product.AddAsync(product);
                await _context.SaveChangesAsync();
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

        // ***** Update *****
        public async Task<Result> Update(BusinessEntity.Product.Product updatedProduct)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // دریافت محصول موجود
                var existingProduct = await _context.Product
                    .Include(p => p.Units)
                        .ThenInclude(u => u.Barcodes)
                    .Include(p => p.Units)
                        .ThenInclude(u => u.Prices)
                    .FirstOrDefaultAsync(p => p.Id == updatedProduct.Id && !p.IsDelete);

                if (existingProduct == null)
                    return Result.Failure("کالا یافت نشد.");

                // بررسی تکراری بودن نام
                if (await _context.Product.AnyAsync(p =>
                    p.Id != updatedProduct.Id && p.Name == updatedProduct.Name && !p.IsDelete))
                    return Result.Failure("نام کالا تکراری است.");

                // بررسی تکراری بودن کلید میانبر
                if (!string.IsNullOrEmpty(updatedProduct.ShortcutKey) &&
                    await _context.Product.AnyAsync(p =>
                        p.Id != updatedProduct.Id && p.ShortcutKey == updatedProduct.ShortcutKey && !p.IsDelete))
                    return Result.Failure("کلید میانبر تکراری است.");

                // به‌روزرسانی فیلدهای اصلی
                _context.Entry(existingProduct).CurrentValues.SetValues(updatedProduct);

                // مدیریت Units
                await UpdateProductUnits(existingProduct, updatedProduct.Units.ToList());

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Result.Success("کالا با موفقیت به‌روزرسانی شد.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "خطا در به‌روزرسانی کالا: {@Product}", updatedProduct);
                return Result.Failure($"خطا در به‌روزرسانی کالا: {ex.Message}");
            }
        }

        private async Task UpdateProductUnits(BusinessEntity.Product.Product existingProduct, List<UnitsLevel> newUnits)
        {
            // حذف واحدهای حذف‌شده
            var unitsToRemove = existingProduct.Units
                .Where(eu => !newUnits.Any(nu => nu.Id == eu.Id))
                .ToList();

            foreach (var unit in unitsToRemove)
            {
                _context.ProductBarcodes.RemoveRange(unit.Barcodes);
                _context.ProductPrices.RemoveRange(unit.Prices);
                _context.UnitsLevel.Remove(unit);
            }

            // پردازش واحدها
            foreach (var newUnit in newUnits)
            {
                var existingUnit = existingProduct.Units
                    .FirstOrDefault(u => u.Id == newUnit.Id);

                if (existingUnit == null)
                {
                    // واحد جدید
                    newUnit.ProductId = existingProduct.Id;
                    existingProduct.Units.Add(newUnit);
                }
                else
                {
                    // به‌روزرسانی واحد موجود
                    _context.Entry(existingUnit).CurrentValues.SetValues(newUnit);

                    // مدیریت بارکدها
                    await UpdateUnitBarcodes(existingUnit, newUnit.Barcodes.ToList());

                    // مدیریت قیمت‌ها
                    await UpdateUnitPrices(existingUnit, newUnit.Prices.ToList());
                }
            }
        }

        private async Task UpdateUnitBarcodes(UnitsLevel existingUnit, List<ProductBarcodes> newBarcodes)
        {
            // حذف بارکدهای حذف‌شده
            var barcodesToRemove = existingUnit.Barcodes
                .Where(eb => !newBarcodes.Any(nb => nb.Id == eb.Id))
                .ToList();

            _context.ProductBarcodes.RemoveRange(barcodesToRemove);

            // بررسی بارکدهای جدید برای تکراری نبودن
            foreach (var newBarcode in newBarcodes.Where(nb => nb.Id == 0))
            {
                if (await _context.ProductBarcodes.AnyAsync(b => b.Barcode == newBarcode.Barcode))
                    throw new Exception($"بارکد {newBarcode.Barcode} تکراری است.");

                newBarcode.ProductUnitId = existingUnit.Id;
                existingUnit.Barcodes.Add(newBarcode);
            }
        }

        private async Task UpdateUnitPrices(UnitsLevel existingUnit, List<ProductPrices> newPrices)
        {
            // حذف قیمت‌های حذف‌شده
            var pricesToRemove = existingUnit.Prices
                .Where(ep => !newPrices.Any(np => np.Id == ep.Id))
                .ToList();

            _context.ProductPrices.RemoveRange(pricesToRemove);

            // افزودن قیمت‌های جدید
            foreach (var newPrice in newPrices.Where(np => np.Id == 0))
            {
                newPrice.UnitLevelId = existingUnit.Id;
                existingUnit.Prices.Add(newPrice);
            }
        }

        // ***** Delete *****
        public async Task<Result> Delete(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var product = await _context.Product
                    .FirstOrDefaultAsync(p => p.Id == id && !p.IsDelete);

                if (product == null)
                    return Result.Failure("کالا یافت نشد.");

                // بررسی استفاده در فاکتور
                bool isUsed = await CheckProductExistsInInvoice(id);
                if (isUsed)
                    return Result.Failure("این کالا در فاکتور استفاده شده و قابل حذف نیست.");

                // Soft Delete
                product.IsDelete = true;
                await _context.SaveChangesAsync();
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

        // ***** CheckProductExistsInInvoice *****
        public async Task<bool> CheckProductExistsInInvoice(int productId)
        {
            return await _context.Invoices_Item
                .AnyAsync(ii => ii.ProductId == productId);
        }

        // ***** GetProductsForCombo *****
        public async Task<List<BusinessEntity.Product.Product>> GetProductsForCombo()
        {
            return await _context.Product
                .Where(p => !p.IsDelete && p.IsActive)
                .OrderBy(p => p.Name)
                .Select(p => new BusinessEntity.Product.Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    SalePrice = p.SalePrice,
                    Inventory = p.Inventory
                })
                .ToListAsync();
        }
    }
}

