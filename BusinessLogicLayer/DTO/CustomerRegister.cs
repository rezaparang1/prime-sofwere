using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class CustomerRegisterDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string? Email { get; set; }
        public bool RegisterInClub { get; set; } = true;
        public int StoreId { get; set; }
        public int? PeopleId { get; set; } // اختیاری - اتصال به شخص حقیقی/حقوقی
    }
}
