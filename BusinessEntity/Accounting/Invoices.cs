using BusinessEntity.Customer_Club;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessEntity.Invoices
{
    public class Invoices
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime Date { get; set; }

        public int PeopleId { get; set; }
        public People.People? People { get; set; } = null!; 
        
        public int NumberofAllItems { get; set; }
        public int OffAll { get; set; }
        public int TotalSum { get; set; }
        public bool IsUpdate { get; set; }
       
        public int? CustomerId { get; set; }
        public Customer_Club.Customer? Customer { get; set; } = null!;

        public int UserId { get; set; }
        public Settings.User? User { get; set; } = null!;

        public Type_Pay TyepPay { get; set; } 
        public Type_Invices TypeInvoices { get; set; }  
        
        public List<Invoices_Item> Invoices_Item { get; set; } = null!;

        public int ClubDiscountAll { get; set; } = 0;
        public int EarnedPoints { get; set; } = 0;
        public int UsedWalletAmount { get; set; } = 0;
        public int? CustomerLevelId { get; set; }
        public int LevelDiscountAmount { get; set; } = 0;
        public bool IsClubDiscountRefunded { get; set; } = false;

        // ارتباطات جدید
        public virtual CustomerLevel? CustomerLevel { get; set; }

        public virtual ICollection<WalletTransaction>? WalletTransactions { get; set; }
        public virtual ICollection<PointTransaction>? PointTransactions { get; set; }
    }
}
