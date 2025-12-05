using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessEntity.People
{
    public class Group_People
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = String.Empty;
        public bool IsDelete { get; set; }
        [JsonIgnore]
        public ICollection<People> People { get; set; } = new List<People>();
    }
}
