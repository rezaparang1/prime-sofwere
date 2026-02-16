using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class CustomerSearchDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? CustomerLevelId { get; set; }
        public int? MinPoints { get; set; }
        public int? MaxPoints { get; set; }
        public string? Barcode { get; set; }
    }
}
