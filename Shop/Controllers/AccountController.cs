using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Models;
using System;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    public class AccountController : Controller
    {
        private readonly SiteContex _context;

        public AccountController(SiteContex context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string login, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserLogin", user.Login);
                return RedirectToAction("Index", "Products");
            }

            TempData["ErrorMessage"] = "Wrong login or password.";
            return RedirectToAction("Login");
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.SetString("UserLogin", "Log in");
            return RedirectToAction("Login");
        }
    }
}