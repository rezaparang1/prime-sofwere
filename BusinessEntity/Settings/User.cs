using BusinessEntity.Bank;
using BusinessEntity.Fund;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessEntity.Settings
{
    public class User
    {
        public int Id { get; set; }

        public int PeopleId { get; set; }
        public BusinessEntity.People.People? People { get; set; }

        [MaxLength(30)]
        public string UserName { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Password { get; set; } = string.Empty;

        public DateTime LastActivity { get; set; }
        public bool IsDelete { get; set; }
        public bool IsActive { get; set; }

        public DateTime Validity { get; set; }
        public string ImageAddress { get; set; } = string.Empty;

        public int GroupUserId { get; set; }
        public Group_User? Group_User { get; set; }

        public string? CurrentSessionId { get; set; }
        [JsonIgnore]
        public ICollection<Cash_Register_To_The_User> CashRegisters { get; set; }
            = new List<Cash_Register_To_The_User>();

        public ICollection<BusinessEntity.Invoices.Invoices> Invoices { get; set; } = new List<BusinessEntity.Invoices.Invoices>();
        public ICollection<LogUser> LogUser { get; set; } = new List<LogUser>();
        public ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();
    }
}
