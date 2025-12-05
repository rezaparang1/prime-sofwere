using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.DTO.Bank
{
    public class Definition_Bank_Account
    {
        public int Id { get; set; }
        public int BankId { get; set; }
        public Definition_Bank Bank { get; set; } = null!;
        [MaxLength()]
        public string AccountNumber { get; set; } = string.Empty;
        [MaxLength()]
        public string TypeAccount { get; set; } = string.Empty;
        [MaxLength()]
        public string PeopleAccount { get; set; } = string.Empty;
        [MaxLength()]
        public string CardNumber { get; set; } = string.Empty;
        public bool CardReader { get; set; }
        public long Inventory { get; set; }

        public int AccountId { get; set; }
        public Financial_Operations.Account Account { get; set; } = null!;
    }
}
