using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CurrencyConverter.Controllers
{
    public class RegisterationController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(string Name, string Email, string Password, string ConfirmPassword, string Gender, string DateOfBirth, string City, string Address)
        {
            TempData["name"] = Name;
            TempData["email"] = Email;
            TempData["password"] = Password;
            TempData["confirmPassword"] = ConfirmPassword;
            TempData["gender"] = Gender;
            TempData["dateOfBirth"] = DateOfBirth;
            TempData["city"] = City;
            TempData["address"] = Address;
            ViewData["successMsg"] = $"You have successfully registered with {Email}.";
            if(Name!=null && Email!=null && Password!=null && ConfirmPassword!=null && Gender!=null && DateOfBirth!=null && City!=null && Address!=null)
            {
                return View();
            }
            return View();
        }
    }
}
