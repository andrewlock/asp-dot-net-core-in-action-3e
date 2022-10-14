using FindingAView.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FindingAView.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult IndexThatRendersPrivacy()
        {
            // Search all locations, found in Views/Home
            return View("Privacy");
        }

        public IActionResult IndexThatRendersPrivacy2()
        {
            // Check specific location
            return View("Views/Home/Privacy.cshtml");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Search all locations, found in Views/Shared/Error
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}