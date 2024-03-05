using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCShop.Entities
{
    public class OrderedProduct
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [MaxLength(36)]
        public string Id { get; set; }

        [Required]
        [MaxLength(36)]
        public string ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Required]
        [MaxLength(36)]
        public string OrderId { get; set; }
        public virtual Order Order { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Count { get; set; }
        public bool DiscountApplied { get; set; }
        
    }
}
