using EntityFramework.Exceptions.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Study_Tracker.Data;
using Study_Tracker.Models;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Study_Tracker.Controllers
{
    public class AccountsController : Controller
    {

        private readonly Study_TrackerContext context;

        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public AccountsController(Study_TrackerContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {

            var userDB = await (context.User.FirstOrDefaultAsync(b => b.username == user.username && b.password == ComputeSha256Hash(user.password)));

            if (userDB != null)
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, userDB.username),
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = false
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);

                return RedirectToAction("Index", "Modules");
            }

            ViewData["ValidateMessage"] = "User not found!";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {

            user.password = ComputeSha256Hash(user.password);
            if (ModelState.IsValid)
            {
                context.Add(user);

                try
                {
                    int response = await context.SaveChangesAsync();

                }
                catch (UniqueConstraintException ex)
                {
                    ViewData["ValidateMessage"] = ex.Message;
                    return View();
                }

                // Log In User
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.username),
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = true,
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);

                return RedirectToAction("Index", "Modules");
            }
            
            
           return View();
        }

        private static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public IActionResult Register()
        {
            return View();
        }
    }
}
