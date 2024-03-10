using PCShop.Entities.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCShop.Entities
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [MaxLength(36)]
        public string Id { get; set; }
        public DateTime OrderedOn { get; set; }

        [Required]
        [MaxLength(36)]
        public string UserId { get; set; }
        public User User { get; set; }


        public OrderStatus Status { get; set; }

        [MaxLength(100)]
        public string Adress { get; set; }

        public decimal TotalPrice { get; set; }

    }
}
