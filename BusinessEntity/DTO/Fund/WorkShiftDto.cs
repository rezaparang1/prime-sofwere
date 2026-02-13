using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.DTO.Fund
{
    public class WorkShiftDto
    {
        public int Id { get; set; }
        public int CashRegisterToUserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public decimal ClosingAmount { get; set; }
        public bool IsClosed { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
