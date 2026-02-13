using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Customer_Club
{
    public class CustomerLevelHistory
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int CustomerLevelId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; } // null یعنی سطح فعلی

        public virtual Customer? Customer { get; set; }
        public virtual CustomerLevel? CustomerLevel { get; set; }
    }
}
