using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    public class BasketController : Controller
    {
        private readonly SiteContex _context;

        public BasketController(SiteContex context)
        {
            _context = context;
        }

        public async Task<IActionResult> BasketAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var userCart = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId && o.Status == "Cart")
                .FirstOrDefaultAsync();

            if (userCart != null)
            {
                return View(userCart.OrderItems);
            }

            ViewData["Message"] = "Your cart is empty.";
            return View(new List<OrderItem>());
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem != null)
            {
                _context.OrderItems.Remove(orderItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Basket");
        }

        public IActionResult Checkout()
        {
            return RedirectToAction("OrderDetails");
        }
    }
}
