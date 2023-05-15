using DieffeClean.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DieffeClean.Domain.Constants;
using Microsoft.AspNetCore.Authorization;

namespace DieffeClean.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [Authorize(Roles = Roles.SuperAdmin + ","+ Roles.Admin + ","+ Roles.CleaningUser)]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}