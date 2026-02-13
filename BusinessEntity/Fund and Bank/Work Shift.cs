using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Fund
{
    public class Work_Shift
    {
        public int Id { get; set; }

        public int CashRegisterToUserId { get; set; }
        public Cash_Register_To_The_User? CashRegisterToUser { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public long OpeningAmount { get; set; }
        public long ClosingAmount { get; set; }

        public bool IsAuto { get; set; }
        public bool IsClosed { get; set; }
    }
}
