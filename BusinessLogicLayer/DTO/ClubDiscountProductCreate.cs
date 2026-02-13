using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class ClubDiscountProductCreateDto
    {
        public int? ProductId { get; set; }      // یکی از این دو
        public int? UnitLevelId { get; set; }    // باید مقدار داشته باشد
        public int ClubPrice { get; set; }
    }
}
