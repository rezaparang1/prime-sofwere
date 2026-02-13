using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class DiscountDetailDto
    {
        public int PublicDiscount { get; set; }
        public int ClubDiscount { get; set; }
        public int LevelDiscount { get; set; }
    }
}
