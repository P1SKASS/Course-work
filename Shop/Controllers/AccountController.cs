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
            var users = _context.Users.Where(u => u.Login == login).ToList();

            foreach (var user in users)
            {
                if (user.Password == password)
                {
                    return RedirectToAction("Index", "Products");
                }
            }

            TempData["ErrorMessage"] = "Wrong login or password.";
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }
    }
}