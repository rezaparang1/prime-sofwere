using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.DTO.Product
{
    public class FailureItemModel
    {
        public string Barcode { get; set; } = string.Empty;
        public int Value { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
