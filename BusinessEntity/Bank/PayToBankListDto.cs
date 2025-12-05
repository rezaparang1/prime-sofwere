using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Bank
{
    public class PayToBankListDto
    {
        public int Id { get; set; }
        public string Source { get; set; } = string.Empty;        // صندوق
        public string Destination { get; set; } = string.Empty;   // بانک (شماره حساب)
        public long Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
