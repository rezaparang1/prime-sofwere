using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class SelectCustomerDto
    {
        public int? PeopleId { get; set; }
        public int? CustomerId { get; set; }
        public int StoreId { get; set; }
    }
}
