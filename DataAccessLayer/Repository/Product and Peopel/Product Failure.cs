using DataAccessLayer.Repository.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity.Product;
namespace DataAccessLayer.Repository.Product
{
    //public class ProductFaliureRepository : Interface.Product.IProductFailureRepository
    //{
    //    private readonly Database _context;
    //    private readonly ILogger<ProductFaliureRepository> _logger;

    //    public ProductFaliureRepository(Database context, ILogger<ProductFaliureRepository> logger)
    //    {
    //        _context = context;
    //        _logger = logger;
    //    }
    //    //*****SEARCH*****
    //    public async Task<List<ProductFailureItemDto>> SearchAsync(ProductFailureSearchFilter filter, CancellationToken cancellationToken = default)
    //    {
    //        var query = _context.Product_Failure_Item
    //            .Include(i => i.Storeroom_Product)
    //            .Include(i => i.ProductFailure)
    //            .Include(i => i.Product) // حالا که اضافه شد
    //            .AsQueryable();

    //        // فیلتر بر اساس انبارها
    //        if (filter.StoreroomIds != null && filter.StoreroomIds.Any())
    //            query = query.Where(i => filter.StoreroomIds.Contains(i.StoreroomId));

    //        // فیلتر بر اساس کالاها
    //        if (filter.ProductIds != null && filter.ProductIds.Any())
    //            query = query.Where(i => i.ProductId != null && filter.ProductIds.Contains(i.ProductId.Value));

    //        // فیلتر تاریخ
    //        if (filter.FromDate.HasValue)
    //            query = query.Where(i => i.ProductFailure.Date >= filter.FromDate.Value);

    //        if (filter.ToDate.HasValue)
    //            query = query.Where(i => i.ProductFailure.Date <= filter.ToDate.Value);

    //        // اجرای query و تبدیل به DTO مشابه GetAll
    //        var result = await query
    //            .Select(i => new ProductFailureItemDto
    //            {
    //                FailureId = i.ProductFailure.Id,
    //                Date = i.ProductFailure.Date,
    //                StoreroomName = i.Storeroom_Product.Name,
    //                Barcode = i.Barcode,
    //                ProductName = i.Product != null ? i.Product.Name : i.Name,
    //                Quantity = i.Value
    //            })
    //            .OrderByDescending(x => x.Date)
    //            .ThenBy(x => x.ProductName)
    //            .ToListAsync(cancellationToken);

    //        return result;
    //    }

    //    //******READ*******
    //    public async Task<List<ProductFailureItemDto>> GetAllDtoAsync(CancellationToken cancellationToken = default)
    //    {
    //        var list = await _context.Product_Failure
    //            .Include(f => f.ProductFailureItems)
    //                .ThenInclude(i => i.Storeroom_Product)
    //            .SelectMany(f => f.ProductFailureItems.Select(i => new ProductFailureItemDto
    //            {
    //                FailureId = f.Id,
    //                Date = f.Date,
    //                StoreroomName = i.Storeroom_Product.Name,
    //                Barcode = i.Barcode,
    //                ProductName = i.Name,
    //                Quantity = i.Value
    //            }))
    //            .OrderByDescending(x => x.Date)
    //            .ToListAsync(cancellationToken);

    //        return list;
    //    }

    //    public async Task<ProductFailureEditDto?> GetByIdForEditAsync(int id, CancellationToken cancellationToken = default)
    //    {
    //        var failure = await _context.Product_Failure
    //            .Include(f => f.ProductFailureItems)
    //                .ThenInclude(i => i.Storeroom_Product)
    //            .Include(f => f.ProductFailureItems)
    //                .ThenInclude(i => i.Storeroom_Product.Products) // اگر نیاز به اطلاعات محصول باشد
    //            .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);

    //        if (failure == null)
    //            return null;

    //        return new ProductFailureEditDto
    //        {
    //            FailureId = failure.Id,
    //            Date = failure.Date,
    //            Description = failure.Description,
    //            AccountId = failure.ProductFailureItems.FirstOrDefault()?.ProductFailure?.Id ?? 0, // یا مقدار صحیح حساب
    //            Items = failure.ProductFailureItems.Select(i => new ProductFailureItemEditDto
    //            {
    //                Id = i.Id,
    //                Barcode = i.Barcode,
    //                ProductName = i.Name,
    //                UnitName = i.Unit,
    //                StoreroomName = i.Storeroom_Product?.Name ?? string.Empty,
    //                Quantity = i.Value,
    //                Description = i.Description
    //            }).ToList()
    //        };
    //    }


    //    //******CRUD*****
    //    private async Task HandleInventoryAsync(BusinessEntity.Product.Product product, int value, bool forceNegative, CancellationToken cancellationToken = default)
    //    {
    //        var storeroom = await _context.Storeroom_Product
    //            .FirstOrDefaultAsync(s => s.Id == product.StoreroomProductId, cancellationToken)
    //            ?? throw new Exception("انبار کالا پیدا نشد");

    //        int newInventory = product.Inventory - value;

    //        switch (storeroom.NegativeBalancePolicy)
    //        {
    //            case NegativeBalancePolicy.Yes:
    //                product.Inventory = newInventory;
    //                return;

    //            case NegativeBalancePolicy.No:
    //                if (newInventory < 0)
    //                    throw new Exception($"موجودی کالا '{product.Name}' کافی نیست.");
    //                product.Inventory = newInventory;
    //                return;

