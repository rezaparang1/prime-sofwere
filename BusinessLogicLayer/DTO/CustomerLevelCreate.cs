using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class CustomerLevelCreateDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int MinPoints { get; set; }
        public int? MaxPoints { get; set; }
        public int DiscountPercent { get; set; }
        public int StoreId { get; set; }
    }
}
