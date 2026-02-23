using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int? PeopleId { get; set; }
        public string? PeopleFullName { get; set; }
        public int? GroupUserId { get; set; }
        public string? GroupName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastActivity { get; set; }
        public DateTime? Validity { get; set; }
        public string? ImageAddress { get; set; }
    }
}
