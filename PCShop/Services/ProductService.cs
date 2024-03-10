using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using PCShop.Abstraction;
using PCShop.Data;
using PCShop.Entities;
using PCShop.Entities.Enums;

namespace PCShop.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }
        public Product Create(string name, string description, string model, int quantity, decimal price, string image, Category category)
        {
            var product = new Product 
            { 
                Name = name, 
                Description = description, 
                Model = model, 
                Quantity = quantity, 
                Price = price, 
                Image = image, 
                Category = category,
                AddedOn = DateTime.UtcNow,
                Discount = 0,
                IsDeleted = false
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            return product;
        }

        public Product Get(string id)
        {
            if(id is null)
            {
                throw new ArgumentNullException("Invalid ID: ID cannot be null");
            }

            return _context.Products
                .Where(x => !x.IsDeleted)
                .FirstOrDefault(x => x.Id == id);
        }

        public ICollection<Product> GetAll()
        {
            return _context.Products
                .Where(x => !x.IsDeleted)
                .ToList();
        }

        public ICollection<Product> GetAllByCategory(Category category)
        {
            return _context.Products
                .Where(x => x.Category == category && !x.IsDeleted)
                .ToList();
        }

        public Product Remove(string id)
        {
            var product = Get(id) ?? throw new InvalidOperationException();

            product.IsDeleted = true;
            _context.Products.Update(product);
            _context.SaveChanges();

            return product;
        }

        public Product RemoveDiscount(string id)
        {
            var product = Get(id) ?? throw new InvalidOperationException();

            product.Price += product.Discount;
            product.Discount = 0;

            _context.Products.Update(product);
            _context.SaveChanges();

            return product;
        }

        public Product SetDiscount(string id, int percentage)
        {
            var product = Get(id) ?? throw new InvalidOperationException();

            if (percentage < 0 || percentage > 100) 
            {
                throw new InvalidOperationException("Wrong percentage bro");
            }

            product.Discount = product.Price * percentage / 100;
            product.Price -= product.Discount;

            _context.Products.Update(product);
            _context.SaveChanges();

            return product;
        }

        public Product Update(string id, string name, string description, string model, int quantity, decimal price, string image, Category category)
        {
            var product = Get(id) ?? throw new InvalidOperationException();

            product.Name = name;
            product.Description = description;
            product.Model = model;
            product.Quantity = quantity;
            product.Price = price;
            product.Image = image;
            product.Category = category;

            _context.Products.Update(product);
            _context.SaveChanges();

            return product;
        }
    }
}
