using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Customer_Club
{
    public class PublicDiscount
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan StartTime { get; set; }  // ساعت شروع روزانه
        public TimeSpan EndTime { get; set; }    // ساعت پایان روزانه
        public bool IsActive { get; set; } = true;
        public DiscountType Type { get; set; }
        public int Value { get; set; }
        // روزهای فعال هفته
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }

        public bool IsRecurring { get; set; } = false; // آیا تکرار شونده است؟
        public int? RecurringDays { get; set; } // هر چند روز یکبار تکرار شود
        public virtual ICollection<PublicDiscountProduct>? Products { get; set; }

        public int StoreId { get; set; }
        public virtual Store? Store { get; set; }
    }
}
