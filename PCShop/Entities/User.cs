using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCShop.Entities
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [MaxLength(36)]
        public string Id { get; set; }

        [MaxLength(30)]
        public string FirstName { get; set; }

        [MaxLength(30)]
        public string LastName { get; set; }

        [MaxLength(64)]
        [Required]
        public string Username { get; set; }

        [MaxLength(64)]
        [Required]
        public string Email { get; set; }

        //This will be hashed value, so no max lenght
        [Required]
        public string Password { get; set; }

        [Range(14,120)]
        public int Age { get; set; }
        public DateTime RegisterDate { get; set; }

        [MaxLength(10)]
        public string PhoneNumber { get; set; }

        public ICollection<Order> Orders { get; set; }

        public bool IsAdministrator { get; set; }
        //TODO
    }
}
