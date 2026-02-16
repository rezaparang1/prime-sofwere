using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Interface.Invoices;
using DataAccessLayer.Interface;
using Microsoft.Extensions.Logging;
using BusinessEntity.Invoices;

namespace BusinessLogicLayer.Repository.Invoices
{
    public class PurchaseReturnService : IPurchaseReturnService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PurchaseReturnService> _logger;

        public PurchaseReturnService(
            IUnitOfWork unitOfWork,
            ILogger<PurchaseReturnService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<PurchaseReturnDto>> CreatePurchaseReturnAsync(PurchaseReturnCreateDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // 1. دریافت فاکتور خرید اصلی
                var originalInvoice = await _unitOfWork.Invoices.GetByIdAsync(dto.OriginalInvoiceId,
                    default,
                    i => i.People,                    // تأمین‌کننده
                    i => i.Invoices_Item);             // آیتم‌ها
                if (originalInvoice == null || originalInvoice.TypeInvoices != Type_Invices.Purchase_Invoice)
                    return Result<PurchaseReturnDto>.Failure("فاکتور خرید اصلی یافت نشد");

                // 2. اعتبارسنجی آیتم‌های برگشتی
                var returnItems = new List<(Invoices_Item originalItem, int returnQty, int perUnitPrice)>();
                foreach (var itemDto in dto.Items)
                {
                    var originalItem = originalInvoice.Invoices_Item.FirstOrDefault(i => i.Barcode == itemDto.Barcode);
                    if (originalItem == null)
                        return Result<PurchaseReturnDto>.Failure($"کالا با بارکد {itemDto.Barcode} در فاکتور اصلی یافت نشد");

                    if (itemDto.Quantity > originalItem.Number)
                        return Result<PurchaseReturnDto>.Failure($"تعداد برگشتی برای {originalItem.Name} بیشتر از تعداد خریداری شده است");

                    int perUnitPrice = originalItem.Price; // قیمت خرید هر واحد (از آیتم اصلی)
                    returnItems.Add((originalItem, itemDto.Quantity, perUnitPrice));
                }

                // 3. ایجاد فاکتور برگشت از خرید
                var returnInvoice = new BusinessEntity.Invoices.Invoices
                {
                    InvoiceNumber = GenerateInvoiceNumber(),
                    Date = dto.Date,
                    PeopleId = originalInvoice.PeopleId,      // همان تأمین‌کننده
                    CustomerId = originalInvoice.CustomerId,  // معمولاً null
                    UserId = dto.UserId,
                    TyepPay = originalInvoice.TyepPay,        // نوع پرداخت اصلی (برای سازگاری)
                    TypeInvoices = Type_Invices.Purchase_Return_Invoice,
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

                // 4. پردازش آیتم‌های برگشتی
                foreach (var (originalItem, returnQty, perUnitPrice) in returnItems)
                {
                    // کاهش موجودی کالا (چون کالا به تأمین‌کننده برمی‌گردد)
                    var product = await _unitOfWork.Products.GetByIdAsync(originalItem.ProductId);
                    if (product == null)
                        return Result<PurchaseReturnDto>.Failure($"کالا با شناسه {originalItem.ProductId} یافت نشد");

                    if (product.Inventory < returnQty)
                        return Result<PurchaseReturnDto>.Failure($"موجودی کالای '{product.Name}' برای برگشت کافی نیست");

                    product.Inventory -= returnQty;
                    _unitOfWork.Products.Update(product);

                    // محاسبه مبلغ برگشتی برای این آیتم (بر اساس قیمت خرید)
                    int itemRefund = perUnitPrice * returnQty;
                    totalRefundAmount += itemRefund;

                    // ایجاد آیتم فاکتور برگشتی
                    var returnItem = new Invoices_Item
                    {
                        InvoicesId = returnInvoice.Id,
                        ProductId = originalItem.ProductId,
                        Barcode = originalItem.Barcode,
                        Name = originalItem.Name,
                        Number = returnQty,
                        Price = perUnitPrice,
                        OFF = 0,
                        AllPrice = itemRefund,
                        OriginalPrice = itemRefund,
                        ClubDiscount = 0,
                        PublicDiscountId = null,
                        ClubDiscountId = null
                    };
                    await _unitOfWork.InvoiceItems.AddAsync(returnItem);
                }

                // 5. پردازش دریافت وجه (RefundPayments)
                if (dto.RefundPayments != null && dto.RefundPayments.Any())
                {
                    int totalPayments = dto.RefundPayments.Sum(p => p.Amount);
                    if (totalPayments != totalRefundAmount)
                        return Result<PurchaseReturnDto>.Failure("مجموع مبالغ دریافتی با مبلغ برگشتی همخوانی ندارد.");

                    foreach (var payment in dto.RefundPayments)
                    {
                        switch (payment.PaymentType)
                        {
                            case Type_Pay.Cash:
                                // دریافت وجه نقد از تأمین‌کننده → افزایش موجودی صندوق
                                if (!payment.FundId.HasValue)
                                    return Result<PurchaseReturnDto>.Failure("شناسه صندوق برای دریافت نقدی الزامی است.");
                                var fund = await _unitOfWork.Funds.GetByIdAsync(payment.FundId.Value);
                                if (fund == null)
                                    return Result<PurchaseReturnDto>.Failure("صندوق انتخابی یافت نشد.");
                                fund.Inventory += payment.Amount;
                                _unitOfWork.Funds.Update(fund);
                                await CreateTransaction(fund.AccountId, payment.Amount, "Increase",
                                    $"دریافت وجه نقد بابت برگشت خرید فاکتور {returnInvoice.InvoiceNumber}");
                                break;

                            case Type_Pay.Bank:
                                // دریافت وجه به حساب بانکی → افزایش موجودی حساب بانکی
                                if (!payment.BankAccountId.HasValue)
                                    return Result<PurchaseReturnDto>.Failure("شناسه حساب بانکی برای دریافت بانکی الزامی است.");
                                var bankAccount = await _unitOfWork.BankAccounts.GetByIdAsync(payment.BankAccountId.Value);
                                if (bankAccount == null)
                                    return Result<PurchaseReturnDto>.Failure("حساب بانکی انتخابی یافت نشد.");
                                bankAccount.Inventory += payment.Amount;
                                _unitOfWork.BankAccounts.Update(bankAccount);
                                await CreateTransaction(bankAccount.AccountId, payment.Amount, "Increase",
                                    $"دریافت وجه بانکی بابت برگشت خرید فاکتور {returnInvoice.InvoiceNumber}");
                                break;

                            case Type_Pay.Credit:
                                // دریافت به صورت کاهش بدهی به تأمین‌کننده
                                // فرض می‌کنیم Inventory تأمین‌کننده میزان بدهی ما به اوست
                                var supplier = originalInvoice.People;
                                if (supplier == null)
                                    return Result<PurchaseReturnDto>.Failure("تأمین‌کننده یافت نشد.");
                                supplier.Inventory -= payment.Amount; // کاهش بدهی
                                _unitOfWork.People.Update(supplier);
                                break;

                            case Type_Pay.Wallet:
                                // در خرید از کیف پول استفاده نمی‌شود
                                return Result<PurchaseReturnDto>.Failure("پرداخت با کیف پول برای برگشت خرید معتبر نیست.");

                            default:
                                return Result<PurchaseReturnDto>.Failure("نوع دریافت وجه نامعتبر است.");
                        }
                    }
                }

                // 6. بروزرسانی مبلغ کل فاکتور برگشتی
                returnInvoice.NumberofAllItems = dto.Items.Sum(i => i.Quantity);
                returnInvoice.TotalSum = totalRefundAmount;
                _unitOfWork.Invoices.Update(returnInvoice);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                var resultDto = await GetPurchaseReturnByIdAsync(returnInvoice.Id);
                return Result<PurchaseReturnDto>.Success(resultDto.Data, "فاکتور برگشت از خرید با موفقیت ایجاد شد");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "خطا در ایجاد فاکتور برگشت از خرید");
                return Result<PurchaseReturnDto>.Failure($"خطا در ایجاد فاکتور برگشت از خرید: {ex.Message}");
            }
        }

