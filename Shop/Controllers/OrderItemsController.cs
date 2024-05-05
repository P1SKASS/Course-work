using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Attributes;
using Shop.Models;

namespace Shop.Controllers
{
    public class OrderItemsController : Controller
    {
        private readonly SiteContex _context;

        public OrderItemsController(SiteContex context)
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

            var userOrderItems = await _context.OrderItems
                .Include(oi => oi.Order)
                .Include(oi => oi.Product)
                .Where(oi => oi.Order.UserId == userId)
                .ToListAsync();

            return View(userOrderItems);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems
                .FirstOrDefaultAsync(m => m.Id == id);

            if (orderItem == null)
            {
                return NotFound();
            }

            if (orderItem.Quantity <= 1)
            {
                _context.OrderItems.Remove(orderItem);
            }
            else
            {
                orderItem.Quantity -= 1;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
