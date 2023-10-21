using CRUDMVC.DAO;
using CRUDMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRUDMVC.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentDbContext _studentDbContext;//to do db operations
        //constructor injection for student db context for database operaitons
        public StudentController(StudentDbContext studentDbContext)
        {
            _studentDbContext = studentDbContext;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(Student student)
        {
            try
            {
                _studentDbContext.Students.Add(student);//adding data to the db context
                _studentDbContext.SaveChanges();//actual save of record to db
                ViewBag.Msg = "Added Successfully";
            }
            catch (Exception e)
            {
                ViewBag.Msg = "Added Failed"+e.Message;
            }
            return View();
        }
    }
}
