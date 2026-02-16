using BusinessEntity;
using BusinessLogicLayer;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Interface.Invoices;
using BusinessLogicLayer.Interface.Producr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Prime_Software.Hubs;

namespace Prime_Software.Controllers.Invoices
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly ITempInvoiceService _tempInvoiceService;
        private readonly IHubContext<InvoiceHub> _hubContext;
        private readonly IProductService _productService;

        // تزریق وابستگی‌ها
        public InvoicesController(
            IInvoiceService invoiceService,
            ITempInvoiceService tempInvoiceService,
            IHubContext<InvoiceHub> hubContext,
            IProductService productService)
        {
            _invoiceService = invoiceService;
            _tempInvoiceService = tempInvoiceService;
            _hubContext = hubContext;
            _productService = productService;
        }

        /// <summary>
        /// ایجاد فاکتور نهایی با تمام تخفیف‌ها (همان قبلی)
        /// </summary>
        [HttpPost("create-with-discounts")]
        public async Task<IActionResult> CreateWithDiscounts([FromBody] InvoiceCreateDto dto)
        {
            var result = await _invoiceService.CreateInvoiceWithAllDiscountsAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(new { success = false, message = result.Message });

            // پس از ثبت، می‌توانید آیتم‌های موقت را پاک کنید
            if (dto.TempInvoiceId.HasValue)
                await _tempInvoiceService.RemoveAsync(dto.TempInvoiceId.Value);

            return Ok(new { success = true, data = result.Data, message = result.Message });
        }

        /// <summary>
        /// دریافت فاکتور نهایی با شناسه
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoice(int id)
        {
            var result = await _invoiceService.GetInvoiceByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(new { success = false, message = result.Message });

            return Ok(new { success = true, data = result.Data });
        }

        /// <summary>
        /// استفاده از کیف پول برای فاکتور
        /// </summary>
        [HttpPost("{invoiceId}/use-wallet")]
        public async Task<IActionResult> UseWallet(int invoiceId, [FromBody] UseWalletDto dto)
        {
            var result = await _invoiceService.UseWalletForPaymentAsync(invoiceId, dto.CustomerId, dto.Amount);
            if (!result.IsSuccess)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(new { success = true, message = result.Message });
        }

        // ========== بخش جدید برای فاکتور موقت و SignalR ==========

        /// <summary>
        /// شروع یک فاکتور موقت (برای اسکن کالا قبل از انتخاب مشتری)
        /// </summary>
        [HttpPost("start")]
        public async Task<IActionResult> StartInvoice()
        {
            // تولید یک شناسه موقت (می‌توانید از Guid یا شماره‌های تصادفی استفاده کنید)
            var tempInvoiceId = new Random().Next(100000, 999999);
            return Ok(new { success = true, invoiceId = tempInvoiceId });
        }

        /// <summary>
        /// افزودن آیتم به فاکتور موقت
        /// </summary>
        [HttpPost("{tempInvoiceId}/add-item")]
        public async Task<IActionResult> AddItem(int tempInvoiceId, [FromBody] InvoiceItemDto item)
        {
            var items = await _tempInvoiceService.GetItemsAsync(tempInvoiceId);
            items.Add(item);
            await _tempInvoiceService.SaveItemsAsync(tempInvoiceId, items);
            return Ok(new { success = true });
        }

        /// <summary>
        /// دریافت اطلاعات محصول با بارکد (برای نمایش در جدول قبل از انتخاب مشتری)
        /// </summary>
        [HttpGet("product-info")]
        public async Task<IActionResult> GetProductInfo(
            [FromQuery] string barcode,
            [FromQuery] int? peopleId = null,
            [FromQuery] int? customerId = null,
            [FromQuery] int storeId = 1)
        {
            var result = await _productService.GetProductInfoForInvoiceAsync(barcode, peopleId, customerId, storeId);
            if (!result.IsSuccess)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(new { success = true, data = result.Data });
        }

        /// <summary>
        /// انتخاب مشتری و به‌روزرسانی قیمت‌ها از طریق SignalR
        /// </summary>
        [HttpPost("{tempInvoiceId}/select-customer")]
        public async Task<IActionResult> SelectCustomer(int tempInvoiceId, [FromBody] SelectCustomerDto dto)
        {
            // 1. دریافت آیتم‌های موقت
            var tempItems = await _tempInvoiceService.GetItemsAsync(tempInvoiceId);
            if (tempItems == null || !tempItems.Any())
                return BadRequest(new { success = false, message = "فاکتور موقت یافت نشد یا خالی است." });

            // 2. تبدیل به فرمت محاسبه
            var calculationRequest = new InvoiceCalculationRequestDto
            {
                PeopleId = dto.PeopleId,
                CustomerId = dto.CustomerId,
                StoreId = dto.StoreId,
                Items = tempItems.Select(i => new InvoiceItemDto
                {
                    Barcode = i.Barcode,
                    Quantity = i.Quantity
                }).ToList()
            };

            // 3. محاسبه مجدد قیمت‌ها
            var calculationResult = await _invoiceService.CalculateInvoiceAsync(calculationRequest);
            if (!calculationResult.IsSuccess)
                return BadRequest(new { success = false, message = calculationResult.Message });

            // 4. ارسال نتیجه به کلاینت از طریق SignalR
            await _hubContext.Clients.Group($"invoice-{tempInvoiceId}")
                .SendAsync("PricesUpdated", calculationResult.Data);

            return Ok(new { success = true, message = "قیمت‌ها با موفقیت به‌روزرسانی شدند." });
        }
    }
}
