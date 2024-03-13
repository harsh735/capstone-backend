using System.ComponentModel.DataAnnotations;

namespace Authentication1.Models
{
    public class Subscriptions
    {
        [Key]
        public int? UserID { get; set; }
        public int? SubscriptionID { get; set; }
        public string? SubscriptionName { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int? Price { get; set; }
        public bool? Active { get; set; }
    }
}
