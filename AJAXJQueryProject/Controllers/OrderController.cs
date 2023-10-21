using AJAXJQueryProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace AJAXJQueryProject.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public JsonResult TellMeDate()
        {
            string date = DateTime.Now.ToShortDateString();
            return Json(date);
        }

        public IActionResult MakeOrder() => View();

        [HttpPost]
        public JsonResult MakeOrder(Order order) {
            Order anOrder = order;
            return Json(order);
        }
    }
}
