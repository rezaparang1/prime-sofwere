using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.DTO.Product
{
    public class SalePerDayDto
    {
        public string Date { get; set; } = string.Empty; // مثال: "1404/04/04" (شمسی)
        public int Qty { get; set; }
    }
}
