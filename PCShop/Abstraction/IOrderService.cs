using PCShop.Entities.Enums;
using PCShop.Entities;

namespace PCShop.Abstraction
{
    public interface IOrderService
    {
        public ICollection<Order> GetOrders();
        public ICollection<Order> GetMyOrders(string userId);
        public Order GetOrder(string id);
        public Order CreateOrder(string userId, string address, ICollection<OrderedProduct> orderedProducts);
        //public bool CreateOrderedProduct(string productId, string orderId, decimal price, int count);
        //public bool EditOrderedProduct(string id, int count);
        public Order ChangeStatus(string id, OrderStatus status);

        //TODO change address
    }
}
