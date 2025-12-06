using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Customer_Club
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Family { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime Britday { get; set; }

        public int PriceLevelId { get; set; }
        public Product.PriceLevels PriceLevels { get; set; } = null!;

        public int Score {  get; set; }
    }
}
