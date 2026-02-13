using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Customer_Club
{
    public class PublicDiscountProduct
    {
        public int Id { get; set; }
        public int PublicDiscountId { get; set; }
        public virtual PublicDiscount? PublicDiscount { get; set; }

        public int? ProductId { get; set; }
        public virtual Product.Product? Product { get; set; }

        public int? UnitLevelId { get; set; }
        public virtual BusinessEntity.Product.UnitsLevel? UnitLevel { get; set; }

        public int DiscountedPrice { get; set; } // قیمت پس از تخفیف
        public int OriginalPrice { get; set; }
    }
}
