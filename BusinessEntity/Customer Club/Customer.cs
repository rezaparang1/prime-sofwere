using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Customer_Club
{
    public class Customer
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;
        [MaxLength(12)]
        public string Mobile { get; set; } = string.Empty;
        [MaxLength(80)]
        public string? Email { get; set; }
        public string? Barcode { get; set; }  
        public DateTime RegisterDate { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsClubMember { get; set; } = false;
        public int TotalPoints { get; set; } = 0;
        public int CurrentPoints { get; set; } = 0;
        public decimal TotalPurchaseAmount { get; set; } = 0;
        public int TotalPurchaseCount { get; set; } = 0;
        public DateTime? LastPurchaseDate { get; set; }

        public int? PeopleId { get; set; }
        [ForeignKey(nameof(PeopleId))]
        public virtual People.People? People { get; set; }


        // ارتباطات
        public int? CustomerLevelId { get; set; }
        public virtual CustomerLevel? CustomerLevel { get; set; }
        public virtual Wallet? Wallet { get; set; }
        public virtual ICollection<CustomerLevelHistory>? LevelHistories { get; set; }
        public virtual ICollection<PointTransaction>? PointTransactions { get; set; }
        public virtual ICollection<Invoices.Invoices>? Invoices { get; set; }

        public int StoreId { get; set; }
        public virtual Store? Store { get; set; }

    }
}
