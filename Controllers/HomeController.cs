using Azure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Study_Tracker.Data;
using Study_Tracker.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace Study_Tracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly Study_TrackerContext context;

        public HomeController(Study_TrackerContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            
            List<Module> modules = await context.Module.Where(a=> a.user.username == User.FindFirstValue(ClaimTypes.NameIdentifier)).ToListAsync();


            return View(modules);
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login","Accounts");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}