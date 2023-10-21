using Microsoft.AspNetCore.Mvc;

namespace AJAXJQueryProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult TellMeTime()
        {
            ViewBag.Time = DateTime.Now;
            return View("Index");
        }
    }
}
