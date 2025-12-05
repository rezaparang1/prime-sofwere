using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.DTO.Fund
{
    public class Cash_Register_To_The_User
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public Settings.User User { get; set; } = null!;
        public int FundId { get; set; }
        public Fund Fund { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime Date { get; set; }
        public ICollection<Work_Shift> WorkShifts { get; set; } = new List<Work_Shift>();
    }
}
