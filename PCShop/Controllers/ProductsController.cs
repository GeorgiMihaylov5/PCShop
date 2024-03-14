using Microsoft.AspNetCore.Authorization;
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

        [Authorize("Employee")]
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

        [Authorize("Employee")]
        public IActionResult Create()
        {
            ViewData["Title"] = "Create";

            return View();
        }

        [Authorize("Employee")]
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

        [Authorize("Employee")]
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

        [Authorize("Employee")]
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

        [Authorize("Employee")]
        public IActionResult Delete(string id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            _productService.Remove(id);

            return RedirectToAction(nameof(AllTable));
        }

        [Authorize("Employee")]
        public IActionResult AddDiscount(string id)
        {
            _productService.SetDiscount(id, 5);

            return RedirectToAction("AllTable");
        }

        [Authorize("Employee")]
        public IActionResult RemoveDiscount(string id)
        {
            _productService.RemoveDiscount(id);

            return RedirectToAction("AllTable");
        }
    }
}
