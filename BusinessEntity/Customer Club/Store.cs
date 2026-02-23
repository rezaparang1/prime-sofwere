using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Customer_Club
{
    public class Store
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Address { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(50)]
        public string? ManagerName { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDelete { get; set; } 
        public DateTime RegisterDate { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual ICollection<Customer>? Customers { get; set; }
        public virtual ICollection<CustomerLevel>? CustomerLevels { get; set; }
        public virtual ICollection<ClubDiscount>? ClubDiscounts { get; set; }
        public virtual ICollection<PublicDiscount>? PublicDiscounts { get; set; }
    }
}
