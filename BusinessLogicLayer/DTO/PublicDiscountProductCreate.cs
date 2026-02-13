using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class PublicDiscountProductCreateDto
    {
        public int? ProductId { get; set; }
        public int? UnitLevelId { get; set; }
        public int DiscountedPrice { get; set; }
    }
}
