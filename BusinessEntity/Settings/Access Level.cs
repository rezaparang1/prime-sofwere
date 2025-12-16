using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Settings
{
    public class Access_Level
    {
        public int Id { get; set; }
        public int GroupUserId { get; set; }
        public Settings.Group_User? Group_User { get; set; } = null!;
        public bool IsBankT { get; set; }
        public bool IsBank { get; set; }
        public bool IsFund { get; set; }
        public bool IsWorkShift { get; set; }
        public bool IsRegisterUser { get; set; }
        public bool IsInvoices{ get; set; }
        public bool IsPeople { get; set; }
        public bool IsGroupPeople { get; set; }
        public bool IsTypePeopel { get; set; }
        public bool IsGroupProduct { get; set; }
        public bool IsPriceLevel { get; set; }
        public bool IsProductFailre { get; set; }
        public bool IsProduct { get; set; }
        public bool IsSectionProduct { get; set; }
        public bool IsTypeProduct { get; set; }
        public bool IsStoreroomProduct { get; set; }
        public bool IsUnitProduct { get; set; }
        public bool IsGroupUser { get; set; }
        public bool IsUser { get; set; }
        public bool IsAccessLevel { get; set; }
        public bool IsViewingofOthers { get; set; }
        public bool IsDelete { get; set; }
        public ICollection<Group_User> Groups { get; set; } = new List<Group_User>();

    }
}
