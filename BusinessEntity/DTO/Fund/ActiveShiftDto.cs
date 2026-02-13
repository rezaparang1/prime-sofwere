using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.DTO.Fund
{
    public class ActiveShiftDto
    {
        public int ShiftId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FundName { get; set; } = string.Empty;
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
