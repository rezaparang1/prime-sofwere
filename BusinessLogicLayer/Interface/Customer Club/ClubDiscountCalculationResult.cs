using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface.Customer_Club
{
    public class ClubDiscountCalculationResult
    {
        public int DiscountAmount { get; set; }
        public int? DiscountId { get; set; }
        public int? ClubPrice { get; set; }
    }
}
