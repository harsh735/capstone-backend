using System.ComponentModel.DataAnnotations;

namespace Authentication1.Models
{
    public class User
    {
        public int UserID { get; set; } = 1006;

        public int? RoleID { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string? AdvisorID { get; set; }

        public string? AgentID { get; set; }

        public string? ClientID { get; set; }


        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string Company { get; set; }

        public string? SortName { get; set; }

        public byte Active { get; set; } = 1;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool? DeletedFlag { get; set; } = false;

        [Required]
        public string Password { get; set; }
    }

}