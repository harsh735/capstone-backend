using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication1.Models
{
    public class FDInvestments
    {
        public int? FixedDepositID { get; set; }

        [ForeignKey("Users")]
        public int? UserID { get; set; }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? InvestorInfoID { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string? Type { get; set; }
        public DateTime? Timeframe { get; set; }
        public string? Risk { get; set; }
        public decimal? RateOfInterest { get; set; }
        public decimal? ExpectedAmount { get; set; }
        public string? FDName { get; set; }
        public string? FDType { get; set; }

    }
}
