using CurrencyConverter.Models;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConverter.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Entry()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Entry(Employee employee)
        {
            int Id = employee.Id;
            string FirstName = employee.FirstName;
            string LastName = employee.LastName;
            ViewData["id"] = Id;
            ViewData["employee"] = employee;
            ViewBag.FullName = FirstName + " " + LastName;
            ViewData["suggestionMsg"] = $"You can go to the nearest office around {employee.Address.Street} street.";
            
            return View();
        }
        public IActionResult MultiEntry()
        {
            return View();
        }
        [HttpPost]
        public IActionResult MultiEntry(IList<string> id, IList<string> name)
        {
            ViewBag.Ids = id;
            ViewBag.Names = name;
            IList<string> records = new List<string>();
            for(int i=0;i<id.Count;i++)
            {
                records.Add(id[i]+"   " + name[i]);
            }
            ViewBag.Records= records;
            return View();
        }
    }
}
