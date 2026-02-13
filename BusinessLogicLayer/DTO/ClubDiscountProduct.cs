using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class ClubDiscountProductDto
    {
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
        public int? UnitLevelId { get; set; }
        public string? UnitName { get; set; }
        public int ClubPrice { get; set; }
        public int OriginalPrice { get; set; }
    }
}
