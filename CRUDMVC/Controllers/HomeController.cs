using Microsoft.AspNetCore.Mvc;

namespace CRUDMVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

    }
}
