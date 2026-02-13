using BusinessEntity.Customer_Club;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Interface.Customer_Club;
using BusinessLogicLayer.Interface.Invoices;
using BusinessLogicLayer.Repository.Fund;
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

        public InvoiceService(
            IUnitOfWork unitOfWork,
            IPublicDiscountService publicDiscountService,
            IClubDiscountService clubDiscountService,
            IWalletService walletService,
            ICustomerService customerService)
        {
            _unitOfWork = unitOfWork;
            _publicDiscountService = publicDiscountService;
            _clubDiscountService = clubDiscountService;
            _walletService = walletService;
            _customerService = customerService;
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

                // 3. پردازش آیتم‌ها
                foreach (var itemDto in dto.Items)
                {
                    // پیدا کردن واحد کالا با بارکد
                    var barcodeEntity = await _unitOfWork.ProductBarcodes
                        .GetByBarcodeAsync(itemDto.Barcode);
                    if (barcodeEntity == null)
                        continue;

                    var unitLevel = await _unitOfWork.UnitsLevels
                        .GetByIdAsync(barcodeEntity.UnitLevelId);
                    if (unitLevel == null)
                        continue;

                    var product = await _unitOfWork.Products
                        .GetByIdAsync(unitLevel.ProductId);
                    if (product == null)
                        continue;

                    // محاسبه قیمت واحد بر اساس سطح قیمتی خریدار
                    int unitPrice = 0;
                    var price = unitLevel.Prices
                        .FirstOrDefault(p => p.PriceLevelId == people.PriceLevelID);
                    if (price != null)
                    {
                        unitPrice = (int)price.SalePrice;
                    }
                    else
                    {
                        unitPrice = (int)product.SalePrice;
                    }

                    int originalTotal = unitPrice * itemDto.Quantity;
                    totalOriginalPrice += originalTotal;

                    // محاسبه تخفیف عمومی
                    int publicDiscountAmount = 0;
                    if (dto.ApplyPublicDiscount)
                    {
                        var publicResult = await _publicDiscountService
                            .CalculatePublicDiscountAsync(itemDto.Barcode, dto.Date, dto.StoreId);
                        if (publicResult.Success)
                        {
                            publicDiscountAmount = publicResult.Data.DiscountAmount * itemDto.Quantity;
                        }
                    }

                    // محاسبه تخفیف باشگاه (فقط در صورت وجود مشتری باشگاه)
                    int clubDiscountAmount = 0;
                    if (customer != null && dto.ApplyClubDiscount)
                    {
                        var clubResult = await _clubDiscountService
                            .CalculateClubDiscountAsync(itemDto.Barcode, customer.Id, originalTotal - publicDiscountAmount);
                        if (clubResult.Success)
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
                        PublicDiscountId = null, // در صورت نیاز ذخیره شود
                        ClubDiscountId = appliedClubDiscountId
                    };

                    await _unitOfWork.InvoiceItems.AddAsync(invoiceItem);

                    totalPublicDiscount += publicDiscountAmount;
                    totalClubDiscount += clubDiscountAmount;
                }

                // 4. اعمال تخفیف سطح مشتری (روی مبلغ نهایی پس از تخفیف‌ها)
                if (customer != null)
                {
                    var levelResult = await _customerService.GetCustomerCurrentLevelAsync(customer.Id);
                    if (levelResult.Success && levelResult.Data != null)
                    {
                        var amountBeforeLevel = totalOriginalPrice - totalPublicDiscount - totalClubDiscount;
                        totalLevelDiscount = (int)(amountBeforeLevel * levelResult.Data.DiscountPercent / 100);
                    }
                }

                int finalInvoiceAmount = totalOriginalPrice - totalPublicDiscount - totalClubDiscount - totalLevelDiscount;

                // 5. بروزرسانی فاکتور
                invoice.NumberofAllItems = dto.Items.Sum(i => i.Quantity);
                invoice.OffAll = totalPublicDiscount;
                invoice.ClubDiscountAll = totalClubDiscount;
                invoice.LevelDiscountAmount = totalLevelDiscount;
                invoice.TotalSum = finalInvoiceAmount;

                _unitOfWork.Invoices.Update(invoice);

                // 6. استفاده از کیف پول (در صورت درخواست)
                if (dto.UsedWalletAmount > 0 && customer != null)
                {
                    var useWalletResult = await UseWalletForPaymentAsync(invoice.Id, customer.Id, dto.UsedWalletAmount);
                    if (!useWalletResult.Success)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return Result<InvoiceDto>.Failure(useWalletResult.Message);
                    }
                    invoice.UsedWalletAmount = dto.UsedWalletAmount;
                    invoice.TotalSum -= dto.UsedWalletAmount;
                }

                // 7. برگشت تخفیف باشگاه به کیف پول
                if (totalClubDiscount > 0 && dto.RefundClubDiscountToWallet && customer != null && appliedClubDiscountId != null)
                {
                    var refundResult = await _walletService.RefundClubDiscountAsync(
                        customer.Id,
                        totalClubDiscount,
                        $"بازگشت تخفیف باشگاه برای فاکتور {invoice.InvoiceNumber}",
                        appliedClubDiscountId.Value,
                        invoice.Id);

                    if (refundResult.Success)
                    {
                        invoice.IsClubDiscountRefunded = true;
                    }
                }

                // 8. محاسبه و اعمال امتیاز
                if (customer != null && finalInvoiceAmount > 0)
                {
                    int earnedPoints = finalInvoiceAmount / 10000; // هر 10,000 ریال = 1 امتیاز
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

                // 9. بروزرسانی آمار خرید مشتری
                if (customer != null)
                {
                    customer.TotalPurchaseAmount += finalInvoiceAmount;
                    customer.TotalPurchaseCount += 1;
                    customer.LastPurchaseDate = dto.Date;
                    _unitOfWork.Customers.Update(customer);
                }

                // 10. بروزرسانی آمار خرید شخص (People)
                people.Inventory += finalInvoiceAmount; // یا هر منطق دیگری
                _unitOfWork.People.Update(people);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                var invoiceDto = await GetInvoiceByIdAsync(invoice.Id);
                return Result<InvoiceDto>.SuccessResult(invoiceDto.Data, "فاکتور با موفقیت ایجاد شد");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
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

            if (!withdrawResult.Success)
                return withdrawResult;

            invoice.UsedWalletAmount = walletAmount;
            invoice.TotalSum -= walletAmount;
            _unitOfWork.Invoices.Update(invoice);
            await _unitOfWork.SaveChangesAsync();

            return Result.SuccessResult();
        }

        public async Task<Result<InvoiceDto>> GetInvoiceByIdAsync(int id)
        {
            var invoice = await _unitOfWork.Invoices.GetByIdAsync(id,
                i => i.People,
                i => i.Customer,
                i => i.CustomerLevel);

            if (invoice == null)
                return Result<InvoiceDto>.Failure("فاکتور یافت نشد");

            var items = await _unitOfWork.InvoiceItems
                .FindAsync(ii => ii.InvoicesId == id, ii => ii.Product);

            var walletTransactions = await _unitOfWork.WalletTransactions
                .FindAsync(wt => wt.InvoiceId == id, wt => wt.Wallet, wt => wt.Invoice);

            var pointTransactions = await _unitOfWork.PointTransactions
                .FindAsync(pt => pt.InvoiceId == id);

            var dto = MapToDto(invoice, items, walletTransactions, pointTransactions);
            return Result<InvoiceDto>.SuccessResult(dto);
        }

        private string GenerateInvoiceNumber()
        {
            return $"INV-{DateTime.Now:yyyyMMddHHmmss}-{new Random().Next(1000, 9999)}";
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
                CustomerName = invoice.Customer != null
                    ? invoice.Customer.FirstName + " " + invoice.Customer.LastName
                    : null,
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
                    UnitName = "", // می‌توانید از واحد کالا استخراج کنید
                    Quantity = i.Number,
                    UnitPrice = i.Price,
                    OriginalPrice = i.OriginalPrice,
                    PublicDiscount = 0, // در صورت ذخیره‌سازی مقدار دهی شود
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
    //public class InvoicesService : Interface.Invoices.IInvoicesService
    //{
    //    private readonly DataAccessLayer.Interface.Invoices.IInvoicesRepository _InvoicesRepository;
    //    private readonly ILogger<InvoicesService> _logger;

    //    public InvoicesService(DataAccessLayer.Interface.Invoices.IInvoicesRepository invoicesRepository, ILogger<InvoicesService> logger)
    //    {
    //        _InvoicesRepository = invoicesRepository;
    //        _logger = logger;
    //    }

    //    public async Task<string> Create(BusinessEntity.Invoices.Invoices invoice)
    //    {
    //        _logger.LogInformation("Request to add new Invoice: {@Invoice}", invoice);

    //        var message = await _InvoicesRepository.AddInvoice(invoice);

    //        _logger.LogInformation("Add result: {Message}", message);
    //        return message;
    //    }

    //}
}
