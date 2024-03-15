using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PCShop.Abstraction;
using PCShop.Entities;
using PCShop.Entities.Enums;
using PCShop.Models;
using System.Data;
using System.Security.Claims;

namespace PCShop.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService orderService;
        private readonly UserManager<User> userManager;

        /// <summary>
        /// Initialize IOrderService and userManager
        /// </summary>
        /// <param name="_orderService"></param>
        /// <param name="userManager"></param>
        public OrdersController(IOrderService _orderService, UserManager<User> userManager)
        {
            orderService = _orderService;
            this.userManager = userManager;
        }


        /// <summary>
        /// Get all orders
        /// </summary>
        /// <returns>Return the view with model of OrderVM list</returns>
        [Authorize(Roles = "Employee,Admin")]
        public IActionResult All()
        {
            ViewData["Title"] = "Orders";

            var orders = orderService.GetOrders()
                .Select(x => new OrderVM
                {
                    Id = x.Id,
                    OrderedOn = x.OrderedOn,
                    Status = x.Status,
                    Address = x.Adress,
                    TotalPrice = x.TotalPrice,
                    OrderedProducts = x.OrderedProducts.Select(op => new OrderedProductVM()
                    {
                        Id = op.Id,
                        Count = op.Count,
                        DiscountApplied = op.DiscountApplied,
                        Price = op.Price,
                        Product = new ProductVM()
                        {
                            Id = op.ProductId,
                            Name = op.Product.Name,
                            Model = op.Product.Model,
                            Description = op.Product.Description,
                            Image = op.Product.Image,
                            Category = op.Product.Category,
                        }
                    }).ToList(),
                    User = new UserVM()
                    {
                        Id = x.UserId,
                        FirstName = x.User.FirstName,
                        LastName = x.User.LastName,
                        Email = x.User.Email,
                        Username = x.User.UserName,
                        PhoneNumber = x.User.PhoneNumber,
                    }

                }).OrderByDescending(x => x.Status == OrderStatus.Pending)
                .ThenByDescending(x => x.Status == OrderStatus.Approved)
                .ThenByDescending(x => x.Status == OrderStatus.Completed)
                .OrderByDescending(x => x.OrderedOn)
                .ToList();

            return View(orders);
        }


        /// <summary>
        /// Get my orders
        /// </summary>
        /// <returns>Return the view with model of OrderVM list</returns>
        [Authorize]
        public IActionResult My()
        {
            ViewData["Title"] = "MyOrders";

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null)
            {
                return BadRequest();
            }

            var orders = orderService.GetMyOrders(userId)
                .Select(x => new OrderVM
                {
                    Id = x.Id,
                    OrderedOn = x.OrderedOn,
                    Status = x.Status,
                    Address = x.Adress,
                    TotalPrice = x.TotalPrice,
                    OrderedProducts = x.OrderedProducts.Select(op => new OrderedProductVM()
                    {
                        Id = op.Id,
                        Count = op.Count,
                        DiscountApplied = op.DiscountApplied,
                        Price = op.Price,
                        Product = new ProductVM()
                        {
                            Id = op.ProductId,
                            Name = op.Product.Name,
                            Model = op.Product.Model,
                            Description = op.Product.Description,
                            Image = op.Product.Image,
                            Category = op.Product.Category,
                        }
                    }).ToList(),
                    User = new UserVM()
                    {
                        Id = x.UserId,
                        FirstName = x.User.FirstName,
                        LastName = x.User.LastName,
                        Email = x.User.Email,
                        Username = x.User.UserName,
                        PhoneNumber = x.User.PhoneNumber,
                    }

                }).OrderByDescending(x => x.Status == OrderStatus.Pending)
                .ThenByDescending(x => x.Status == OrderStatus.Approved)
                .ThenByDescending(x => x.Status == OrderStatus.Completed)
                .OrderByDescending(x => x.OrderedOn)
                .ToList();

            return View(nameof(All), orders);
        }

        /// <summary>
        /// Creates a new order
        /// </summary>
        /// <param name="orderVM"></param>
        /// <returns>Success: Redirect to my orders page, Failed: return BadRequest</returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync([FromBody] OrderVM orderVM)
        {
            var user = await userManager.GetUserAsync(User);

            try
            {
                var order = orderService.CreateOrder(user.Id, orderVM.Address, orderVM.OrderedProducts.Select(x => new OrderedProduct()
                {
                    Id = x.Id,
                    DiscountApplied = x.DiscountApplied,
                    Count = x.Count,
                    OrderId = orderVM.Id,
                    Price = x.Price,
                    ProductId = x.Product.Id,
                }).ToList());


                return RedirectToAction(nameof(My));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Edit the status of the order
        /// </summary>
        /// <param name="orderVM"></param>
        /// <returns>Success: Redirect to all orders page, Failed: return BadRequest</returns>
        [Authorize(Roles = "Employee,Admin")]
        [HttpPost]
        public IActionResult EditStatus(OrderVM orderVM)
        {
            try
            {
                var order = orderService.ChangeStatus(orderVM.Id, orderVM.Status);

                if (order is not null)
                {
                    return RedirectToAction(nameof(All));
                }

                return BadRequest("Order cannot be edited!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Shopping cart page
        /// </summary>
        /// <returns>The view of shopping cart page</returns>
        public IActionResult ShoppingCart()
        {
            return View();
        }
    }
}
