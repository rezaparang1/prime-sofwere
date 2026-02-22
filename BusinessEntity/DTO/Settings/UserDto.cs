using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.DTO.Settings
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int? PeopleId { get; set; }
        public string? PeopleFullName { get; set; } // نام کامل شخص (اختیاری)
        public int? GroupUserId { get; set; }
        public string? GroupName { get; set; }      // نام گروه کاربری
        public bool IsActive { get; set; }
        public DateTime? LastActivity { get; set; }
        public DateTime? Validity { get; set; }
    }
}
