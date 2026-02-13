using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class PublicDiscountDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsActive { get; set; }
        public string Type { get; set; } = string.Empty;
        public int Value { get; set; }
        public int StoreId { get; set; }
        public List<string> DaysOfWeek { get; set; } = new();
        public List<PublicDiscountProductDto>? Products { get; set; }
    }
}
