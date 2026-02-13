using BusinessEntity.Settings;

namespace BusinessEntity.Fund
{
    public class Cash_Register_To_The_User
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int FundId { get; set; }
        public Fund? Fund { get; set; }

        public long InitialAmount { get; set; }
        public bool IsActive { get; set; }
        public DateTime Date { get; set; }
        public ICollection<Work_Shift> WorkShifts { get; set; } = new List<Work_Shift>();
    }
}
