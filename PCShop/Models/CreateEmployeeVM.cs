using System.ComponentModel.DataAnnotations;

namespace PCShop.Models
{
    public class CreateEmployeeVM
    {
        [MaxLength(30)]
        public string FirstName { get; set; }
        [MaxLength(30)]
        public string LastName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
