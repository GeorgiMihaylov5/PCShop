using PCShop.Entities.Enums;
using PCShop.Entities;

namespace PCShop.Abstraction
{
    public interface IOrderService
    {
        public ICollection<Order> GetOrders();
        public ICollection<Order> GetMyOrders(string username);
        public Order GetOrder(string id);
        public Order CreateOrder(string username, string address, ICollection<OrderedProduct> orderedProducts);
        //public bool CreateOrderedProduct(string productId, string orderId, decimal price, int count);
        //public bool EditOrderedProduct(string id, int count);
        public bool EditOrder(string id, OrderStatus status, string notes);

        //TODO change address
    }
}
