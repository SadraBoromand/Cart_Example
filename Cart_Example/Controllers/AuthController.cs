using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Cart_Example.Data;
using Cart_Example.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Cart_Example.Controllers
{
    public class AuthController : Controller
    {
        private MyContext _context;

        public AuthController(MyContext context)
        {
            _context = context;
        }

        [Route("Login")]
        public IActionResult Login(string ReturnUrl = "/")
        {
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(UserLogin login, [FromRoute] string ReturnUrl = "/")
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var user = _context.Users.Single(u => u.Email == login.Email && u.Password == login.Password);
            if (user == null)
            {
                ModelState.AddModelError("Email", "اطلاعات کاربری نادرست می باشد");
                return View(login);
            }


            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                new Claim(ClaimTypes.Name,user.Email),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principle = new ClaimsPrincipal(identity);

            var properties = new AuthenticationProperties()
            {
                IsPersistent = login.RemmberMe
            };

            await HttpContext.SignInAsync(principle, properties);

            return Redirect(ReturnUrl);

        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
    }
}
