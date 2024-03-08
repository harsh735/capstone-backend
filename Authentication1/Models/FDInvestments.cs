using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication1.Models
{
    public class FDInvestments
    {
        [Key]
        public int FixedDepositID { get; set; }

        [ForeignKey("Users")]
        public int UserID { get; set; }

        [ForeignKey("InvestmentInfo")]

        public int InvestorInfoID { get; set; }
        public decimal Amount { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Type { get; set; }
        public DateTime Timeframe { get; set; }
        public string Risk { get; set; }
        public decimal RateOfInterest { get; set; }
        public decimal ExpectedAmount { get { return Amount * (1 + (RateOfInterest / 100)); } }
    }
}
