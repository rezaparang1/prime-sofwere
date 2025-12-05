using AutoMapper;
using BusinessEntity;
using BusinessLogicLayer;
using BusinessLogicLayer.Repository.Bank;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prime_Software.Controllers.Peoduct
{
    [Route("api/Product/Product")]
    [ApiController]
    [Authorize]
    public class Product : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly BusinessLogicLayer.Interface.Producr.IProductService _ProductService;
        private readonly ILogger<Product> _logger;
        private readonly IMapper _mapper;
        public Product(ICurrentUserService currentUser, IMapper mapper, BusinessLogicLayer.Interface.Producr.IProductService ProductService, ILogger<Product> logger)
        {
            _currentUser = currentUser;
            _ProductService = ProductService;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Request to receive all Product");
            var getall = await _ProductService.GetAll();
            _logger.LogInformation("{Count} items received", getall.Count());
            return Ok(getall);
        }
        [HttpGet("product")]
        public async Task<IActionResult> GetProductReport()
        {
            _logger.LogInformation("Request to receive all Product");
            var getall = await _ProductService.GetProductReport();
            _logger.LogInformation("{Count} items received", getall.Count());
            return Ok(getall);
        }
        [HttpGet("sales")]
        public async Task<IActionResult> GetSalesReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate , [FromQuery] string? barcode = null)
        {
            if (startDate > endDate)
            {
                return BadRequest("تاریخ شروع نباید بعد از تاریخ پایان باشد.");
            }

            _logger.LogInformation("Received sales report request from {Start} to {End}.", startDate, endDate);

            var report = await _ProductService.GetProductSalesReportByDateAsync(startDate, endDate , barcode);

            if (!report.Any())
                return NotFound("در این بازه زمانی فروشی ثبت نشده است.");

            return Ok(report);
        }
        [HttpGet("inventory")]
        public async Task<IActionResult> GetProductInventoryAsync([FromQuery] string? barcode = null)
        {
            
            _logger.LogInformation("Received sales report request from GetProductInventoryAsync {barcode}.", barcode);

            var report = await _ProductService.GetProductInventoryAsync(barcode);

            if (!report.Any())
                return NotFound("موردی پیدا نشد .");

            return Ok(report);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessEntity.Product.Product>> GetById(int id)
        {
            _logger.LogInformation("Request to receive Product with ID: {Id}", id);
            var getbyid = await _ProductService.GetById(id);
            if (getbyid == null)
            {
                _logger.LogWarning("Product with ID {Id} not found.", id);
                return NotFound();
            }
            return Ok(getbyid);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DTO.Product.Product dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = _currentUser.UserId!.Value;

            var product = _mapper.Map<BusinessEntity.Product.Product>(dto); // AutoMapper استفاده کن

            var result = await _ProductService.Create(userId, product);

            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(new { success = true, message = result.Message });
        }
        [HttpGet("barcode/{barcode}")]
        public async Task<IActionResult> GetByBarcode(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode))
                return BadRequest("بارکد نمی‌تواند خالی باشد.");

            var product = await _ProductService.GetProductByBarcodeAsync(barcode);

            if (product == null)
                return NotFound($"محصول با بارکد {barcode} یافت نشد.");

            return Ok(product);
        }
        [HttpGet("button-products")]
        public async Task<IActionResult> GetButtonProducts()
        {
            var result = await _ProductService.SearchbyButtonProducts();

            if (result == null || result.Count == 0)
                return NotFound("هیچ محصول فعالی یافت نشد.");

            return Ok(result);
        }
    }
}
