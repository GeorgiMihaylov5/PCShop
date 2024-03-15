using System.ComponentModel.DataAnnotations;

namespace PCShop.Models
{
    public class OrderedProductVM
    {
        public string Id { get; set; }
        public virtual ProductVM Product { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Count { get; set; }
        public bool DiscountApplied { get; set; }
    }
}
