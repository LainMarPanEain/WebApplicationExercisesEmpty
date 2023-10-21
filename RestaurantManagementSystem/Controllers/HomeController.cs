using Microsoft.AspNetCore.Mvc;
using RestaurantManagementSystem.DAO;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Models.ViewModels;
using System.Diagnostics;

namespace RestaurantManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RMSDBContext _rMSDBContext;

        public HomeController(ILogger<HomeController> logger, RMSDBContext rMSDBContext)
        {
            _logger = logger;
            _rMSDBContext = rMSDBContext;
        }

        public IActionResult Index()
        {
            ViewBag.TodaySpecialProds = _rMSDBContext.Products.Where(x => x.IsTodaySpecial).Select(
                s=> new ProductViewModel
                {
                    Name = s.Name,
                    Price = s.Price,
                    Category = s.Category
                }).ToList();
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

        [HttpGet]
        public IActionResult Service()
        {
            // Your action logic here
            return View();
        }
        [HttpGet]
        public IActionResult About()
        {
            // Your action logic here
            return View();
        }

    }
}