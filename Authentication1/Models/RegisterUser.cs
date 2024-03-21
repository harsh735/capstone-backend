using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication1.Models
{
    public class RegisterUser
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int UserID { get; set; }

        public int? RoleID { get; set; }


        public int? AdvisorID { get; set; }


        [Required]
        public string FirstName { get; set; }

        public string? LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string? Password { get; set; }

        public string? Phone { get; set; }

        public string? Company { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Address { get; set; }
        public bool? DeletedFlag { get; set; } = false;
        public byte? Active { get; set; } = 1;
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; }

        public string? ModifiedBy { get; set; }

    }
}
