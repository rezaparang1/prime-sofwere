using BusinessEntity.Customer_Club;
using BusinessLogicLayer.DTO;
using DataAccessLayer.Interface;
using DataAccessLayer.Interface.Customer_Club;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Prime_Software.Controllers.Customer_Club
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LevelsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LevelsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Levels?storeId=1
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? storeId = null)
        {
            if (storeId.HasValue)
            {
                var levels = await _unitOfWork.CustomerLevels
                    .FindAsync(cl => cl.StoreId == storeId.Value && cl.IsActive);
                return Ok(new { success = true, data = levels });
            }
            else
            {
                var levels = await _unitOfWork.CustomerLevels
                    .FindAsync(cl => cl.IsActive);
                return Ok(new { success = true, data = levels });
            }
        }

        // GET: api/Levels/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var level = await _unitOfWork.CustomerLevels.GetByIdAsync(id);
            if (level == null)
                return NotFound(new { success = false, message = "سطح یافت نشد" });

            return Ok(new { success = true, data = level });
        }

        // POST: api/Levels
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerLevelCreateDto dto)
        {
            // اعتبارسنجی ساده
            if (dto.MaxPoints.HasValue && dto.MinPoints >= dto.MaxPoints)
                return BadRequest(new { success = false, message = "حداقل امتیاز باید کمتر از حداکثر باشد" });

            // بررسی تکراری نبودن نام (اختیاری)
            var existing = await _unitOfWork.CustomerLevels
                .FirstOrDefaultAsync(cl => cl.Name == dto.Name && cl.StoreId == dto.StoreId);
            if (existing != null)
                return BadRequest(new { success = false, message = "این نام سطح قبلاً ثبت شده است" });

            var level = new CustomerLevel
            {
                Name = dto.Name,
                Description = dto.Description,
                MinPoints = dto.MinPoints,
                MaxPoints = dto.MaxPoints,
                DiscountPercent = dto.DiscountPercent,
                StoreId = dto.StoreId,
                IsActive = true
            };

            await _unitOfWork.CustomerLevels.AddAsync(level);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new { success = true, data = level, message = "سطح با موفقیت ایجاد شد" });
        }

        // PUT: api/Levels/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerLevelUpdateDto dto)
        {
            var level = await _unitOfWork.CustomerLevels.GetByIdAsync(id);
            if (level == null)
                return NotFound(new { success = false, message = "سطح یافت نشد" });

            // اعتبارسنجی بازه امتیاز (در صورت تغییر)
            if (dto.MinPoints.HasValue && dto.MaxPoints.HasValue && dto.MinPoints >= dto.MaxPoints)
                return BadRequest(new { success = false, message = "حداقل امتیاز باید کمتر از حداکثر باشد" });

            // به‌روزرسانی فقط فیلدهایی که ارسال شده‌اند
            if (!string.IsNullOrWhiteSpace(dto.Name))
            {
                // بررسی تکراری نبودن نام جدید (به جز خودش)
                var existing = await _unitOfWork.CustomerLevels
                    .FirstOrDefaultAsync(cl => cl.Name == dto.Name && cl.StoreId == level.StoreId && cl.Id != id);
                if (existing != null)
                    return BadRequest(new { success = false, message = "این نام سطح قبلاً ثبت شده است" });
                level.Name = dto.Name;
            }

            if (dto.Description != null)
                level.Description = dto.Description;

            if (dto.MinPoints.HasValue)
                level.MinPoints = dto.MinPoints.Value;

            if (dto.MaxPoints != null) // می‌تواند null ارسال شود
                level.MaxPoints = dto.MaxPoints;

            if (dto.DiscountPercent.HasValue)
                level.DiscountPercent = dto.DiscountPercent.Value;

            if (dto.IsActive.HasValue)
                level.IsActive = dto.IsActive.Value;

            _unitOfWork.CustomerLevels.Update(level);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new { success = true, data = level, message = "سطح با موفقیت به‌روزرسانی شد" });
        }

        // DELETE: api/Levels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var level = await _unitOfWork.CustomerLevels.GetByIdAsync(id);
            if (level == null)
                return NotFound(new { success = false, message = "سطح یافت نشد" });

            // بررسی عدم استفاده از سطح در مشتریان (اختیاری)
            var hasCustomers = await _unitOfWork.Customers.AnyAsync(c => c.CustomerLevelId == id);
            if (hasCustomers)
                return BadRequest(new { success = false, message = "این سطح به مشتریانی متصل است و قابل حذف نیست" });

            _unitOfWork.CustomerLevels.Remove(level);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new { success = true, message = "سطح با موفقیت حذف شد" });
        }
    }
}
