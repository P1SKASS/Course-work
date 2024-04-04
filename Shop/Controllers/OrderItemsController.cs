using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IActionResult> Index()
        {
            var siteContex = _context.OrderItems.Include(o => o.Order).Include(o => o.Product);
            return View(await siteContex.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems
                .Include(o => o.Order)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderId,ProductId,Quantity,UnitPrice,Date,DeliveryDate")] OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {

                _context.Add(orderItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", orderItem.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", orderItem.ProductId);
            return View(orderItem);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", orderItem.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", orderItem.ProductId);
            return View(orderItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderId,ProductId,Quantity,UnitPrice,Date,DeliveryDate")] OrderItem orderItem)
        {
            if (id != orderItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderItemExists(orderItem.Id))
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
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", orderItem.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", orderItem.ProductId);
            return View(orderItem);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems
                .Include(o => o.Order)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem != null)
            {
                _context.OrderItems.Remove(orderItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderItemExists(int id)
        {
            return _context.OrderItems.Any(e => e.Id == id);
        }
    }
}
