using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class InvoiceCalculationRequestDto
    {
        public int? PeopleId { get; set; }
        public int? CustomerId { get; set; }
        public int StoreId { get; set; }
        public List<InvoiceItemDto> Items { get; set; }
    }
}
