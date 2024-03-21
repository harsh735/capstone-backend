using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication1.Models
{
    public class AdvisorPlan
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? InvestorInfoID { get; set; }
        public int? UserID { get; set; }
        public int? AdvisorID { get; set; }

        public string? InvestmentName { get; set; }
        public string? InvestmentType { get; set; }
        public decimal? Amount { get; set; }
        public string? Risk { get; set; }
        public string? Status { get; set; }
        public DateTime? StartDate { get; set; }
    }
}
