using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication1.Models
{
    public class GoldInvestments
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? GoldID { get; set; }

        [ForeignKey("Users")]
        public int? UserID { get; set; }

        [ForeignKey("InvestmentInfo")]
        public int? InvestorInfoID { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string? Type { get; set; }
        public int? Timeframe { get; set; }
        public string? Risk { get; set; }

        public string? Frequency { get; set; }
        public decimal? ExpectedAmount { get; set; }
    }
}
