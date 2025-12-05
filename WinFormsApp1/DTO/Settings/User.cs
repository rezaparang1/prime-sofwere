
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.DTO.Settings
{
    public class User
    {
        public int Id { get; set; }
        public int PeopleId { get; set; }
        public People.People People { get; set; } = null!;
        [MaxLength(30)]
        public string UserName { get; set; } = string.Empty;
        [MaxLength(30)]
        public string Password { get; set; } = string.Empty;
        public int GroupUserId { get; set; }
        public Group_User Group_User { get; set; } = null!;
        public Fund.Cash_Register_To_The_User? CashRegister { get; set; }
        public ICollection<Financial_Operations.Receive_OR_Pay> Receive_OR_Pay { get; set; } = new List<Financial_Operations.Receive_OR_Pay>();
      //  public ICollection<LogUser> LogUser { get; set; } = new List<LogUser>();
        public ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();
    }
}
