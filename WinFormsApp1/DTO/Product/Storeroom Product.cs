using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.DTO.Product
{
    public class Storeroom_Product
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = String.Empty;
        public int SectionProductId { get; set; }
        public Section_Product Section_Product { get; set; } = null!;
        public int PeopleId { get; set; }
        public People.People People { get; set; } = null!;
        [MaxLength(200)]
        public string Description { get; set; } = String.Empty;
        [MaxLength(200)]
        public string Address { get; set; } = String.Empty;
        public bool IsPositive { get; set; }
        public bool IsNegative { get; set; }
        public bool IsMessage { get; set; }
        public ICollection<ProductDto> Products { get; set; } = new List<ProductDto>();
    }
}
