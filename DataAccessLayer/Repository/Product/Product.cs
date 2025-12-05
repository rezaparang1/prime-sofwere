using BusinessEntity.Product;
using DataAccessLayer.Interface.Product;
using DataAccessLayer.Repository.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Product
{
    public class ProductRepository : Interface.Product.IProductRepository
    {
        private readonly Database _context;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(Database context, ILogger<ProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        //*****SEARCH*****
        public async Task<IEnumerable<ProductSalesByDateDto>> GetProductSalesReportByDateAsync(
     DateTime startDate,
     DateTime endDate,
     string? barcode = null)
        {
            _logger.LogInformation(
                "Sales report generation started for range {Start} to {End}, barcode: {Barcode}.",
                startDate, endDate, barcode);

            // اطمینان از اینکه تاریخ‌ها UTC هستند
            startDate = DateTime.SpecifyKind(startDate, DateTimeKind.Utc);
            endDate = DateTime.SpecifyKind(endDate, DateTimeKind.Utc);

            // گام 1: فیلتر آیتم‌های فاکتور بر اساس تاریخ
            var query = _context.Invoices_Item
                .Where(ii => ii.Invoices != null &&
                             ii.Invoices.Date >= startDate &&
                             ii.Invoices.Date <= endDate);

            // گام 2: فیلتر اختیاری بر اساس بارکد
            if (!string.IsNullOrEmpty(barcode))
            {
                query = query.Where(ii => ii.Product.Units
                                             .SelectMany(u => u.Barcodes)
                                             .Any(b => b.Barcode == barcode));
            }

            // گام 3: گروه‌بندی روی ProductId و جمع فروش روزانه (فقط ستون‌های مستقیم)
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

            // گام 4: گرفتن نام محصول از جدول Product بعد از GroupBy
            var productIds = groupedData.Select(g => g.ProductId).ToList();
            var products = await _context.Product
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id, p => p.Name);

            // گام 5: ساخت DTO نهایی و تبدیل تاریخ به شمسی
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
            // query محصولات و بارکدها
            var query = _context.Product
                .SelectMany(p => p.Units
                                  .SelectMany(u => u.Barcodes)
                                  .DefaultIfEmpty(), (p, b) => new { Product = p, Barcode = b })
                .AsQueryable();

            // فیلتر اختیاری بر اساس بارکد
            if (!string.IsNullOrEmpty(barcode))
            {
                query = query.Where(x => x.Barcode != null && x.Barcode.Barcode == barcode);
            }

            // projection به DTO
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

        public async Task<BusinessEntity.ProductDto?> GetProductByBarcodeAsync(string barcode)
        {
            var productUnit = await _context.ProductBarcodes
                .Include(pb => pb.ProductUnit)
                    .ThenInclude(u => u.Product)
                .Include(pb => pb.ProductUnit)
                    .ThenInclude(u => u.Prices)
                        .ThenInclude(p => p.PriceLevel)
                .Include(pb => pb.ProductUnit)
                    .ThenInclude(u => u.Barcodes)
                .FirstOrDefaultAsync(pb => pb.Barcode == barcode);

            if (productUnit == null)
                return null;

            var unit = productUnit.ProductUnit;
            var product = unit.Product;

            return new BusinessEntity.ProductDto
            {
                ProductId = product.Id,
                ProductName = product.Name,
                UnitId = unit.Id,
                UnitProductId = unit.UnitProductId,
                UnitConversionFactor = unit.ConversionFactor,
                Barcodes = unit.Barcodes.Select(b => b.Barcode).ToList(),
                Prices = unit.Prices.Select(p => new BusinessEntity.PriceDto
                {
                    PriceLevelId = p.PriceLevelId,
                    PriceLevelName = p.PriceLevel.Name,
                    BuyPrice = p.BuyPrice,
                    Profit = p.Profit,
                    SalePrice = p.SalePrice
                }).ToList()
            };
        }
        public async Task<List<BusinessEntity.Product.Product>> SearchbyweightedProducts(bool? Isweighty, bool? IsActive)
        {
            _logger.LogInformation("Searching for Aync: {Isweighty},{IsActive}", Isweighty, IsActive);
            var query = _context.Product.AsQueryable();
            if (Isweighty != null || IsActive != null)
            {
                query = query.Where(r => r.IsWeighty == true || r.IsActive == true);
            }
            var result = await query.ToListAsync();

            _logger.LogInformation("{Count} results found for search Aync: {Isweighty},{IsActive}", result.Count, Isweighty, IsActive);
            return result;
        }
        public async Task<List<BusinessEntity.Product.Product>> SearchbyButtonProducts()
        {
            _logger.LogInformation("Searching for Aync:IsButton and IsActive ");
            var query = _context.Product.AsQueryable();
            query = query.Where(r => r.IsIsButton == true || r.IsActive == true); 
            var result = await query.ToListAsync();

            _logger.LogInformation("{Count} results found for search Aync: IsButton and IsActive", result.Count);
            return result;
        }
        public async Task<BusinessEntity.Product.Product?> GetProductByBarcode(string? barcode)
        {
            if (string.IsNullOrEmpty(barcode))
                return null;

            var productBarcode = await _context.ProductBarcodes
                .Include(b => b.ProductUnit) 
                .FirstOrDefaultAsync(b => b.Barcode == barcode);

            if (productBarcode?.ProductUnit == null)
                return null;

            var product = await _context.Product
                .Include(p => p.Units)
                    .ThenInclude(u => u.Barcodes)
                .Include(p => p.Units)
                    .ThenInclude(u => u.Prices)
                        .ThenInclude(p => p.PriceLevel)
                .FirstOrDefaultAsync(p => p.Id == productBarcode.ProductUnit.ProductId);

            return product;
        }
        public async Task<BusinessEntity.Product.Product?> GetByShortcutKey(string? ShortcutKey)
        {
            _logger.LogInformation("Searching for Barcode: {ShortcutKey}", ShortcutKey);
            var query = _context.Product.AsQueryable();
            if (!string.IsNullOrEmpty(ShortcutKey))
            {
                query = query.Where(r => r.ShortcutKey == ShortcutKey);
            }
            var result = await
                query.SingleAsync();

            _logger.LogInformation(" results found for search ShortcutKey: {ShortcutKey}", ShortcutKey);
            return result;
        }
        public async Task<List<BusinessEntity.Product.Product>> Search(string? Name = null,string? Barcode = null,int? TypeProduct = null,bool? IsActive = null,string? Description = null,bool? IsTax = null,int? GroupId = null,int? StoreroomId = null,int? UnitId = null,int? SectionId = null)
        {
            _logger.LogInformation("Searching for Async: {Name}{Barcode}{TypeProduct}{IsActive}{Description}{IsTax}{GroupId}{StoreroomId}{UnitId}{SectionId}",
                Name, Barcode, TypeProduct, IsActive, Description, IsTax, GroupId, StoreroomId, UnitId, SectionId);

            var query = _context.Product
                .Include(p => p.Units) 
                    .ThenInclude(u => u.Barcodes) 
                .Include(p => p.Units)
                    .ThenInclude(u => u.Prices) 
                .AsQueryable();

            if (!string.IsNullOrEmpty(Name))
            {
                query = query.Where(r => r.Name.Contains(Name));
            }

            if (!string.IsNullOrEmpty(Barcode)) 
            {
                query = query.Where(r => r.Units
                    .Any(u => u.Barcodes
                        .Any(b => b.Barcode == Barcode)));
            }

            if (TypeProduct.HasValue) 
            {
                query = query.Where(r => r.TypeProductId == TypeProduct.Value);
            }

            if (IsActive.HasValue)
            {
                query = query.Where(r => r.IsActive == IsActive);
            }

            if (!string.IsNullOrEmpty(Description))
            {
                query = query.Where(r => r.Description.Contains(Description));
            }

            if (IsTax.HasValue)
            {
                query = query.Where(r => r.IsTax == IsTax);
            }

            if (GroupId.HasValue)
            {
                query = query.Where(r => r.GroupProductId == GroupId);
            }

            if (StoreroomId.HasValue)
            {
                query = query.Where(r => r.StoreroomProductId == StoreroomId);
            }

            if (UnitId.HasValue)
            {
                query = query.Where(r => r.UnitProductId == UnitId);
            }

            if (SectionId.HasValue)
            {
                query = query.Where(r => r.SectionProductId == SectionId);
            }

            var result = await query.ToListAsync();

            _logger.LogInformation("{Count} results found for search Async", result.Count);
            return result;
        }
        //******READ*******
        public async Task<IEnumerable<BusinessEntity.Product.ProductReportDto>> GetProductReport()
        {
            _logger.LogInformation("Product report generation started.");

            var result = await _context.Product
                .SelectMany(p => p.Units, (p, u) => new { Product = p, Unit = u })
                .SelectMany(pu => pu.Unit.Barcodes, (pu, b) => new { pu.Product, pu.Unit, Barcode = b })
                .SelectMany(pub => pub.Unit.Prices, (pub, price) => new ProductReportDto
                {
                    Barcode = pub.Barcode.Barcode,
                    ProductName = pub.Product.Name,
                    Inventory = pub.Product.Inventory,
                    BuyPrice = price.BuyPrice,
                    SalePrice = price.SalePrice
                })
                .ToListAsync();

            _logger.LogInformation("{Count} product report records retrieved.", result.Count);

            return result;
        }
        public async Task<IEnumerable<BusinessEntity.Product.Product>> GetAll()
        {
            _logger.LogInformation("All Product have started to be received from the database.");

            var result = await _context.Product.ToListAsync();

            _logger.LogInformation("{Count} records received.", result.Count);
            return result;
        }
        public async Task<BusinessEntity.Product.Product?> GetById(int id)
        {
            _logger.LogInformation("Request to receive Product  with ID: {Id}", id);

            var entity = await _context.Product.FindAsync(id);

            if (entity == null)
                _logger.LogWarning("Product  with ID: {Id} not found", id);
            else
                _logger.LogInformation("Product name with ID: {Id} successfully retrieved", id);

            return entity;
        }
        //******CRUD*****
        public async Task<ServiceResult> Create(int userId, BusinessEntity.Product.Product product)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _logger.LogInformation("Adding new Product: {@Product}", product);

                // 🔹 چک نام تکراری
                bool duplicateName = await _context.Product
                    .AsNoTracking()
                    .AnyAsync(p => p.Name == product.Name);

                if (duplicateName)
                    return new ServiceResult(false, "نام کالا وارد شده تکراری است.");

                // 🔹 چک کلید میانبر
                if (!string.IsNullOrEmpty(product.ShortcutKey))
                {
                    bool duplicateShortcut = await _context.Product
                        .AsNoTracking()
                        .AnyAsync(p => p.ShortcutKey == product.ShortcutKey);

                    if (duplicateShortcut)
                        return new ServiceResult(false, "کلید سریع کالا وارد شده تکراری است.");
                }

                // 🔹 فیلتر بارکدهای خالی
                foreach (var unit in product.Units)
                {
                    unit.Barcodes = unit.Barcodes
                        .Where(b => !string.IsNullOrWhiteSpace(b.Barcode))
                        .ToList();
                }

                // 🔹 چک بارکدهای تکراری در دیتابیس
                var barcodes = product.Units.SelectMany(u => u.Barcodes.Select(b => b.Barcode)).ToList();
                if (barcodes.Any())
                {
                    var existingBarcodes = await _context.ProductBarcodes
                        .AsNoTracking()
                        .Where(b => barcodes.Contains(b.Barcode))
                        .Select(b => b.Barcode)
                        .ToListAsync();

                    if (existingBarcodes.Any())
                        return new ServiceResult(false, $"بارکد {string.Join(", ", existingBarcodes)} تکراری است.");
                }

                // 🔹 ست کردن navigation properties
                foreach (var unit in product.Units)
                {
                    unit.Product = product;

                    foreach (var price in unit.Prices)
                        price.ProductUnit = unit;

                    foreach (var barcode in unit.Barcodes)
                        barcode.ProductUnit = unit;
                }

                // 🔹 اضافه کردن محصول با تمام Units، Prices و Barcodes
                await _context.Product.AddAsync(product);
                await _context.SaveChangesAsync(); // EF Core خودش FKها رو درست مدیریت میکنه

                await transaction.CommitAsync();
                _logger.LogInformation("Product added successfully. ID: {Id}", product.Id);

                return new ServiceResult(true, "عملیات با موفقیت انجام شد.");
            }
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Database error while adding Product: {Error}, Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return new ServiceResult(false, $"خطای دیتابیس: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Unexpected error while adding Product: {@Product}", product);
                return new ServiceResult(false, "خطای غیرمنتظره رخ داد. لطفاً با پشتیبانی تماس بگیرید.");
            }
        }
        public async Task<ServiceResult> Update(int userId, BusinessEntity.Product.Product updatedProduct)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var existingProduct = await _context.Product
                    .Include(p => p.Units)
                        .ThenInclude(u => u.Barcodes)
                    .Include(p => p.Units)
                        .ThenInclude(u => u.Prices)
                    .FirstOrDefaultAsync(p => p.Id == updatedProduct.Id);

                if (existingProduct == null)
                    return new ServiceResult(false, "کالا پیدا نشد.");

                if (await _context.Product.AsNoTracking()
                    .AnyAsync(p => p.Id != updatedProduct.Id && p.Name == updatedProduct.Name))
                {
                    return new ServiceResult(false, "نام کالا وارد شده تکراری است.");
                }

                if (!string.IsNullOrEmpty(updatedProduct.ShortcutKey) &&
                    await _context.Product.AsNoTracking()
                    .AnyAsync(p => p.Id != updatedProduct.Id && p.ShortcutKey == updatedProduct.ShortcutKey))
                {
                    return new ServiceResult(false, "کلید سریع کالا وارد شده تکراری است.");
                }

                _context.Entry(existingProduct).CurrentValues.SetValues(updatedProduct);

                foreach (var updatedUnit in updatedProduct.Units)
                {
                    var existingUnit = existingProduct.Units
                        .FirstOrDefault(u => u.Id == updatedUnit.Id);

                    if (existingUnit == null)
                    {
                        existingProduct.Units.Add(updatedUnit); 
                    }
                    else
                    {
                        _context.Entry(existingUnit).CurrentValues.SetValues(updatedUnit);

                        foreach (var updatedBarcode in updatedUnit.Barcodes)
                        {
                            if (!existingUnit.Barcodes.Any(b => b.Id == updatedBarcode.Id))
                            {
                                if (await _context.ProductBarcodes.AsNoTracking()
                                    .AnyAsync(b => b.Barcode == updatedBarcode.Barcode))
                                {
                                    return new ServiceResult(false, $"بارکد {updatedBarcode.Barcode} تکراری است.");
                                }
                                existingUnit.Barcodes.Add(updatedBarcode);
                            }
                        }

                        foreach (var updatedPrice in updatedUnit.Prices)
                        {
                            var existingPrice = existingUnit.Prices
                                .FirstOrDefault(p => p.Id == updatedPrice.Id);

                            if (existingPrice == null)
                                existingUnit.Prices.Add(updatedPrice);
                            else
                                _context.Entry(existingPrice).CurrentValues.SetValues(updatedPrice);
                        }

                        existingUnit.Barcodes
                            .Where(b => !updatedUnit.Barcodes.Any(ub => ub.Id == b.Id))
                            .ToList()
                            .ForEach(b => _context.ProductBarcodes.Remove(b));

                        existingUnit.Prices
                            .Where(p => !updatedUnit.Prices.Any(up => up.Id == p.Id))
                            .ToList()
                            .ForEach(p => _context.ProductPrices.Remove(p));
                    }
                }

                existingProduct.Units
                    .Where(u => !updatedProduct.Units.Any(uu => uu.Id == u.Id))
                    .ToList()
                    .ForEach(u => _context.UnitsLevel.Remove(u));

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ServiceResult(true, "آپدیت کالا با موفقیت انجام شد.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error updating product: {@Product}", updatedProduct);
                return new ServiceResult(false, "خطایی در آپدیت کالا رخ داد.");
            }
        }
        public async Task<ServiceResult> Delete(int userId, int productId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var product = await _context.Product
                    .Include(p => p.Units)
                        .ThenInclude(u => u.Barcodes)
                    .Include(p => p.Units)
                        .ThenInclude(u => u.Prices)
                    .FirstOrDefaultAsync(p => p.Id == productId);

                if (product == null)
                    return new ServiceResult(false, "کالا پیدا نشد.");

                bool isUsedInInvoice = await _context.Invoices
                    .AnyAsync(i => i.Invoices_Item.Any(ii => ii.ProductId == productId));

                if (isUsedInInvoice)
                    return new ServiceResult(false, "این کالا در فاکتور یا تراکنش استفاده شده و نمی‌توان آن را حذف کرد.");

               
                _context.Product.Remove(product);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ServiceResult(true, "کالا با موفقیت حذف شد.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error deleting product with ID {ProductId}", productId);
                return new ServiceResult(false, "خطایی در حذف کالا رخ داد.");
            }
        }
    }
}
