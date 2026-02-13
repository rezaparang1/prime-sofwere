using BusinessEntity.Customer_Club;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class ClubDiscountCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public DiscountType Type { get; set; }
        public int Value { get; set; }
        public bool RefundToWallet { get; set; } = true;
        public int StoreId { get; set; }
        public List<ClubDiscountProductCreateDto> Products { get; set; } = new();
    }
}
