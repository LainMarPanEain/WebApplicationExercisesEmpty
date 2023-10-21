using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.NETCore;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using RestaurantManagementSystem.DAO;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Models.ReportModels;
using RestaurantManagementSystem.Models.ViewModels;
using RestaurantManagementSystem.Utilities;
using System.Data;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace RestaurantManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeeController : Controller
    {
        private readonly RMSDBContext _rMSDBContext;
        private readonly IWebHostEnvironment _webHostEnvironment; //to read rdlc file under wwwroot/reportFiles
        private readonly UserManager<IdentityUser> _userManager;

        public EmployeeController(RMSDBContext rMSDBContext, IWebHostEnvironment webHostEnvironment, UserManager<IdentityUser> userManager)
        {
            _rMSDBContext = rMSDBContext;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Entry()
        {
            ViewBag.Positions = _rMSDBContext.Positions.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Entry(EmployeeViewModel employeeViewModel)
        {
            try
            {
                var user = CreateUser();
                EmployeeEntity employee = new EmployeeEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = employeeViewModel.Code,
                    Name = employeeViewModel.Name,
                    Nrc = employeeViewModel.Nrc,
                    Phone = employeeViewModel.Phone,
                    Email = employeeViewModel.Email,
                    Address = employeeViewModel.Address,
                    Gender = employeeViewModel.Gender.Equals("Female") ? "Female" : "Male",
                    DOB = employeeViewModel.DOB,
                    JoinedDate = employeeViewModel.JoinedDate,
                    PositionId = employeeViewModel.PositionId,
                    Ip = NetworkHelper.GetLocalIp()
                };
                _rMSDBContext.Employees.Add(employee);
                _rMSDBContext.SaveChanges();
                user.Email = employeeViewModel.Email;
                user.NormalizedEmail = employeeViewModel.Email;
                user.UserName = employeeViewModel.Email;
                user.NormalizedUserName = employeeViewModel.Name;
                string defaultPassword = "RMS@123welcome";
                var result = await _userManager.CreateAsync(user, defaultPassword);//xxx@gmail.com, 123456
                if(result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Employee");//creating employee for created user
                    ViewBag.Msg = $"1 record is created successfully and you can login with this email : {employeeViewModel.Email} and password: {defaultPassword}";
                }
                //ViewBag.Msg = "Employee Record created successfully.";
            }
            catch (Exception e)
            {
                ViewBag.Msg = "Error occured when recored create. Reason: " + e.Message;
                throw;
            }
            ViewBag.Positions = _rMSDBContext.Positions.ToList();
            return View(employeeViewModel);
        }
        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
        public IActionResult List()
        {
            IList<EmployeeViewModel> employees = _rMSDBContext.Employees.Select(x => new EmployeeViewModel
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Position = x.Position,
                Nrc = x.Nrc,
                Phone = x.Phone,
                Email = x.Email,
                Address = x.Address,
                Gender = x.Gender,
                DOB = x.DOB,
                JoinedDate = x.JoinedDate
            }).OrderBy(o => o.Code).ToList();
            return View(employees);
        }

        public IActionResult Edit(string Id)
        {
            EmployeeViewModel employeeViewModel = _rMSDBContext.Employees.Where(x => x.Id.Equals(Id)).Select(x => new EmployeeViewModel
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Position = x.Position,
                Nrc = x.Nrc,
                Phone = x.Phone,
                Email = x.Email,
                Address = x.Address,
                Gender = x.Gender,
                DOB = x.DOB,
                JoinedDate = x.JoinedDate
            }
            ).SingleOrDefault();
            ViewBag.Positions = _rMSDBContext.Positions.ToList();
            return View(employeeViewModel);
        }
        [HttpPost]
        public IActionResult Update(EmployeeViewModel employeeViewModel) 
        {
            try
            {
                EmployeeEntity employee = new EmployeeEntity()
                {
                    Id = employeeViewModel.Id,
                    Code = employeeViewModel.Code,
                    Name = employeeViewModel.Name,
                    PositionId = employeeViewModel.PositionId,
                    Nrc = employeeViewModel.Nrc,
                    Phone = employeeViewModel.Phone,
                    Email = employeeViewModel.Email,
                    Address = employeeViewModel.Address,
                    Gender = employeeViewModel.Gender,
                    DOB = employeeViewModel.DOB,
                    JoinedDate = employeeViewModel.JoinedDate,
                    Ip = NetworkHelper.GetLocalIp()
                };
                _rMSDBContext.Entry(employee).State = EntityState.Modified;
                _rMSDBContext.SaveChanges();
                TempData["Msg"] = "update process is completed successfully.";
            }
            catch (Exception e)
            {
                TempData["Msg"] = "Error occured when recored update. Reason: " + e.Message;
                throw;
            }
            ViewBag.Positions = _rMSDBContext.Positions.ToList();
            return RedirectToAction("List");
        }
        public IActionResult Delete(string Id)
        {
            try
            {
                EmployeeEntity employee = _rMSDBContext.Employees.Where(x => x.Id.Equals(Id)).SingleOrDefault();
                if (employee != null)
                {
                    TempData["Msg"] = "No data to delete.";
                }
                _rMSDBContext.Employees.Remove(employee);
                _rMSDBContext.SaveChanges();
                TempData["Msg"] = "delete process is completed successfully.";
            }
            catch (Exception e)
            {
                TempData["Msg"] = "Error occured when recored delete. Reason: " + e.Message;
                throw;
            }
            return RedirectToAction("List");
        }
        
        public IActionResult EmployeeDetailReport() 
        {
            ViewBag.FromCode = _rMSDBContext.Employees.OrderBy(o=>o.Code).ToList();
            ViewBag.ToCode = _rMSDBContext.Employees.OrderBy(o => o.Code).ToList();
            return View(); 
        }
        [HttpPost]
        public IActionResult EmployeeDetailReport(string FromCode, string ToCode)
        {
            var rdlcFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "ReportFiles", "EmployeeReport.rdlc");

            IList<EmployeeReportModel> employees = _rMSDBContext.Employees.Where(e => e.Code.CompareTo(FromCode) >= 0 && e.Code.CompareTo(ToCode) <= 0)
                .Select(x => new EmployeeReportModel
                {
                    Code = x.Code,
                    Name = x.Name,
                    Nrc = x.Nrc,
                    Phone = x.Phone,
                    Email = x.Email,
                    Address = x.Address,
                    Gender = x.Gender,
                    DOB = x.DOB.ToShortDateString(),
                    JoinedDate = x.JoinedDate.ToShortDateString(),
                    Position = x.Position.Name,
                    BaseSalary = x.Position.BasicSalary,
                    Age = DateTime.Now.Year - x.DOB.Year
                }).ToList();
            Stream reportDefinition;
            using var fs = new FileStream(rdlcFilePath, FileMode.Open);
            reportDefinition = fs;
            LocalReport localReport = new LocalReport();
            localReport.LoadReportDefinition(reportDefinition);
            localReport.DataSources.Add(new ReportDataSource("EmployeeReportDataSet", employees));
            localReport.SetParameters(new[] { new ReportParameter("EmpFromCodeToCodeReportParameter", $"From Code: {FromCode} To Code: {ToCode}") });
            fs.Dispose();
            return File(localReport.Render("pdf"), "application/pdf");//use excel for excel
        }
    }
}
