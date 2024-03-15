using System.ComponentModel.DataAnnotations;

namespace PCShop.Models
{
    public class RegisterVM
    {
        [MaxLength(30)]
        public string FirstName { get; set; }
        [MaxLength(30)]
        public string LastName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
