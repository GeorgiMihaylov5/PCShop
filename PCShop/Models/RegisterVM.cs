using System.ComponentModel.DataAnnotations;

namespace PCShop.Models
{
    public class RegisterVM : CreateEmployeeVM
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
