using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Interface.Invoices;
using DataAccessLayer.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repository.Invoices
{
    public class PurchaseInvoiceService : IPurchaseInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PurchaseInvoiceService> _logger;

        public PurchaseInvoiceService(
            IUnitOfWork unitOfWork,
            ILogger<PurchaseInvoiceService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<PurchaseInvoiceDto>> CreatePurchaseInvoiceAsync(PurchaseInvoiceCreateDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // 1. اعتبارسنجی تأمین‌کننده
                var supplier = await _unitOfWork.People.GetByIdAsync(dto.PeopleId);
                if (supplier == null)
                    return Result<PurchaseInvoiceDto>.Failure("تأمین‌کننده یافت نشد");

                // 2. ایجاد فاکتور خرید
                var invoice = new BusinessEntity.Invoices.Invoices
                {
                    InvoiceNumber = GenerateInvoiceNumber(),
                    Date = dto.Date,
                    PeopleId = dto.PeopleId,
                    CustomerId = dto.CustomerId,
                    UserId = dto.UserId,
                    TyepPay = dto.TypePay,
                    TypeInvoices = BusinessEntity.Invoices.Type_Invices.Purchase_Invoice,  // نوع فاکتور خرید
                    IsUpdate = false,
                    TotalSum = 0,
                    OffAll = 0,
                    ClubDiscountAll = 0,
                    UsedWalletAmount = 0,
                    LevelDiscountAmount = 0,
                    IsClubDiscountRefunded = false
                };

                await _unitOfWork.Invoices.AddAsync(invoice);
                await _unitOfWork.SaveChangesAsync();

                int totalAmount = 0;

                // 3. پردازش آیتم‌ها و افزایش موجودی + به‌روزرسانی قیمت‌ها
                foreach (var itemDto in dto.Items)
                {
                    var barcodeEntity = await _unitOfWork.ProductBarcodes.GetByBarcodeAsync(itemDto.Barcode);
                    if (barcodeEntity == null)
                        continue;

                    var unitLevel = await _unitOfWork.UnitsLevels.GetByIdAsync(barcodeEntity.ProductUnitId);
                    if (unitLevel == null)
                        continue;

                    var product = await _unitOfWork.Products.GetByIdAsync(unitLevel.ProductId);
                    if (product == null)
                        continue;

                    // افزایش موجودی
                    product.Inventory += itemDto.Quantity;

                    // به‌روزرسانی قیمت خرید و فروش
                    product.BuyPrice = itemDto.BuyPrice;
                    product.SalePrice = itemDto.SalePrice;

                    _unitOfWork.Products.Update(product);

                    // محاسبه مبلغ کل آیتم (بر اساس قیمت خرید)
                    int itemTotal = itemDto.BuyPrice * itemDto.Quantity;
                    totalAmount += itemTotal;

                    // ایجاد آیتم فاکتور
                    var invoiceItem = new BusinessEntity.Invoices.Invoices_Item
                    {
                        InvoicesId = invoice.Id,
                        ProductId = product.Id,
                        Barcode = itemDto.Barcode,
                        Name = $"{product.Name} ({unitLevel.Title})",
                        Number = itemDto.Quantity,
                        Price = itemDto.BuyPrice,      // قیمت خرید در فاکتور خرید
                        OFF = 0,
                        AllPrice = itemTotal,
                        OriginalPrice = itemTotal,
                        ClubDiscount = 0,
                        PublicDiscountId = null,
                        ClubDiscountId = null
                    };

                    await _unitOfWork.InvoiceItems.AddAsync(invoiceItem);
                }

                // بروزرسانی مبلغ کل فاکتور
                invoice.NumberofAllItems = dto.Items.Sum(i => i.Quantity);
                invoice.TotalSum = totalAmount;
                _unitOfWork.Invoices.Update(invoice);

                // 4. پردازش پرداخت‌های ترکیبی (مشابه فروش اما با علامت معکوس برای نقد/بانک)
                if (dto.Payments != null && dto.Payments.Any())
                {
                    int totalPayments = dto.Payments.Sum(p => p.Amount);
                    if (totalPayments != totalAmount)
                        return Result<PurchaseInvoiceDto>.Failure("مجموع مبالغ پرداختی با مبلغ فاکتور همخوانی ندارد.");

                    foreach (var payment in dto.Payments)
                    {
                        switch (payment.PaymentType)
                        {
                            case BusinessEntity.Invoices.Type_Pay.Cash:
                                if (!payment.FundId.HasValue)
                                    return Result<PurchaseInvoiceDto>.Failure("شناسه صندوق برای پرداخت نقدی الزامی است.");
                                var fund = await _unitOfWork.Funds.GetByIdAsync(payment.FundId.Value);
                                if (fund == null)
                                    return Result<PurchaseInvoiceDto>.Failure("صندوق انتخابی یافت نشد.");
                                // برای خرید، موجودی صندوق کاهش می‌یابد
                                if (fund.Inventory < payment.Amount)
                                    return Result<PurchaseInvoiceDto>.Failure("موجودی صندوق کافی نیست.");
                                fund.Inventory -= payment.Amount;
                                _unitOfWork.Funds.Update(fund);
                                await CreateTransaction(fund.AccountId, payment.Amount, "Decrease",
                                    $"پرداخت خرید (نقد) فاکتور {invoice.InvoiceNumber}");
                                break;

                            case BusinessEntity.Invoices.Type_Pay.Bank:
                                if (!payment.BankAccountId.HasValue)
                                    return Result<PurchaseInvoiceDto>.Failure("شناسه حساب بانکی برای پرداخت بانکی الزامی است.");
                                var bankAccount = await _unitOfWork.BankAccounts.GetByIdAsync(payment.BankAccountId.Value);
                                if (bankAccount == null)
                                    return Result<PurchaseInvoiceDto>.Failure("حساب بانکی انتخابی یافت نشد.");
                                if (bankAccount.Inventory < payment.Amount)
                                    return Result<PurchaseInvoiceDto>.Failure("موجودی حساب بانکی کافی نیست.");
                                bankAccount.Inventory -= payment.Amount;
                                _unitOfWork.BankAccounts.Update(bankAccount);
                                await CreateTransaction(bankAccount.AccountId, payment.Amount, "Decrease",
                                    $"پرداخت خرید (بانکی) فاکتور {invoice.InvoiceNumber}");
                                break;

                            case BusinessEntity.Invoices.Type_Pay.Credit:
                                // پرداخت اعتباری = افزایش بدهی به تأمین‌کننده
                                // (فرض می‌کنیم Inventory تأمین‌کننده میزان بدهی ما به اوست)
                                supplier.Inventory += payment.Amount;
                                _unitOfWork.People.Update(supplier);
                                break;

                         
                            default:
                                return Result<PurchaseInvoiceDto>.Failure("نوع پرداخت نامعتبر است.");
                        }
                    }
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                var invoiceDto = await GetPurchaseInvoiceByIdAsync(invoice.Id);
                return Result<PurchaseInvoiceDto>.Success(invoiceDto.Data, "فاکتور خرید با موفقیت ایجاد شد");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "خطا در ایجاد فاکتور خرید");
                return Result<PurchaseInvoiceDto>.Failure($"خطا در ایجاد فاکتور خرید: {ex.Message}");
            }
        }

        public async Task<Result<PurchaseInvoiceDto>> GetPurchaseInvoiceByIdAsync(int id)
        {
            var invoice = await _unitOfWork.Invoices.GetByIdAsync(id,
                default,
                i => i.People,
                i => i.Customer,
                i => i.CustomerLevel);

            if (invoice == null || invoice.TypeInvoices != BusinessEntity.Invoices.Type_Invices.Purchase_Invoice)
                return Result<PurchaseInvoiceDto>.Failure("فاکتور خرید یافت نشد");

            var items = await _unitOfWork.InvoiceItems.FindAsync(
                ii => ii.InvoicesId == id,
                default,
                ii => ii.Product);

            var dto = MapToDto(invoice, items);
            return Result<PurchaseInvoiceDto>.Success(dto);
        }

        private string GenerateInvoiceNumber()
        {
            return $"PUR-{DateTime.UtcNow:yyyyMMddHHmmss}-{new Random().Next(1000, 9999)}";
        }

        private async Task CreateTransaction(int accountId, long amount, string type, string description)
        {
            var transaction = new BusinessEntity.Invoices.Transaction
            {
                AccountId = accountId,
                Amount = amount,
                Type = type,
                Description = description,
                Date = DateTime.UtcNow
            };
            await _unitOfWork.Transactions.AddAsync(transaction);
        }

        private PurchaseInvoiceDto MapToDto(BusinessEntity.Invoices.Invoices invoice, IEnumerable<BusinessEntity.Invoices.Invoices_Item> items)
        {
            return new PurchaseInvoiceDto
            {
                Id = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber,
                Date = invoice.Date,
                PeopleId = invoice.PeopleId,
                PeopleName = invoice.People?.FirstName + " " + invoice.People?.LastName,
                CustomerId = invoice.CustomerId,
                CustomerName = invoice.Customer?.FirstName + " " + invoice.Customer?.LastName,
                TotalAmount = invoice.TotalSum,
                Items = items.Select(i => new PurchaseInvoiceItemDto
                {
                    Barcode = i.Barcode,
                    ProductId = i.ProductId,
                    ProductName = i.Product?.Name ?? i.Name,
                    UnitName = "",
                    Quantity = i.Number,
                    BuyPrice = i.Price,
                    SalePrice = (int)(i.Product?.SalePrice ?? 0), // یا از جای دیگری
                    TotalPrice = i.AllPrice
                }).ToList()
            };
        }
    }
}
