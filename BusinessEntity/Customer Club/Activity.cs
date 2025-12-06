using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Customer_Club
{
    public class Activity
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;    //id chizi k graftah
        public string Title { get; set; } = string.Empty;   //tozihat baray chi grafta
        public int Points { get; set; }
    }
}
