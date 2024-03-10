using PCShop.Entities;
using PCShop.Entities.Enums;

namespace PCShop.Abstraction
{
    public interface IProductService
    {
        public ICollection<Product> GetAll();
        public ICollection<Product> GetAllByCategory(Category category);
        public Product SetDiscount(string id, decimal discount);
        public Product RemoveDiscount(string id, int percentige);
        public Product Get(string id);
        public Product Remove(string id);
        public Product Create(string name,
            string description,
            string model,
            int quantity,
            decimal price,
            string image,
            Category category);
        public Product Update(string id,
            string name,
            string description,
            string model,
            int quantity,
            decimal price,
            string image,
            Category category);

        //TODO
        //public ICollection<Product> SearchBy();
        //public ICollection<Product> GetBestSellers();
    }
}
