using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Shop.Attributes;
using Shop.Models;

namespace Shop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly SiteContex _context;

        public ProductsController(SiteContex context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> IndexForAdmin()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId.HasValue)
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);

                if (user != null && user.Administrator)
                {
                    var products = await _context.Products.ToListAsync();
                    return View(products);
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [Authorize]
        public IActionResult Create()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId.HasValue)
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);

                if (user != null && (user.Entrepreneur || user.Administrator))
                {
                    return View();
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId.HasValue)
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);

                if (user != null && (user.Administrator))
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var product = await _context.Products.FindAsync(id);
                    if (product == null)
                    {
                        return NotFound();
                    }

                    return View(product);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {


            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId.HasValue)
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);

                if (user != null && (user.Administrator))
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var product = await _context.Products
                        .FirstOrDefaultAsync(m => m.Id == id);
                    if (product == null)
                    {
                        return NotFound();
                    }

                    return View(product);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [Authorize]
        public async Task<IActionResult> Buy(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId.HasValue)
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);

                if (user != null)
                {
                    var existingOrder = _context.Orders.FirstOrDefault(o => o.UserId == userId);
                    if (existingOrder == null)
                    {
                        var newOrder = new Order
                        {
                            UserId = (int)userId,
                            Status = "New",
                            Date = DateTime.Now,
                            DeliveryDate = DateTime.Now.AddDays(5)
                        };

                        _context.Orders.Add(newOrder);
                        await _context.SaveChangesAsync();
                    }

                    return RedirectToAction("PlaceOrder", "Orders", new { id = id });
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Products");
        }


        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
