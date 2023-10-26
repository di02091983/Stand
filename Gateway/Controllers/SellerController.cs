using Gateway.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Gateway.Controllers
{
    public class SellerController : Controller
    {
        private readonly ILogger<SellerController> _logger;

        public SellerController(ILogger<SellerController> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<>>  GetCatalog()
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