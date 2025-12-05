using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.People
{
    public class Type_People
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = String.Empty;
        public bool IsDelete { get; set; }
        public ICollection<People> People { get; set; } = new List<People>();
    }
}