        public async Task<Result<PurchaseReturnDto>> GetPurchaseReturnByIdAsync(int id)
        {
            var invoice = await _unitOfWork.Invoices.GetByIdAsync(id,
                default,
                i => i.People,
                i => i.Invoices_Item);

            if (invoice == null || invoice.TypeInvoices != Type_Invices.Purchase_Return_Invoice)
                return Result<PurchaseReturnDto>.Failure("فاکتور برگشت از خرید یافت نشد");

            var dto = MapToDto(invoice);
            return Result<PurchaseReturnDto>.Success(dto);
        }

        private string GenerateInvoiceNumber()
        {
            return $"PR-{DateTime.UtcNow:yyyyMMddHHmmss}-{new Random().Next(1000, 9999)}";
        }

        private async Task CreateTransaction(int accountId, long amount, string type, string description)
        {
            var transaction = new Transaction
            {
                AccountId = accountId,
                Amount = amount,
                Type = type,
                Description = description,
                Date = DateTime.UtcNow
            };
            await _unitOfWork.Transactions.AddAsync(transaction);
        }

        private PurchaseReturnDto MapToDto(BusinessEntity.Invoices.Invoices invoice)
        {
            return new PurchaseReturnDto
            {
                Id = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber,
                // OriginalInvoiceId و OriginalInvoiceNumber را باید از جایی بیاوریم؛ در حال حاضر در موجودیت نیست.
                // می‌توانید یک فیلد به Invoices اضافه کنید (OriginalInvoiceId) و هنگام ساخت مقداردهی کنید.
                PeopleId = invoice.PeopleId,
                PeopleName = invoice.People?.FirstName + " " + invoice.People?.LastName,
                CustomerId = invoice.CustomerId,
                TotalRefundAmount = invoice.TotalSum,
                Items = invoice.Invoices_Item?.Select(i => new PurchaseReturnItemDto
                {
                    Barcode = i.Barcode,
                    Quantity = i.Number
                }).ToList() ?? new()
            };
        }
    }
}
