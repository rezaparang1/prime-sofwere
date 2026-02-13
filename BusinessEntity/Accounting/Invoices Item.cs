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
    public class Invoices_Item
    {
        public int Id { get; set; }
     
        public int ProductId { get; set; }
        public Product.Product? Product { get; set; } = null!;

        public string Barcode { get; set; } = string.Empty;
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        public int Number { get; set; }
        public int Price { get; set; }
        public int OFF { get; set; }    
        public int AllPrice { get; set; }
        
        public int InvoicesId { get; set; }
        public Invoices? Invoices { get; set; } = null!;

        public int ClubDiscount { get; set; } = 0;
        public int? PublicDiscountId { get; set; }
        public int? ClubDiscountId { get; set; }
        public int OriginalPrice { get; set; }  // قیمت قبل از تخفیف

        public virtual PublicDiscount? PublicDiscount { get; set; }
        public virtual ClubDiscount? ClubDiscountEntity { get; set; }
    }
}
