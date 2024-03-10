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

        public IActionResult All()
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

        public IActionResult AllByCategory(string category)
        {
            var products = _productService.GetAllByCategory(category)
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

            return View("All", products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AllProductsVM productVM)
        {
            if (productVM is null)
            {
                return BadRequest();
            }

            var product = _productService.Create(productVM.Name,
                productVM.Description,
                productVM.Model,
                productVM.Quantity,
                productVM.Price,
                productVM.Image,
                productVM.Category);

            return View("Details", product);
        }
    }
}
