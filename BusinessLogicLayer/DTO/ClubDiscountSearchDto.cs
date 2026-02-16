using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class ClubDiscountSearchDto
    {
        public int? StoreId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Title { get; set; }
        public int? ProductId { get; set; }
        public int? UnitLevelId { get; set; }
        public bool? IsActive { get; set; }
    }
}