    //            case NegativeBalancePolicy.Message:
    //                if (newInventory < 0 && !forceNegative)
    //                {
    //                    // پیام نیاز به تایید برمی‌گردانیم (UI باید آن را گیر بیاندازد و دوباره با ForceNegative=true ارسال کند)
    //                    throw new InventoryConfirmRequiredException(
    //                        $"موجودی کالا '{product.Name}' ({product.Inventory}) کمتر از مقدار درخواستی ({value}) است. آیا مایلید موجودی منفی شود؟",
    //                        product.Id,
    //                        product.Name,
    //                        value,
    //                        product.Inventory
    //                    );
    //                }
    //                product.Inventory = newInventory;
    //                return;

    //            default:
    //                throw new Exception("سیاست انبار نامعتبر است");
    //        }
    //    }
    //    private decimal CalculateItemAmount(BusinessEntity.Product.Product product, int value)
    //    {
    //        // اگر نیاز داری قیمت را از Prices یا UnitLevel بگیری، اینجا تغییر بده
    //        return product.BuyPrice * value;
    //    }
    //    private async Task AddMoneyToAccountAsync(int accountId, decimal amount, string description, int failureId, CancellationToken cancellationToken = default)
    //    {
    //        var account = await _context.Account
    //            .FirstOrDefaultAsync(a => a.AccountId == accountId, cancellationToken)
    //            ?? throw new Exception("حساب انتخاب شده یافت نشد.");

    //        // موجودی حساب را افزایش می‌دهیم
    //        account.Balance += amount;

    //        // ثبت تراکنش
    //        var transaction = new BusinessEntity.Financial_Operations.Transaction
    //        {
    //            AccountId = account.AccountId,
    //            Amount = (long)amount, // اگر در مدل تراکنش long است، تبدیل کن؛ یا مدل را decimal کن
    //            Type = "Increase",
    //            Description = description,
    //            RelatedDocumentType = "ProductFailure",
    //            RelatedDocumentId = failureId,
    //            PaymentMethod = account.AccountType == "Bank" ? "BankTransfer" : "Cash",
    //            Date = DateTime.Now
    //        };

    //        _context.Transaction.Add(transaction);
    //        _context.Account.Update(account);
    //    }
    //    public async Task<int> SubmitFailureAsync(FailureSubmitModel model, int userId, CancellationToken cancellationToken = default)
    //    {
    //        // شروع تراکنش DB
    //        await using var tx = await _context.Database.BeginTransactionAsync(cancellationToken);

    //        try
    //        {
    //            var failure = new Product_Failure
    //            {
    //                Date = model.Date,
    //                Description = model.Description,
    //                ProductFailureItems = new List<Product_Failure_Item>()
    //            };

    //            decimal totalAmount = 0m;

    //            foreach (var item in model.Items)
    //            {
    //                // پیدا کردن Product از روی بارکد
    //                var barcodeEntity = await _context.ProductBarcodes
    //                    .Include(b => b.ProductUnit)
    //                        .ThenInclude(ul => ul.Product)
    //                    .Include(b => b.ProductUnit)
    //                        .ThenInclude(ul => ul.UnitProduct)
    //                    .FirstOrDefaultAsync(b => b.Barcode == item.Barcode, cancellationToken);

    //                if (barcodeEntity == null)
    //                    throw new Exception($"بارکد '{item.Barcode}' پیدا نشد.");

    //                var product = barcodeEntity.ProductUnit.Product;

    //                if (product == null)
    //                    throw new Exception($"کالای مرتبط با بارکد '{item.Barcode}' یافت نشد.");

    //                // کاهش موجودی بر اساس سیاست انبار
    //                await HandleInventoryAsync(product, item.Value, model.ForceNegative, cancellationToken);
    //                _context.Product.Update(product);

    //                // افزودن آیتم مرجوعی
    //                var failureItem = new Product_Failure_Item
    //                {
    //                    Barcode = item.Barcode,
    //                    Name = product.Name,
    //                    Unit = barcodeEntity.ProductUnit.UnitProduct?.Name ?? string.Empty,
    //                    StoreroomId = product.StoreroomProductId,
    //                    Value = item.Value,
    //                    Description = item.Description,
    //                    ProductFailure = failure
    //                };
    //                failure.ProductFailureItems.Add(failureItem);

    //                // محاسبه مبلغ کل
    //                totalAmount += CalculateItemAmount(product, item.Value);
    //            }

    //            // ذخیره هدر + آیتم‌ها
    //            _context.Product_Failure.Add(failure);
    //            await _context.SaveChangesAsync(cancellationToken); // حالا failure.Id مقدار گرفته

    //            // افزایش موجودی حساب و ثبت تراکنش
    //            await AddMoneyToAccountAsync(model.AccountId, totalAmount, $"مرجوعی کالا شماره {failure.Id}", failure.Id, cancellationToken);

    //            // ثبت لاگ کاربر
    //            var log = new BusinessEntity.Settings.LogUser
    //            {
    //                Description = $"ثبت مرجوعی کالا شماره {failure.Id} با {model.Items.Count} قلم کالا. مجموع مبلغ: {totalAmount} ریال",
    //                UserId = userId,
    //                Date = DateTime.UtcNow
    //            };
    //            _context.LogUser.Add(log);
    //            await _context.SaveChangesAsync(cancellationToken);

    //            // Commit تراکنش
    //            await tx.CommitAsync(cancellationToken);

    //            return failure.Id;
    //        }
    //        catch
    //        {
    //            await tx.RollbackAsync(cancellationToken);
    //            throw;
    //        }
    //    }

    //}
}
