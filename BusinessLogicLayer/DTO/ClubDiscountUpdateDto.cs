using BusinessEntity.Customer_Club;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class ClubDiscountUpdateDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public DiscountType? Type { get; set; }
        public int? Value { get; set; }
        public bool? RefundToWallet { get; set; }
        public bool? IsActive { get; set; }
        public List<ClubDiscountProductUpdateDto> Products { get; set; } = new();
    }

    public class ClubDiscountProductUpdateDto
    {
        public int? Id { get; set; } // شناسه محصول-تخفیف (برای ویرایش یا حذف)
        public int? ProductId { get; set; }
        public int? UnitLevelId { get; set; }
        public int ClubPrice { get; set; }
        public bool IsDeleted { get; set; } // علامت‌گذاری برای حذف
    }
}
