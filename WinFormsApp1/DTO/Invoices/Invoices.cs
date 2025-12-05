using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.DTO.Invoices
{
    public class Invoices
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int PeopleId { get; set; }
        public People.People People { get; set; } = null!;
        public int NumberofAllItems { get; set; }
        public int OffAll { get; set; }
        public int TotalSum { get; set; }
        public bool IsUpdate { get; set; }
        public int UserId { get; set; }
        public Settings.User User { get; set; } = null!;
        public Type_Pay TyepPay { get; set; }
        public Type_Invices TypeInvoices { get; set; }
        public List<Invoices_Item> Invoices_Item { get; set; } = null!;
    }
}
