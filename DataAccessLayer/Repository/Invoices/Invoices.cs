using BusinessEntity.Financial_Operations;
using BusinessEntity.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Invoices
{
    public class InvoicesRepository : Interface.Invoices.IInvoicesRepository
    {
        private readonly Database _context;
        private readonly ILogger<InvoicesRepository> _logger;

        public InvoicesRepository(Database context, ILogger<InvoicesRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public static class PersianDateHelper
    {
        private static readonly PersianCalendar _pc = new PersianCalendar();

        public static string ToPersianDate(DateTime dt)
        {
            int y = _pc.GetYear(dt);
            int m = _pc.GetMonth(dt);
            int d = _pc.GetDayOfMonth(dt);
            return $"{y:0000}/{m:00}/{d:00}";
        }

        public static DateTime ParsePersian(string date)
        {
            var spl = date.Split('/', '-', '.');
            int y = int.Parse(spl[0]);
            int m = int.Parse(spl[1]);
            int d = int.Parse(spl[2]);
            return _pc.ToDateTime(y, m, d, 0, 0, 0, 0);
        }
}
        //******READ*****
        public async Task<List<ProductSalesDto>> GetProductsDailySales(
         DateTime startDate,
         DateTime endDate,
         int? productId = null)   // اگر null باشد → همه محصولات
        {
            var query = _context.Invoices_Item
                .Where(x =>
                    x.Invoices.Date >= startDate &&
                    x.Invoices.Date <= endDate &&
                    x.Invoices.TypeInvoices == BusinessEntity.Invoices.Type_Invices.Sales_Invoice);

            // اگر یک محصول خاص بخواهی
            if (productId.HasValue)
                query = query.Where(x => x.ProductId == productId.Value);

            // مرحله ۱: گروه‌بندی در دیتابیس (ProductId + Date)
            var grouped = await query
                .GroupBy(x => new
                {
                    x.ProductId,
                    ProductName = x.Product.Name,
                    Date = x.Invoices.Date.Date
                })
                .Select(g => new
                {
                    g.Key.ProductId,
                    g.Key.ProductName,
                    g.Key.Date,
                    Qty = g.Sum(i => i.Number)
                })
                .ToListAsync();

            // مرحله ۲: تبدیل به خروجی گروه‌بندی‌شده
            var result = grouped
                .GroupBy(x => new { x.ProductId, x.ProductName })
                .Select(g => new ProductSalesDto
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    Sales = g.OrderBy(x => x.Date)
                             .Select(x => new SalePerDayDto
                             {
                                 Date = PersianDateHelper.ToPersianDate(x.Date),
                                 Qty = x.Qty
                             })
                             .ToList()
                })
                .OrderBy(x => x.ProductName)
                .ToList();

            return result;
        }
        //******CRUD*****
        public async Task<string> AddInvoice(BusinessEntity.Invoices.Invoices invoice)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // تاریخ رو به UTC تبدیل می‌کنیم
                invoice.Date = DateTime.SpecifyKind(invoice.Date, DateTimeKind.Utc);

                // Navigation Property ها رو null می‌کنیم
                invoice.People = null;
                invoice.User = null;
                foreach (var item in invoice.Invoices_Item)
                {
                    item.Product = null;
                    item.Invoices = null;
                }

                _context.Set<BusinessEntity.Invoices.Invoices>().Add(invoice);

                foreach (var item in invoice.Invoices_Item)
                {
                    var product = await _context.Set<BusinessEntity.Product.Product>()
                        .FirstOrDefaultAsync(p => p.Id == item.ProductId);

                    if (product == null)
                        return $"❌ محصول با شناسه {item.ProductId} یافت نشد.";

                    switch (invoice.TypeInvoices)
                    {
                        case BusinessEntity.Invoices.Type_Invices.Sales_Invoice:
                        case BusinessEntity.Invoices.Type_Invices.Purchase_Return_Invoice:
                            if (product.Inventory < item.Number)
                                return $"❌ موجودی محصول '{product.Name}' کافی نیست. موجودی فعلی: {product.Inventory}, تعداد مورد نیاز: {item.Number}";
                            product.Inventory -= item.Number;
                            break;

                        case BusinessEntity.Invoices.Type_Invices.Purchase_Invoice:
                        case BusinessEntity.Invoices.Type_Invices.Sales_Return_Invoice:
                            product.Inventory += item.Number;
                            break;
                    }

                    _context.Update(product);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return $"✅ فاکتور با شماره '{invoice.InvoiceNumber}' با موفقیت ثبت شد.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return $"❌ خطا در ثبت فاکتور: {errorMessage}";
            }
        }

    }
}
