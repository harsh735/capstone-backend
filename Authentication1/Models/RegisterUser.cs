using System.ComponentModel.DataAnnotations;

namespace Authentication1.Models
{
    public class RegisterUser
    {
        public int UserID { get; set; }
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Key]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Company { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Address { get; set; }
        public bool DeletedFlag { get; set; } = false;
        public byte Active { get; set; } = 1;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}
