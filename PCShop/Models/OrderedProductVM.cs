using PCShop.Entities;

namespace PCShop.Models
{
    public class OrderedProductVM
    {
        public string Id { get; set; }
        public virtual ProductVM Product { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public bool DiscountApplied { get; set; }
    }
}
