using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication1.Models
{
    public class GoldInvestments
    {

        [Key]
        public int GoldID { get; set; }
        [ForeignKey("Users")]
        public int UserID { get; set; }
        [ForeignKey("InvestmentInfo")]
        public int InvestorInfoID { get; set; }
        public decimal Amount { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Type { get; set; }
        public DateTime Timeframe { get; set; }
        public string Risk { get; set; }
        public decimal? ExpectedAmount { get; set; }
        public GoldInvestments()
        {
            ExpectedAmount = Amount * 1.5m;
        }
    }
}
