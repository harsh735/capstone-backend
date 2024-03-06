using System.ComponentModel.DataAnnotations;

namespace Authentication1.Models
{
    public class InvestmentModel
    {
        public int InvestorInfoID { get; set; }
        public int InvestmentTypeID { get; set; }
        public decimal Amount { get; set; }

        [Key]
        public int UserID { get; set; }
        public string InvestmentName { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public bool DeleteFlag { get; set; }
        public string InvestmentType { get; set; }
    }
}
