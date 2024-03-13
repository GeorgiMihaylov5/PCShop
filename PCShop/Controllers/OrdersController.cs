﻿using Microsoft.AspNetCore.Mvc;
using PCShop.Abstraction;
using PCShop.Entities;
using PCShop.Entities.Enums;
using PCShop.Models;
using System.Security.Claims;

namespace PCShop.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService _orderService)
        {
            orderService = _orderService;
        }

        public IActionResult All()
        {
            ViewData["Title"] = "Orders";

            var orders = orderService.GetOrders()
                .Select(x => new OrderVM
                {
                    Id = x.Id,
                    OrderedOn = x.OrderedOn,
                    Status = x.Status,
                    Adress = x.Adress,
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
                .ThenByDescending(x => x.Status == OrderStatus.Completed).ToList();

            return View(orders);
        }

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
                    Adress = x.Adress,
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
                .ThenByDescending(x => x.Status == OrderStatus.Completed).ToList();

            return View(nameof(All), orders);
        }

        [HttpPost]
        public IActionResult CreateOrder(OrderVM orderVM)
        {
            try
            {
                var order = orderService.CreateOrder(orderVM.User.Id,orderVM.Adress, orderVM.OrderedProducts.Select(x => new OrderedProduct() 
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

        [HttpPut]
        public IActionResult EditStatus(OrderVM orderVM)
        {
            try
            {
                var order = orderService.ChangeStatus(orderVM.Id, orderVM.Status);

                if (order is not null)
                {
                    return Ok();
                }

                return BadRequest("Order cannot be edited!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
