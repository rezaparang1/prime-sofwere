using BusinessEntity.Bank;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Settings
{
    public class Group_User
    {
        public int Id { get; set; }

        [MaxLength(30)]
        public string Name { get; set; } = string.Empty;
        public bool IsDelete { get; set; }
        public Access_Level? AccessLevel { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
