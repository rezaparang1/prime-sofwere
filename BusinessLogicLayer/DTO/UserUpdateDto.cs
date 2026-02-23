using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class UserUpdateDto
    {
        public string? UserName { get; set; }
        public int? PeopleId { get; set; }
        public int? GroupUserId { get; set; }
        public bool? IsActive { get; set; }
        public string? ImageAddress { get; set; }
        public DateTime? Validity { get; set; }
        public string? Password { get; set; } // در صورت تغییر رمز
    }
}
