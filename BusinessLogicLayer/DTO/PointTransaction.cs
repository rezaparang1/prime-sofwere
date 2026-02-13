using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class PointTransactionDto
    {
        public int Points { get; set; }
        public string Type { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
