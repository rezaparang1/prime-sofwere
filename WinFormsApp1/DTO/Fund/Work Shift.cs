using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.DTO.Fund
{
    public class Work_Shift
    {
        public int Id { get; set; }

        public int CashANDUserId { get; set; }
        public Cash_Register_To_The_User CashRegister { get; set; } = null!;

        public DateTime DateFirst { get; set; }
        public DateTime DateEnd { get; set; }

        public DateTime HoursFirst { get; set; }
        public DateTime HoursEnd { get; set; }
    }
}
