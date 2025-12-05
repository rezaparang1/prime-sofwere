using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Bank
{
    public class Definition_Bank
    {
        public int Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; } = String.Empty;
        public bool IsDelete { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public ICollection<Definition_Bank_Account> BankAccounts { get; set; } = new List<Definition_Bank_Account>();
    }
}
