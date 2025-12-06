using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Customer_Club
{
    public class Customer_Level
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int RequiredPoints { get; set; }
        public string Description { get; set; } = string.Empty;
        public ICollection<Customer> Customers { get; set; } = null!;

    }
}
