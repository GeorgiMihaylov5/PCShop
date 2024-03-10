using Microsoft.AspNetCore.Mvc;
using PCShop.Abstraction;
using PCShop.Models;

namespace PCShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult GetAll()
        {
            var products = _productService.GetAll()
                .Select(p => new AllProductsVM()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Model = p.Model,
                    Description = p.Description,
                    Price = p.Price,
                    Discount = p.Discount,
                    AddedOn = p.AddedOn,
                    Quantity = p.Quantity,
                    Image = p.Image,
                    Category = p.Category,
                });

            return View(products);
        }

        public IActionResult GetAllByCategory(string category)
        {
            return View();
        }
    }
}
