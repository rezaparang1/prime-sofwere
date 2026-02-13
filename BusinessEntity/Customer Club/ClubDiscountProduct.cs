using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Customer_Club
{
    public class ClubDiscountProduct
    {
        public int Id { get; set; }
        public int ClubDiscountId { get; set; }
        public virtual ClubDiscount? ClubDiscount { get; set; }

        // اگر تخفیف روی کل محصول باشد
        public int? ProductId { get; set; }
        public virtual Product.Product? Product { get; set; }

        // اگر تخفیف روی واحد خاصی از محصول باشد
        public int? UnitLevelId { get; set; }
        public virtual BusinessEntity.Product.UnitsLevel? UnitLevel { get; set; }

        public int ClubPrice { get; set; }       // قیمت ویژه باشگاه برای این واحد
        public int OriginalPrice { get; set; }   // قیمت اصلی (جهت اطلاع)
    }
}
