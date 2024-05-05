using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shop.Models;
using Shop.Attributes;

namespace Shop.Controllers
{
    public class OrdersController : Controller
    {
        private readonly SiteContex _context;

        public OrdersController(SiteContex context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {

            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var orderUser = await _context.Orders
                .Where(oi => oi.UserId == userId)
                .ToListAsync();

            foreach (var order in orderUser)
            {
                order.TotalPrice = _context.OrderItems
                    .Where(oi => oi.OrderId == order.Id)
                    .Sum(oi => oi.Quantity * oi.Product.Price);
            }

            await _context.SaveChangesAsync();

            return View(orderUser);
        }

        [Authorize]
        public async Task<IActionResult> PlaceOrder(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.UserId == userId);

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (order == null)
                {
                    order = new Order
                    {
                        UserId = userId.Value,
                        Status = "Processing",
                        Date = DateTime.Now,
                        DeliveryDate = DateTime.Now.AddDays(5),
                        PostOffice = "Not Indicated",
                        TotalPrice = 0,
                        CreditCardId = 0
                    };
                    _context.Orders.Add(order);
                }
                else
                {
                    order.Status = "Processing";
                }

                var productInOrder = await _context.OrderItems.FirstOrDefaultAsync(p => p.ProductId == id && p.Order.UserId == userId);

                if (productInOrder != null)
                {
                    productInOrder.Quantity += 1;
                }
                else
                {
                    var newOrderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = id,
                        Quantity = 1,
                        UnitPrice = product.Price
                    };

                    _context.OrderItems.Add(newOrderItem);
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Products");
        }

        [Authorize]
        public async Task<IActionResult> CompletionOrder()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var order = await _context.Orders.FirstOrDefaultAsync(o => o.UserId == userId);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CompletionOrder(Order order, string postOffice, string creditCardNumber, string expiryDate, string cvv)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var existingOrder = await _context.Orders
                .Include(o => o.CreditCard)
                .FirstOrDefaultAsync(o => o.UserId == userId);

            if (existingOrder == null)
            {
                return NotFound();
            }

            if (existingOrder.CreditCard != null)
            {
                existingOrder.CreditCard.CardNumber = creditCardNumber;
                existingOrder.CreditCard.ExpiryDate = expiryDate;
                existingOrder.CreditCard.CVV = cvv;
            }
            else
            {
                var creditCard = new CreditCard
                {
                    CardNumber = creditCardNumber,
                    ExpiryDate = expiryDate,
                    CVV = cvv
                };

                _context.CreditCards.Add(creditCard);
                existingOrder.CreditCard = creditCard;
            }

            existingOrder.PostOffice = postOffice;
            existingOrder.Status = "En route";

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Products");
        }
    }
}
