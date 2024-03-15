using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCShop.Abstraction;
using PCShop.Entities;
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
                .Select(p => new ProductVM()
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

        [HttpPost]
        public IActionResult All(string filter, int minPrice, int maxPrice, string name, string model)
        {
            var oldProducts = _productService.GetAll();
           
            return Search(filter, minPrice,maxPrice, name, model, oldProducts);
        }

        [Authorize(Roles = "Employee")]
        public IActionResult AllTable()
        {
            var products = _productService.GetAll()
                .Select(p => new ProductVM()
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

        [Route("[controller]/[action]/{category}")]
        public IActionResult AllByCategory(string category)
        {
            var products = _productService.GetAllByCategory(category)
                 .Select(p => new ProductVM()
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

            return View(nameof(All), products);
        }

        [HttpPost]
        [Route("[controller]/[action]/{category}")]
        public IActionResult AllByCategory(string category, string filter, int minPrice, int maxPrice, string name, string model)
        {
            var oldProducts = _productService.GetAllByCategory(category);

           return Search(filter, minPrice, maxPrice, name, model, oldProducts);
        }

        public IActionResult AllDiscounts()
        {
            var products = _productService.GetAll()
                 .Where(x => x.Discount > 0)
                 .Select(p => new ProductVM()
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

            return View(nameof(All), products);
        }

        [HttpPost]
        public IActionResult AllDiscounts(string filter, int minPrice, int maxPrice, string name, string model)
        {
            var oldProducts = _productService.GetAll()
                 .Where(x => x.Discount > 0)
                 .ToList();

           return Search(filter, minPrice, maxPrice, name, model, oldProducts);
        }

        public IActionResult Details(string id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var product = _productService.Get(id) ?? throw new ArgumentException();

            return View(new ProductVM()
                {
                    Id = id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Discount = product.Discount,
                    AddedOn = product.AddedOn,
                    Category = product.Category,
                    Image = product.Image,
                    Model = product.Model,
                    Quantity = product.Quantity,
                });
        }

        [Authorize(Roles = "Employee")]
        public IActionResult Create()
        {
            ViewData["Title"] = "Create";

            return View();
        }

        [Authorize(Roles = "Employee")]
        [HttpPost]
        public IActionResult Create(ProductVM productVM)
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

            return RedirectToAction(nameof(AllTable));
        }

        [Authorize(Roles = "Employee")]
        public IActionResult Edit(string id)
        {
            ViewData["Title"] = "Edit";

            if (id is null)
            {
                return BadRequest();
            }

            var product = _productService.Get(id);

            return View(nameof(Create),
                new ProductVM() 
                { 
                    Id = id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Discount = product.Discount,
                    AddedOn = product.AddedOn,
                    Category = product.Category,
                    Image = product.Image,
                    Model = product.Model,
                    Quantity = product.Quantity,
                });
        }

        [Authorize(Roles = "Employee")]
        [HttpPost]
        public IActionResult Edit(ProductVM productVM)
        {
            if (productVM is null)
            {
                return BadRequest();
            }

            var product = _productService.Update(productVM.Id,
                productVM.Name,
                productVM.Description,
                productVM.Model,
                productVM.Quantity,
                productVM.Price,
                productVM.Image,
                productVM.Category);

            return RedirectToAction(nameof(AllTable));
        }

        [Authorize(Roles = "Employee")]
        public IActionResult Delete(string id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            _productService.Remove(id);

            return RedirectToAction(nameof(AllTable));
        }

        [Authorize(Roles = "Employee")]
        public IActionResult AddDiscount(string id)
        {
            _productService.SetDiscount(id, 5);

            return RedirectToAction("AllTable");
        }

        [Authorize(Roles = "Employee")]
        public IActionResult RemoveDiscount(string id)
        {
            _productService.RemoveDiscount(id);

            return RedirectToAction("AllTable");
        }

        private IActionResult Search(string filter, int minPrice, int maxPrice, string name, string model, ICollection<Product> oldProducts)
        {
            var productsVm = _productService.Search(filter, minPrice, maxPrice, name, model, oldProducts)
                        .Select(p => new ProductVM
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
                        }).ToList();

            return View(nameof(All), productsVm);
        }
    }
}
