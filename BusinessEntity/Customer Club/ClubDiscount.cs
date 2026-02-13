using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Customer_Club
{
    public class ClubDiscount
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public bool IsActive { get; set; } = true;
        public DiscountType Type { get; set; }
        public int Value { get; set; }  // مقدار تخفیف (ریال یا درصد)
        public bool RefundToWallet { get; set; } = true;  // به کیف پول برگردد؟
        public virtual ICollection<ClubDiscountProduct>? Products { get; set; }
        public int StoreId { get; set; }
        public virtual Store? Store { get; set; }
    }
}
