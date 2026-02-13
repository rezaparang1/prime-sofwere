using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface.Customer_Club
{
    public class PublicDiscountCalculationResult
    {
        public int DiscountAmount { get; set; }
        public int? DiscountId { get; set; }
        public int? DiscountedPrice { get; set; }
    }
}
