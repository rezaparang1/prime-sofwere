using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.DTO.Product
{
    public class SaleDto
    {
        public string Date { get; set; } = string.Empty; // تاریخ شمسی
        public int Qty { get; set; }
    }
}
