using PCShop.Entities;
using PCShop.Entities.Enums;

namespace PCShop.Abstraction
{
    public interface IOrderService
    {
        public ICollection<Order> GetOrders();
        public ICollection<Order> GetMyOrders(string userId);
        public Order GetOrder(string id);
        public Order CreateOrder(string userId, string address, ICollection<OrderedProduct> orderedProducts);
        public Order ChangeStatus(string id, OrderStatus status);
    }
}
