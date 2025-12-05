using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Fund
{
    public class ActiveShiftDto
    {
        public int ShiftId { get; set; }
        public string UserName { get; set; }
        public string FundName { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }

}
