using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Product
{
    public class StoreroomProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SectionName { get; set; } = string.Empty;
        public string PeopleName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public NegativeBalancePolicy NegativeBalancePolicy { get; set; }
    }
}
