using PCShop.Entities.Enums;

namespace PCShop.Models
{
    public class ProductVM
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Model { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal Discount { get; set; }

        public string Image { get; set; }
        public DateTime AddedOn { get; set; }

        public Category Category { get; set; }
    }
}
