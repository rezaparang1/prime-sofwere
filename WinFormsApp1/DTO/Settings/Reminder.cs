using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.DTO.Settings
{
    public class Reminder
    {
        public int Id { get; set; }
        public string Description { get; set; } = String.Empty;
        public DateTime? Date { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
