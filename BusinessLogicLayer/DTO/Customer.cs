using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Barcode { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool IsClubMember { get; set; }
        public int TotalPoints { get; set; }
        public int CurrentPoints { get; set; }
        public decimal TotalPurchaseAmount { get; set; }
        public int TotalPurchaseCount { get; set; }
        public DateTime? LastPurchaseDate { get; set; }
        public int? CustomerLevelId { get; set; }
        public string? CustomerLevelName { get; set; }
        public decimal WalletBalance { get; set; }
        public int? PeopleId { get; set; }
        public string? PeopleName { get; set; }
    }
}
