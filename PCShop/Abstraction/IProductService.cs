using PCShop.Entities;
using PCShop.Entities.Enums;

namespace PCShop.Abstraction
{
    public interface IProductService
    {
        public ICollection<Product> GetAll();
        public ICollection<Product> GetAllByCategory(string category);
        public Product SetDiscount(string id, int percentage);
        public Product RemoveDiscount(string id);
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

        public ICollection<Product> Search(string filter, int minPrice, int maxPrice, string name, string model, IEnumerable<Product> oldProducts);
        //public ICollection<Product> GetBestSellers();
    }
}
