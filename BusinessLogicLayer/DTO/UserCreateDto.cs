using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class UserCreateDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // متن ساده
        public int PeopleId { get; set; }
        public int GroupUserId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? Validity { get; set; }
        public string? ImageAddress { get; set; }
    }
}
