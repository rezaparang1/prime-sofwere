using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.DTO.Financial_Operations
{
    public class Receive_OR_Pay
    {
        public int Id { get; set; }

        public OperationType Type { get; set; }

        [MaxLength(80)]
        public string Reason { get; set; } = string.Empty;

        public int UserId { get; set; }
        public Settings.User User { get; set; } = null!;

        public DateTime Date { get; set; }

        public ICollection<Receive_OR_Pay_Item> Items { get; set; } = new List<Receive_OR_Pay_Item>();
    }
}
