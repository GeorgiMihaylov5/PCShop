using PCShop.Abstraction;
using PCShop.Data;
using PCShop.Entities;
using PCShop.Entities.Enums;
using PCShop.Exceptions;

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
            if (id is null)
            {
                throw new ArgumentNullException("User id is null!");
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

        public ICollection<Product> GetAllByCategory(string category)
        {
            return _context.Products
                .Where(x => x.Category == (Category)Enum.Parse(typeof(Category), category, true) && !x.IsDeleted)
                .ToList();
        }

        public Product Remove(string id)
        {
            var product = Get(id) ?? throw new ProductNotFoundException();

            product.IsDeleted = true;
            _context.Products.Update(product);
            _context.SaveChanges();

            return product;
        }

        public Product RemoveDiscount(string id)
        {
            var product = Get(id) ?? throw new ProductNotFoundException();

            product.Price += product.Discount;
            product.Discount = 0;

            _context.Products.Update(product);
            _context.SaveChanges();

            return product;
        }

        public Product SetDiscount(string id, int percentage)
        {
            var product = Get(id) ?? throw new ProductNotFoundException();

            if (product.Discount != 0)
            {
                product.Price += product.Discount;
                percentage += (int)(product.Discount * 100 / product.Price);
            }

            if (percentage < 0 && percentage > 100)
            {
                throw new InvalidDiscountException();
            }

            product.Discount = product.Price * percentage / 100;
            product.Price -= product.Discount;

            _context.Products.Update(product);
            _context.SaveChanges();

            return product;
        }

        public Product Update(string id, string name, string description, string model, int quantity, decimal price, string image, Category category)
        {
            var product = Get(id) ?? throw new ProductNotFoundException();

            product.Name = name;
            product.Description = description;
            product.Model = model;
            product.Quantity = quantity;
            product.Price = price;
            product.Image = image;
            product.Category = category;
            product.Discount = 0;

            _context.Products.Update(product);
            _context.SaveChanges();

            return product;
        }

        public ICollection<Product> Search(string filter, int minPrice, int maxPrice, string name, string model, IEnumerable<Product> oldProducts)
        {
            var products = new List<Product>();

            if (name is not null)
            {
                var currentProducts = oldProducts.Where(x => x.Name.ToLower().StartsWith(name?.ToLower())).ToList();
                products.AddRange(currentProducts);
            }
            else
            {
                products = oldProducts.ToList();
            }
            if (model is not null)
            {
                products = products.Where(x => x.Model.ToLower().StartsWith(model?.ToLower())).Distinct().ToList();
            }

            if (minPrice > 0 && maxPrice > minPrice)
            {
                products = products.Where(x => x.Price >= minPrice && x.Price <= maxPrice).ToList();
            }
            else if (minPrice == maxPrice && minPrice > 0)
            {
                products = products.Where(x => x.Price == minPrice).ToList();
            }
            else if (minPrice > 0 && maxPrice < minPrice)
            {
                products = products.Where(x => x.Price >= minPrice).ToList();
            }
            else if (maxPrice > 0)
            {
                products = products.Where(x => x.Price <= maxPrice).ToList();
            }

            if (filter == "1")
            {
                products = products.Where(x => x.Discount != 0).ToList();
            }
            else if (filter == "2")
            {
                products = products.OrderBy(x => x.Price).ToList();
            }

            return products;
        }
    }
}
