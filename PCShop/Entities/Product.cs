using PCShop.Entities.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCShop.Entities
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [MaxLength(36)]
        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        [Required]
        [MaxLength(100)]
        public string Model { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        public decimal Discount { get; set; }

        [MaxLength(200)]
        public string Image { get; set; }
        public DateTime AddedOn { get; set; }
        public bool IsDeleted { get; set; }

        [Required]
        public Category Category { get; set; }
    }
}
