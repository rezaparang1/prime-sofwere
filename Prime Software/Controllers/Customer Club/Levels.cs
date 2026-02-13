using BusinessEntity.Customer_Club;
using BusinessLogicLayer.DTO;
using DataAccessLayer.Interface;
using DataAccessLayer.Interface.Customer_Club;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Prime_Software.Controllers.Customer_Club
{
    public class LevelsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LevelsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int storeId)
        {
            var levels = await _unitOfWork.CustomerLevels
                .FindAsync(cl => cl.StoreId == storeId && cl.IsActive);
            return Ok(new { success = true, data = levels });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerLevelCreateDto dto)
        {
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
    }
}
