using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Customer_Club
{
    public class CustomerLevel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int MinPoints { get; set; }
        public int? MaxPoints { get; set; }
        public int DiscountPercent { get; set; }  // مثلاً 10% تخفیف
        public bool IsActive { get; set; } = true;

        public virtual ICollection<Customer>? Customers { get; set; }
        public virtual ICollection<CustomerLevelHistory>? LevelHistories { get; set; }

        public int StoreId { get; set; }
        public virtual Store? Store { get; set; }
    }
}
