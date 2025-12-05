using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.DTO.People
{
    public class People
    {
        public int Id { get; set; }
        public string IdPeople { get; set; } = string.Empty;
        public int TypePeopleId { get; set; }
        public Type_People Type_People { get; set; } = null!;
        [MaxLength(50)]
        public string FirstName { get; set; } = String.Empty;
        [MaxLength(50)]
        public string LastName { get; set; } = String.Empty;
        [MaxLength(12)]
        public string Phone { get; set; } = string.Empty;
        public decimal CreditLimit { get; set; }
        public bool IsCreditLimit { get; set; }
        public HowToDoBusiness HowToDoBusiness { get; set; }
        public int OFF { get; set; }
        public bool Business { get; set; }
        public bool User { get; set; }
        public bool Employee { get; set; }
        public bool Investor { get; set; }
        [MaxLength(200)]
        public string Description { get; set; } = String.Empty;
        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;
        public bool TaxFree { get; set; }
        public int InitialCapital { get; set; }
        public long Inventory { get; set; }
        public int GroupPeopleId { get; set; }
        public Group_People Group_People { get; set; } = null!;
        public int PriceLevelID { get; set; }
        public Product.PriceLevels PriceLevel { get; set; } = null!;
        public ICollection<Settings.User> Users { get; set; } = new List<Settings.User>();
        public ICollection<Product.Storeroom_Product> Storeroom_Product { get; set; } = new List<Product.Storeroom_Product>();
    }
}
