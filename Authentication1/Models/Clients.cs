using System.ComponentModel.DataAnnotations;

namespace Authentication1.Models
{
    public class Clients
    {
        public int? RoleID { get; set; }

        [Key]
        public int? UserID { get; set; }
        public int? AdvisorID { get; set; }
        public string? ClientName { get; set; }
        public decimal? PortfolioValue { get; set; }
        public DateTime? SubscribedOn { get; set; }
        public string? TypeOfInvestment { get; set; }
    }
}
