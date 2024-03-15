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

        /// <summary>
        /// Initialize IProductService
        /// </summary>
        /// <param name="productService"></param>
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns>Return all products as list of ProductVM</returns>
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

        /// <summary>
        /// Search in all products page
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <param name="name"></param>
        /// <param name="model"></param>
        /// <returns>Return filtered list of ProductVM</returns>
        [HttpPost]
        public IActionResult All(string filter, int minPrice, int maxPrice, string name, string model)
        {
            var oldProducts = _productService.GetAll();
           
            return Search(filter, minPrice,maxPrice, name, model, oldProducts);
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns>Return the products in page that has a table view</returns>
        [Authorize(Roles = "Admin")]
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

        /// <summary>
        /// Get all products by category
        /// </summary>
        /// <param name="category"></param>
        /// <returns>Return products as list of ProductVM</returns>
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

        /// <summary>
        /// Search in some category products page
        /// </summary>
        /// <param name="category"></param>
        /// <param name="filter"></param>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <param name="name"></param>
        /// <param name="model"></param>
        /// <returns>Return filtered products</returns>
        [HttpPost]
        [Route("[controller]/[action]/{category}")]
        public IActionResult AllByCategory(string category, string filter, int minPrice, int maxPrice, string name, string model)
        {
            var oldProducts = _productService.GetAllByCategory(category);

           return Search(filter, minPrice, maxPrice, name, model, oldProducts);
        }

        /// <summary>
        /// Get all products with discount
        /// </summary>
        /// <returns>Return list of ProductVM with discount</returns>
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

        /// <summary>
        /// Search in discount page
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <param name="name"></param>
        /// <param name="model"></param>
        /// <returns>Return filtered products with discount</returns>
        [HttpPost]
        public IActionResult AllDiscounts(string filter, int minPrice, int maxPrice, string name, string model)
        {
            var oldProducts = _productService.GetAll()
                 .Where(x => x.Discount > 0)
                 .ToList();

           return Search(filter, minPrice, maxPrice, name, model, oldProducts);
        }

        /// <summary>
        /// Get the product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success: Return product details page with model of ProductVM, Failed: return BadRequest</returns>
        public IActionResult Details(string id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var product = _productService.Get(id);

            if (product is null)
            {
                return BadRequest();
            }

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

        /// <summary>
        /// Create product page
        /// </summary>
        /// <returns>Return product page</returns>
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["Title"] = "Create";

            return View();
        }

        /// <summary>
        /// Create product
        /// </summary>
        /// <param name="productVM"></param>
        /// <returns>Success: Redirect to AllTable, Failed: return BadRequest</returns>
        [Authorize(Roles = "Admin")]
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


        /// <summary>
        /// Edit product page
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success: return edit page with product model, Failed: return BadRequest</returns>
        [Authorize(Roles = "Admin")]
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

        /// <summary>
        /// Edit product
        /// </summary>
        /// <param name="productVM"></param>
        /// <returns>Success: Redirect to AllTable, Failed: return BadRequest</returns>
        [Authorize(Roles = "Admin")]
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

        /// <summary>
        /// Delete product
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success: Redirect to AllTable, Failed: return BadRequest</returns>
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            _productService.Remove(id);

            return RedirectToAction(nameof(AllTable));
        }

        /// <summary>
        /// Add 5% discount on product
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Redirect to AllTable</returns>
        [Authorize(Roles = "Admin")]
        public IActionResult AddDiscount(string id)
        {
            _productService.SetDiscount(id, 5);

            return RedirectToAction("AllTable");
        }

        /// <summary>
        /// Remove product discount
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Redirect to AllTable</returns>
        [Authorize(Roles = "Admin")]
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
