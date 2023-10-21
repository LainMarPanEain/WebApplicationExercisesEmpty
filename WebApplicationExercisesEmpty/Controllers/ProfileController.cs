using Microsoft.AspNetCore.Mvc;

namespace WebApplicationExercises.Controllers
{
    public class ProfileController : Controller
    {
        public string Now()
        {
            DateTime now = DateTime.Now;
            string nowMsg;
            if(now.Hour < 12)
            {
                nowMsg = "Hi, Good Morning. Time is " + now.ToString();
            }
            else
            {
                nowMsg = "Hi, Good Afternoon. Time is "+now.ToString();
            }
            return nowMsg;
        }
        public IActionResult Me()
        {
            ViewBag.myName = "Eain";
            ViewBag.now = Now();
            return View();
        }
        public IActionResult CV() 
        { 
            return View(); 
        }
        public IActionResult Photo()
        {
            string[] myPhotos = {"profile.img", "memories.img", "birthday.jpg"};
            ViewBag.photos = myPhotos;
            return View();
        }
        public IActionResult Group()
        {
            return View();
        }
    }
}
