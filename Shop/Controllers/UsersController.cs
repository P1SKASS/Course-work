using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shop.Attributes;
using Shop.Models;

namespace Shop.Controllers
{

    public class UsersController : Controller
    {
        private readonly SiteContex _context;


        public UsersController(SiteContex context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound();
            }

            if (!user.Administrator)
            {
                id = userId;
            }

            if (id == null)
            {
                return NotFound();
            }

            var detailsUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (detailsUser == null)
            {
                return NotFound();
            }

            return View(detailsUser);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Login,Mail,Password,Entrepreneur")] User user)
        {
            if (ModelState.IsValid)
            {
                var existingUserWithLogin = await _context.Users.FirstOrDefaultAsync(u => u.Login == user.Login);
                if (existingUserWithLogin != null)
                {
                    ModelState.AddModelError("Login", "A user with this login already exists.");
                    return View(user);
                }

                var existingUserWithEmail = await _context.Users.FirstOrDefaultAsync(u => u.Mail == user.Mail);
                if (existingUserWithEmail != null)
                {
                    ModelState.AddModelError("Mail", "A user with this email already exists.");
                    return View(user);
                }

                if (user.Password.Length < 8 || !char.IsUpper(user.Password[0]))
                {
                    ModelState.AddModelError("Password", "Password must be at least 8 characters long and start with an uppercase letter.");
                    return View(user);
                }

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Products");
            }

            return View(user);
        }

        [Authorize]
        public async Task<IActionResult> Edit()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Login,Mail,Password,Entrepreneur")] User user)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId != user.Id)
            {
                return NotFound();
            }

            var existingUserWithLogin = await _context.Users.FirstOrDefaultAsync(u => u.Login == user.Login && u.Id != user.Id);
            if (existingUserWithLogin != null)
            {
                ModelState.AddModelError("Login", "A user with this login already exists.");
                return View(user);
            }

            var existingUserWithEmail = await _context.Users.FirstOrDefaultAsync(u => u.Mail == user.Mail && u.Id != user.Id);
            if (existingUserWithEmail != null)
            {
                ModelState.AddModelError("Mail", "A user with this email already exists.");
                return View(user);
            }

            if (user.Password.Length < 8 || !char.IsUpper(user.Password[0]))
            {
                ModelState.AddModelError("Password", "Password must be at least 8 characters long and start with an uppercase letter.");
                return View(user);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = user.Id });
            }
            return View(user);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}