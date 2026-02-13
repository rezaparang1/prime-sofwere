using BusinessEntity.Customer_Club;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Interface.Customer_Club;
using BusinessLogicLayer.Interface.Invoices;
using BusinessLogicLayer.Repository.Fund;
using DataAccessLayer.Interface;
using DataAccessLayer.Interface.Customer_Club;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repository.Invoices
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublicDiscountService _publicDiscountService;
        private readonly IClubDiscountService _clubDiscountService;
        private readonly IWalletService _walletService;
        private readonly ICustomerService _customerService;
        private readonly ILogger<InvoiceService> _logger;

        public InvoiceService(
            IUnitOfWork unitOfWork,
            IPublicDiscountService publicDiscountService,
            IClubDiscountService clubDiscountService,
            IWalletService walletService,
            ICustomerService customerService,
            ILogger<InvoiceService> logger)
        {
            _unitOfWork = unitOfWork;
            _publicDiscountService = publicDiscountService;
            _clubDiscountService = clubDiscountService;
            _walletService = walletService;
            _customerService = customerService;
            _logger = logger;
        }

        public async Task<Result<InvoiceDto>> CreateInvoiceWithAllDiscountsAsync(InvoiceCreateDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // 1. اعتبارسنجی اولیه
                var people = await _unitOfWork.People.GetByIdAsync(dto.PeopleId);
                if (people == null)
                    return Result<InvoiceDto>.Failure("خریدار یافت نشد");

                Customer? customer = null;
                if (dto.CustomerId.HasValue)
                {
                    customer = await _unitOfWork.Customers.GetByIdAsync(dto.CustomerId.Value);
                    if (customer == null)
                        return Result<InvoiceDto>.Failure("عضو باشگاه یافت نشد");
                }

                // 2. ایجاد فاکتور
                var invoice = new BusinessEntity.Invoices.Invoices
                {
                    InvoiceNumber = GenerateInvoiceNumber(),
                    Date = dto.Date,
                    PeopleId = dto.PeopleId,
                    CustomerId = dto.CustomerId,
                    UserId = dto.UserId,
                    TyepPay = dto.TypePay,
                    TypeInvoices = BusinessEntity.Invoices.Type_Invices.Sales_Invoice,
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

                int totalOriginalPrice = 0;
                int totalPublicDiscount = 0;
                int totalClubDiscount = 0;
                int totalLevelDiscount = 0;
                int? appliedClubDiscountId = null;

                // 3. پردازش آیتم‌ها و کسر موجودی
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

                    // کسر موجودی با رعایت سیاست انبار
                    var storeroom = await _unitOfWork.StoreroomProducts.GetByIdAsync(product.StoreroomProductId);
                    if (storeroom == null)
                        return Result<InvoiceDto>.Failure($"انبار محصول '{product.Name}' یافت نشد.");

                    int newInventory = product.Inventory - itemDto.Quantity;
                    switch (storeroom.NegativeBalancePolicy)
                    {
                        case BusinessEntity.Product.NegativeBalancePolicy.Yes:
                            product.Inventory = newInventory;
                            break;
                        case BusinessEntity.Product.NegativeBalancePolicy.No:
                            if (newInventory < 0)
                                return Result<InvoiceDto>.Failure($"موجودی کالای '{product.Name}' کافی نیست.");
                            product.Inventory = newInventory;
                            break;
                        case BusinessEntity.Product.NegativeBalancePolicy.Message:
                            product.Inventory = newInventory;
                            break;
                        default:
                            return Result<InvoiceDto>.Failure("سیاست انبار نامعتبر است.");
                    }
                    _unitOfWork.Products.Update(product);

                    // قیمت واحد بر اساس سطح قیمتی خریدار
                    int unitPrice = 0;
                    var price = unitLevel.Prices.FirstOrDefault(p => p.PriceLevelId == people.PriceLevelID);
                    unitPrice = price != null ? (int)price.SalePrice : (int)product.SalePrice;

                    int originalTotal = unitPrice * itemDto.Quantity;
                    totalOriginalPrice += originalTotal;

                    // تخفیف عمومی
                    int publicDiscountAmount = 0;
                    if (dto.ApplyPublicDiscount)
                    {
                        var publicResult = await _publicDiscountService.CalculatePublicDiscountAsync(
                            itemDto.Barcode, dto.Date, dto.StoreId);
                        if (publicResult.IsSuccess)
                            publicDiscountAmount = publicResult.Data.DiscountAmount * itemDto.Quantity;
                    }

                    // تخفیف باشگاه
                    int clubDiscountAmount = 0;
                    if (customer != null && dto.ApplyClubDiscount)
                    {
                        var clubResult = await _clubDiscountService.CalculateClubDiscountAsync(
                            itemDto.Barcode, customer.Id, originalTotal - publicDiscountAmount);
                        if (clubResult.IsSuccess)
                        {
                            clubDiscountAmount = clubResult.Data.DiscountAmount;
                            appliedClubDiscountId = clubResult.Data.DiscountId;
                        }
                    }

                    int finalPrice = originalTotal - publicDiscountAmount - clubDiscountAmount;

                    var invoiceItem = new BusinessEntity.Invoices.Invoices_Item
                    {
                        InvoicesId = invoice.Id,
                        ProductId = product.Id,
                        Barcode = itemDto.Barcode,
                        Name = $"{product.Name} ({unitLevel.Title})",
                        Number = itemDto.Quantity,
                        Price = unitPrice,
                        OFF = 0,
                        AllPrice = finalPrice,
                        OriginalPrice = originalTotal,
                        ClubDiscount = clubDiscountAmount,
                        PublicDiscountId = null,
                        ClubDiscountId = appliedClubDiscountId
                    };

                    await _unitOfWork.InvoiceItems.AddAsync(invoiceItem);

                    totalPublicDiscount += publicDiscountAmount;
                    totalClubDiscount += clubDiscountAmount;
                }

                // 4. تخفیف سطح مشتری
                if (customer != null)
                {
                    var levelResult = await _customerService.GetCustomerCurrentLevelAsync(customer.Id);
                    if (levelResult.IsSuccess && levelResult.Data != null)
                    {
                        var amountBeforeLevel = totalOriginalPrice - totalPublicDiscount - totalClubDiscount;
                        totalLevelDiscount = (int)(amountBeforeLevel * levelResult.Data.DiscountPercent / 100);
                    }
                }

                int finalInvoiceAmount = totalOriginalPrice - totalPublicDiscount - totalClubDiscount - totalLevelDiscount;

                // بروزرسانی فاکتور
                invoice.NumberofAllItems = dto.Items.Sum(i => i.Quantity);
                invoice.OffAll = totalPublicDiscount;
                invoice.ClubDiscountAll = totalClubDiscount;
                invoice.LevelDiscountAmount = totalLevelDiscount;
                invoice.TotalSum = finalInvoiceAmount;
                _unitOfWork.Invoices.Update(invoice);

                // 5. استفاده از کیف پول
                if (dto.UsedWalletAmount > 0 && customer != null)
                {
                    var useWalletResult = await UseWalletForPaymentAsync(invoice.Id, customer.Id, dto.UsedWalletAmount);
                    if (!useWalletResult.IsSuccess)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return Result<InvoiceDto>.Failure(useWalletResult.Message);
                    }
                    invoice.UsedWalletAmount = dto.UsedWalletAmount;
                    invoice.TotalSum -= dto.UsedWalletAmount;
                }

                // 6. بازگشت تخفیف باشگاه به کیف پول
                if (totalClubDiscount > 0 && dto.RefundClubDiscountToWallet && customer != null && appliedClubDiscountId != null)
                {
                    var refundResult = await _walletService.RefundClubDiscountAsync(
                        customer.Id,
                        totalClubDiscount,
                        $"بازگشت تخفیف باشگاه برای فاکتور {invoice.InvoiceNumber}",
                        appliedClubDiscountId.Value,
                        invoice.Id);
                    if (refundResult.IsSuccess)
                        invoice.IsClubDiscountRefunded = true;
                }

                // 7. محاسبه و اعمال امتیاز
                if (customer != null && finalInvoiceAmount > 0)
                {
                    int earnedPoints = finalInvoiceAmount / 10000;
                    if (earnedPoints > 0)
                    {
                        await _customerService.UpdateCustomerPointsAsync(
                            customer.Id,
                            earnedPoints,
                            $"خرید از فاکتور {invoice.InvoiceNumber}",
                            invoice.Id);
                        invoice.EarnedPoints = earnedPoints;
                    }
                }

                // 8. پردازش پرداخت‌های ترکیبی
                if (dto.Payments != null && dto.Payments.Any())
                {
                    int totalPayments = dto.Payments.Sum(p => p.Amount);
                    if (totalPayments != invoice.TotalSum)
                        return Result<InvoiceDto>.Failure("مجموع مبالغ پرداختی با مبلغ فاکتور همخوانی ندارد.");

                    foreach (var payment in dto.Payments)
                    {
                        switch (payment.PaymentType)
                        {
                            case BusinessEntity.Invoices.Type_Pay.Cash:
                                if (!payment.FundId.HasValue)
                                    return Result<InvoiceDto>.Failure("شناسه صندوق برای پرداخت نقدی الزامی است.");
                                var fund = await _unitOfWork.Funds.GetByIdAsync(payment.FundId.Value);
                                if (fund == null)
                                    return Result<InvoiceDto>.Failure("صندوق انتخابی یافت نشد.");
                                fund.Inventory += payment.Amount;
                                _unitOfWork.Funds.Update(fund);
                                await CreateTransaction(fund.AccountId, payment.Amount, "Increase",
                                    $"پرداخت نقدی فاکتور {invoice.InvoiceNumber}");
                                break;

                            case BusinessEntity.Invoices.Type_Pay.Bank:
                                if (!payment.BankAccountId.HasValue)
                                    return Result<InvoiceDto>.Failure("شناسه حساب بانکی برای پرداخت بانکی الزامی است.");
                                var bankAccount = await _unitOfWork.BankAccounts.GetByIdAsync(payment.BankAccountId.Value);
                                if (bankAccount == null)
                                    return Result<InvoiceDto>.Failure("حساب بانکی انتخابی یافت نشد.");
                                bankAccount.Inventory += payment.Amount;
                                _unitOfWork.BankAccounts.Update(bankAccount);
                                await CreateTransaction(bankAccount.AccountId, payment.Amount, "Increase",
                                    $"پرداخت بانکی فاکتور {invoice.InvoiceNumber}");
                                break;

                            case BusinessEntity.Invoices.Type_Pay.Credit:
                                if (people.CreditLimit > 0 && people.Inventory + payment.Amount > people.CreditLimit)
                                    return Result<InvoiceDto>.Failure("سقف اعتبار مشتری نقض می‌شود.");
                                people.Inventory += payment.Amount;
                                _unitOfWork.People.Update(people);
                                break;

                            case BusinessEntity.Invoices.Type_Pay.Wallet:
                                // قبلاً پردازش شده
                                continue;

                            default:
                                return Result<InvoiceDto>.Failure("نوع پرداخت نامعتبر است.");
                        }
                    }
                }

                // 9. بروزرسانی آمار
                if (customer != null)
                {
                    customer.TotalPurchaseAmount += finalInvoiceAmount;
                    customer.TotalPurchaseCount += 1;
                    customer.LastPurchaseDate = dto.Date;
                    _unitOfWork.Customers.Update(customer);
                }
              //  people.Inventory += finalInvoiceAmount;
                _unitOfWork.People.Update(people);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                var invoiceDto = await GetInvoiceByIdAsync(invoice.Id);
                return Result<InvoiceDto>.Success(invoiceDto.Data, "فاکتور با موفقیت ایجاد شد");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "خطا در ایجاد فاکتور");
                return Result<InvoiceDto>.Failure($"خطا در ایجاد فاکتور: {ex.Message}");
            }
        }

        public async Task<Result> UseWalletForPaymentAsync(int invoiceId, int customerId, int walletAmount)
        {
            var invoice = await _unitOfWork.Invoices.GetByIdAsync(invoiceId);
            if (invoice == null)
                return Result.Failure("فاکتور یافت نشد");

            if (invoice.TotalSum < walletAmount)
                return Result.Failure("مبلغ درخواستی بیشتر از مبلغ فاکتور است");

            var withdrawResult = await _walletService.WithdrawAsync(
                customerId,
                walletAmount,
                $"پرداخت بخشی از فاکتور {invoice.InvoiceNumber}",
                invoiceId);

            if (!withdrawResult.IsSuccess)
                return withdrawResult;

            invoice.UsedWalletAmount = walletAmount;
            invoice.TotalSum -= walletAmount;
            _unitOfWork.Invoices.Update(invoice);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<InvoiceDto>> GetInvoiceByIdAsync(int id)
        {
            var invoice = await _unitOfWork.Invoices.GetByIdAsync(id,
                default,
                i => i.People,
                i => i.Customer,
                i => i.CustomerLevel);

            if (invoice == null)
                return Result<InvoiceDto>.Failure("فاکتور یافت نشد");

            var items = await _unitOfWork.InvoiceItems.FindAsync(
                ii => ii.InvoicesId == id,
                default,
                ii => ii.Product);

            var walletTransactions = await _unitOfWork.WalletTransactions.FindAsync(
                wt => wt.InvoiceId == id,
                default,
                wt => wt.Wallet,
                wt => wt.Invoice);

            var pointTransactions = await _unitOfWork.PointTransactions.FindAsync(pt => pt.InvoiceId == id);

            var dto = MapToDto(invoice, items, walletTransactions, pointTransactions);
            return Result<InvoiceDto>.Success(dto);
        }

        private string GenerateInvoiceNumber()
        {
            return $"INV-{DateTime.Now:yyyyMMddHHmmss}-{new Random().Next(1000, 9999)}";
        }

        private async Task CreateTransaction(int accountId, long amount, string type, string description)
        {
            var transaction = new BusinessEntity.Invoices.Transaction
            {
                AccountId = accountId,
                Amount = amount,
                Type = type,
                Description = description,
                Date = DateTime.Now
            };
            await _unitOfWork.Transactions.AddAsync(transaction);
        }

        private InvoiceDto MapToDto(BusinessEntity.Invoices.Invoices invoice,
            IEnumerable<BusinessEntity.Invoices.Invoices_Item> items,
            IEnumerable<WalletTransaction> walletTransactions,
            IEnumerable<PointTransaction> pointTransactions)
        {
            return new InvoiceDto
            {
                Id = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber,
                Date = invoice.Date,
                PeopleId = invoice.PeopleId,
                PeopleName = invoice.People?.FirstName + " " + invoice.People?.LastName,
                CustomerId = invoice.CustomerId,
                CustomerName = invoice.Customer?.FirstName + " " + invoice.Customer?.LastName,
                TotalOriginalPrice = items.Sum(i => i.OriginalPrice),
                TotalPublicDiscount = invoice.OffAll,
                TotalClubDiscount = invoice.ClubDiscountAll,
                LevelDiscountAmount = invoice.LevelDiscountAmount,
                UsedWalletAmount = invoice.UsedWalletAmount,
                FinalAmount = invoice.TotalSum,
                EarnedPoints = invoice.EarnedPoints,
                Items = items.Select(i => new InvoiceItemDto
                {
                    Barcode = i.Barcode,
                    ProductId = i.ProductId,
                    ProductName = i.Product?.Name ?? i.Name,
                    UnitName = "",
                    Quantity = i.Number,
                    UnitPrice = i.Price,
                    OriginalPrice = i.OriginalPrice,
                    PublicDiscount = 0,
                    ClubDiscount = i.ClubDiscount,
                    LevelDiscount = 0,
                    FinalPrice = i.AllPrice
                }).ToList(),
                WalletTransactions = walletTransactions.Select(wt => new WalletTransactionDto
                {
                    Id = wt.Id,
                    Amount = wt.Amount,
                    Type = wt.Type.ToString(),
                    TransactionDate = wt.TransactionDate,
                    Description = wt.Description,
                    InvoiceNumber = wt.Invoice?.InvoiceNumber
                }).ToList(),
                PointTransactions = pointTransactions.Select(pt => new PointTransactionDto
                {
                    Points = pt.Points,
                    Type = pt.Type.ToString(),
                    Description = pt.Description,
                    TransactionDate = pt.TransactionDate
                }).ToList()
            };
        }
    }
}
