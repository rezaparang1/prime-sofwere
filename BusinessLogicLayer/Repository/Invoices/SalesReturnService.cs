using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Interface.Customer_Club;
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
    public class SalesReturnService : ISalesReturnService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWalletService _walletService;
        private readonly ICustomerService _customerService;
        private readonly ILogger<SalesReturnService> _logger;

        public SalesReturnService(
            IUnitOfWork unitOfWork,
            IWalletService walletService,
            ICustomerService customerService,
            ILogger<SalesReturnService> logger)
        {
            _unitOfWork = unitOfWork;
            _walletService = walletService;
            _customerService = customerService;
            _logger = logger;
        }

        public async Task<Result<SalesReturnDto>> CreateSalesReturnAsync(SalesReturnCreateDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // 1. دریافت فاکتور اصلی
                var originalInvoice = await _unitOfWork.Invoices.GetByIdAsync(dto.OriginalInvoiceId,
                    default,
                    i => i.People,
                    i => i.Customer,
                    i => i.Invoices_Item); // آیتم‌ها را هم لود می‌کنیم
                if (originalInvoice == null || originalInvoice.TypeInvoices != BusinessEntity.Invoices.Type_Invices.Sales_Invoice)
                    return Result<SalesReturnDto>.Failure("فاکتور فروش اصلی یافت نشد");

                // 2. اعتبارسنجی آیتم‌های برگشتی
                var returnItems = new List<(BusinessEntity.Invoices.Invoices_Item originalItem, int returnQty, int perUnitPrice, int perUnitClubDiscount)>();
                foreach (var itemDto in dto.Items)
                {
                    // پیدا کردن آیتم مشابه در فاکتور اصلی (بر اساس بارکد)
                    var originalItem = originalInvoice.Invoices_Item.FirstOrDefault(i => i.Barcode == itemDto.Barcode);
                    if (originalItem == null)
                        return Result<SalesReturnDto>.Failure($"کالا با بارکد {itemDto.Barcode} در فاکتور اصلی یافت نشد");

                    if (itemDto.Quantity > originalItem.Number)
                        return Result<SalesReturnDto>.Failure($"تعداد برگشتی برای {originalItem.Name} بیشتر از تعداد خریداری شده است");

                    // محاسبه قیمت نهایی هر واحد (پس از تخفیف)
                    int perUnitPrice = originalItem.AllPrice / originalItem.Number;
                    int perUnitClubDiscount = originalItem.ClubDiscount / originalItem.Number;

                    returnItems.Add((originalItem, itemDto.Quantity, perUnitPrice, perUnitClubDiscount));
                }

                // 3. ایجاد فاکتور برگشت از فروش
                var returnInvoice = new BusinessEntity.Invoices.Invoices
                {
                    InvoiceNumber = GenerateInvoiceNumber(),
                    Date = dto.Date,
                    PeopleId = originalInvoice.PeopleId,      // همان خریدار
                    CustomerId = originalInvoice.CustomerId,  // همان عضو باشگاه (اگر داشته)
                    UserId = dto.UserId,
                    TyepPay = originalInvoice.TyepPay,        // شبیه فاکتور اصلی (اما در RefundPayments تصمیم می‌گیریم)
                    TypeInvoices = BusinessEntity.Invoices.Type_Invices.Sales_Return_Invoice,
                    IsUpdate = false,
                    TotalSum = 0,
                    OffAll = 0,
                    ClubDiscountAll = 0,
                    UsedWalletAmount = 0,
                    LevelDiscountAmount = 0,
                    IsClubDiscountRefunded = false
                };

                await _unitOfWork.Invoices.AddAsync(returnInvoice);
                await _unitOfWork.SaveChangesAsync();

                int totalRefundAmount = 0;
                int totalClubDiscountToReverse = 0;
                int totalPointsToDeduct = 0;
                int originalTotalAmount = originalInvoice.TotalSum; // برای محاسبه تناسب امتیاز

                // 4. پردازش آیتم‌های برگشتی
                foreach (var (originalItem, returnQty, perUnitPrice, perUnitClubDiscount) in returnItems)
                {
                    // افزایش موجودی کالا
                    var product = await _unitOfWork.Products.GetByIdAsync(originalItem.ProductId);
                    if (product == null)
                        return Result<SalesReturnDto>.Failure($"کالا با شناسه {originalItem.ProductId} یافت نشد");

                    product.Inventory += returnQty;
                    _unitOfWork.Products.Update(product);

                    // محاسبه مبلغ برگشتی برای این آیتم
                    int itemRefund = perUnitPrice * returnQty;
                    totalRefundAmount += itemRefund;

                    // محاسبه تخفیف باشگاه که باید از کیف پول کسر شود
                    int clubDiscountToReverse = perUnitClubDiscount * returnQty;
                    totalClubDiscountToReverse += clubDiscountToReverse;

                    // ایجاد آیتم فاکتور برگشتی (با مبلغ مثبت)
                    var returnItem = new BusinessEntity.Invoices.Invoices_Item
                    {
                        InvoicesId = returnInvoice.Id,
                        ProductId = originalItem.ProductId,
                        Barcode = originalItem.Barcode,
                        Name = originalItem.Name,
                        Number = returnQty,
                        Price = perUnitPrice,   // قیمت واحد همان مبلغ نهایی
                        OFF = 0,
                        AllPrice = itemRefund,
                        OriginalPrice = itemRefund,
                        ClubDiscount = 0,        // تخفیف باشگاه در آیتم جداگانه ثبت نمی‌شود
                        PublicDiscountId = null,
                        ClubDiscountId = null
                    };
                    await _unitOfWork.InvoiceItems.AddAsync(returnItem);
                }

                // 5. محاسبه امتیاز قابل کسر (به نسبت مبلغ برگشتی)
                if (originalInvoice.CustomerId.HasValue && originalInvoice.EarnedPoints > 0)
                {
                    totalPointsToDeduct = (int)((long)totalRefundAmount * originalInvoice.EarnedPoints / originalTotalAmount);
                }

                // 6. به‌روزرسانی کیف پول (کسر مبلغ تخفیف باشگاه)
                if (totalClubDiscountToReverse > 0 && originalInvoice.CustomerId.HasValue)
                {
                    // فرض می‌کنیم RefundClubDiscountAsync موجود است که مبلغ را از کیف پول کم می‌کند
                    // اگر چنین متدی نداریم، می‌توانیم از Withdraw استفاده کنیم
                    var walletResult = await _walletService.WithdrawAsync(
                        originalInvoice.CustomerId.Value,
                        totalClubDiscountToReverse,
                        $"کسر تخفیف باشگاه به دلیل برگشت از فروش فاکتور {originalInvoice.InvoiceNumber}",
                        returnInvoice.Id);
                    if (!walletResult.IsSuccess)
                        return Result<SalesReturnDto>.Failure(walletResult.Message);
                }

                // 7. کسر امتیازات
                if (totalPointsToDeduct > 0 && originalInvoice.CustomerId.HasValue)
                {
                    var pointsResult = await _customerService.UpdateCustomerPointsAsync(
                        originalInvoice.CustomerId.Value,
                        -totalPointsToDeduct,
                        $"کسر امتیاز به دلیل برگشت از فروش فاکتور {originalInvoice.InvoiceNumber}",
                        returnInvoice.Id);
                    if (!pointsResult.IsSuccess)
                        return Result<SalesReturnDto>.Failure(pointsResult.Message);
                }

                // 8. پردازش بازگشت وجه (RefundPayments)
                if (dto.RefundPayments != null && dto.RefundPayments.Any())
                {
                    int totalPayments = dto.RefundPayments.Sum(p => p.Amount);
                    if (totalPayments != totalRefundAmount)
                        return Result<SalesReturnDto>.Failure("مجموع مبالغ بازگشتی با مبلغ فاکتور همخوانی ندارد.");

                    foreach (var payment in dto.RefundPayments)
                    {
                        switch (payment.PaymentType)
                        {
                            case BusinessEntity.Invoices.Type_Pay.Cash:
                                if (!payment.FundId.HasValue)
                                    return Result<SalesReturnDto>.Failure("شناسه صندوق برای بازگشت نقدی الزامی است.");
                                var fund = await _unitOfWork.Funds.GetByIdAsync(payment.FundId.Value);
                                if (fund == null)
                                    return Result<SalesReturnDto>.Failure("صندوق انتخابی یافت نشد.");
                                if (fund.Inventory < payment.Amount)
                                    return Result<SalesReturnDto>.Failure("موجودی صندوق برای بازگشت کافی نیست.");
                                fund.Inventory -= payment.Amount;
                                _unitOfWork.Funds.Update(fund);
                                await CreateTransaction(fund.AccountId, payment.Amount, "Decrease",
                                    $"بازگشت وجه (نقد) فاکتور {returnInvoice.InvoiceNumber}");
                                break;

                            case BusinessEntity.Invoices.Type_Pay.Bank:
                                if (!payment.BankAccountId.HasValue)
                                    return Result<SalesReturnDto>.Failure("شناسه حساب بانکی برای بازگشت بانکی الزامی است.");
                                var bankAccount = await _unitOfWork.BankAccounts.GetByIdAsync(payment.BankAccountId.Value);
                                if (bankAccount == null)
                                    return Result<SalesReturnDto>.Failure("حساب بانکی انتخابی یافت نشد.");
                                if (bankAccount.Inventory < payment.Amount)
                                    return Result<SalesReturnDto>.Failure("موجودی حساب بانکی برای بازگشت کافی نیست.");
                                bankAccount.Inventory -= payment.Amount;
                                _unitOfWork.BankAccounts.Update(bankAccount);
                                await CreateTransaction(bankAccount.AccountId, payment.Amount, "Decrease",
                                    $"بازگشت وجه (بانکی) فاکتور {returnInvoice.InvoiceNumber}");
                                break;

                            case BusinessEntity.Invoices.Type_Pay.Credit:
                                // بازگشت به اعتبار = کاهش بدهی مشتری
                                var customerPeople = await _unitOfWork.People.GetByIdAsync(originalInvoice.PeopleId);
                                if (customerPeople == null)
                                    return Result<SalesReturnDto>.Failure("شخص خریدار یافت نشد.");
                                customerPeople.Inventory -= payment.Amount; // کاهش بدهی
                                _unitOfWork.People.Update(customerPeople);
                                break;

                            case BusinessEntity.Invoices.Type_Pay.Wallet:
                                // بازگشت به کیف پول (افزایش موجودی کیف پول)
                                if (!originalInvoice.CustomerId.HasValue)
                                    return Result<SalesReturnDto>.Failure("برای بازگشت به کیف پول، مشتری باید عضو باشگاه باشد.");
                                var walletDeposit = await _walletService.DepositAsync(
                                    originalInvoice.CustomerId.Value,
                                    payment.Amount,
                                    $"بازگشت وجه به کیف پول فاکتور {returnInvoice.InvoiceNumber}",
                                    returnInvoice.Id);
                                if (!walletDeposit.IsSuccess)
                                    return Result<SalesReturnDto>.Failure(walletDeposit.Message);
                                break;

                            default:
                                return Result<SalesReturnDto>.Failure("نوع بازگشت وجه نامعتبر است.");
                        }
                    }
                }

                // 9. بروزرسانی مبلغ کل فاکتور برگشتی
                returnInvoice.NumberofAllItems = dto.Items.Sum(i => i.Quantity);
                returnInvoice.TotalSum = totalRefundAmount;
                _unitOfWork.Invoices.Update(returnInvoice);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                var resultDto = await GetSalesReturnByIdAsync(returnInvoice.Id);
                return Result<SalesReturnDto>.Success(resultDto.Data, "فاکتور برگشت از فروش با موفقیت ایجاد شد");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "خطا در ایجاد فاکتور برگشت از فروش");
                return Result<SalesReturnDto>.Failure($"خطا در ایجاد فاکتور برگشت از فروش: {ex.Message}");
            }
        }

        public async Task<Result<SalesReturnDto>> GetSalesReturnByIdAsync(int id)
        {
            var invoice = await _unitOfWork.Invoices.GetByIdAsync(id,
                default,
                i => i.People,
                i => i.Customer,
                i => i.Invoices_Item);

            if (invoice == null || invoice.TypeInvoices != BusinessEntity.Invoices.Type_Invices.Sales_Return_Invoice)
                return Result<SalesReturnDto>.Failure("فاکتور برگشت از فروش یافت نشد");

            var dto = MapToDto(invoice);
            return Result<SalesReturnDto>.Success(dto);
        }

        private string GenerateInvoiceNumber()
        {
            return $"SR-{DateTime.UtcNow:yyyyMMddHHmmss}-{new Random().Next(1000, 9999)}";
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

        private SalesReturnDto MapToDto(BusinessEntity.Invoices.Invoices invoice)
        {
            return new SalesReturnDto
            {
                Id = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber,
                // OriginalInvoiceId و OriginalInvoiceNumber را باید از جایی بیاوریم؟ در حال حاضر نداریم.
                // می‌توانیم یک فیلد در Invoices برای ارجاع به فاکتور اصلی اضافه کنیم.
                // فعلاً خالی می‌گذاریم.
                PeopleId = invoice.PeopleId,
                PeopleName = invoice.People?.FirstName + " " + invoice.People?.LastName,
                CustomerId = invoice.CustomerId,
                CustomerName = invoice.Customer?.FirstName + " " + invoice.Customer?.LastName,
                TotalRefundAmount = invoice.TotalSum,
                Items = invoice.Invoices_Item?.Select(i => new SalesReturnItemDto
                {
                    Barcode = i.Barcode,
                    Quantity = i.Number
                }).ToList() ?? new()
            };
        }
    }
}
