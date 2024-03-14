using PCShop.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace PCShop.Models
{
    public class OrderVM
    {
        [MaxLength(36)]
        public string Id { get; set; }
        public DateTime OrderedOn { get; set; }

        public UserVM User { get; set; }

        public OrderStatus Status { get; set; }

        [MaxLength(100)]
        public string Address { get; set; }

        public decimal TotalPrice { get; set; }

        public ICollection<OrderedProductVM> OrderedProducts { get; set; }
    }
}
