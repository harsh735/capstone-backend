using System.ComponentModel.DataAnnotations;

namespace Authentication1.Models
{
    public class MutualFundsInvestments
    {
        [Key]
        public int? MutualFundsID { get; set; }
        public int? UserID { get; set; }
        public int? InvestorInfoID { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string? Type { get; set; } = "Mutual Funds";
        public string? Risk { get; set; }
        public decimal? ExpectedAmount { get; set; }
        public string? MFName { get; set; }

        public string? MFType { get; set; }
        public float? NAV { get; set; }


    }
}
