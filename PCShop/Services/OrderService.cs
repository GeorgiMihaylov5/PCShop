﻿using Microsoft.EntityFrameworkCore;
using PCShop.Abstraction;
using PCShop.Data;
using PCShop.Entities;
using PCShop.Entities.Enums;

namespace PCShop.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public Order CreateOrder(string userId, string address, ICollection<OrderedProduct> orderedProducts)
        {
            var order = new Order
            {
                UserId = userId,
                Adress = address,
                OrderedOn = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                TotalPrice = orderedProducts.Sum(p => p.Price)
            };

            _context.Orders.Add(order);

            foreach (var item in orderedProducts)
            {
                var orderedProduct = new OrderedProduct
                {
                    DiscountApplied = item.DiscountApplied,
                    Count = item.Count,
                    OrderId = order.Id,
                    Price = item.Price,
                    ProductId = item.ProductId
                };

                _context.OrderedProducts.Add(orderedProduct);
            }

            _context.SaveChanges();
            return order;
        }

        public Order ChangeStatus(string id, OrderStatus status)
        {
            var order = GetOrder(id) ?? throw new InvalidOperationException();

            order.Status = status;

            _context.Orders.Update(order);
            _context.SaveChanges();

            return order;
        }

        public ICollection<Order> GetMyOrders(string userId)
        {
            if (userId is null)
            {
                throw new ArgumentNullException("Invalid user ID: ID cannot be null");
            }

            return _context.Orders
                .Where(_context => _context.UserId == userId)
                .Include(x => x.OrderedProducts)
                .ToList();
        }

        public Order GetOrder(string id)
        {
            if (id is null)
            {
                throw new ArgumentNullException("Invalid order ID: ID cannot be null");
            }

            return _context.Orders
                .Include(x => x.OrderedProducts)
                .FirstOrDefault(x => x.Id == id);
        }

        public ICollection<Order> GetOrders()
        {
            return _context.Orders
                .Include(x => x.OrderedProducts)
                .ToList();
        }
    }
}
