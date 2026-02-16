using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class InvoiceCalculationResultDto
    {
        public int TotalOriginalPrice { get; set; }
        public int TotalPublicDiscount { get; set; }
        public int TotalClubDiscount { get; set; }
        public int TotalLevelDiscount { get; set; }
        public int FinalAmount { get; set; }
        public List<CalculatedItemDto> Items { get; set; }
    }
}
