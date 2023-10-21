using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.DAO;
using RestaurantManagementSystem.Models.ViewModels;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Utilities;
using Microsoft.AspNetCore.Authorization;

namespace RestaurantManagementSystem.Controllers
{
    public class TablesController : Controller
    {
        private readonly RMSDBContext rMSDBContext;

        public TablesController(RMSDBContext rMSDBContext)
        {
            this.rMSDBContext = rMSDBContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles ="Admin")]
        public IActionResult Entry()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Entry(TablesViewModel tablesViewModel)
        {
            try
            {
                TablesEntity tables = new TablesEntity()
                {
                    No = tablesViewModel.No,
                    IsAvailable = tablesViewModel.IsAvailable.Equals("yes") ? true : false,
                    AvailablePerson = tablesViewModel.AvailablePerson,
                    Id = Guid.NewGuid().ToString(),
                    Ip = NetworkHelper.GetLocalIp()
                };
                rMSDBContext.Tables.Add(tables);
                rMSDBContext.SaveChanges();
                ViewBag.Msg = "Successfully added new tables.";
            }
            catch (Exception e)
            {
                ViewBag.Msg = "Error occured while inserting new tables. Reason: " + e.Message;
                throw;
            }
            return View();
        }
        public IActionResult List()
        {
            IList<TablesViewModel> tables = rMSDBContext.Tables.Select(x => new TablesViewModel
            {
                Id = x.Id,
                No = x.No,
                IsAvailable = x.IsAvailable.Equals(true) ? "Yes" : "No",
                AvailablePerson = x.AvailablePerson
            }).OrderBy(o => o.No).ToList();
            return View(tables);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(string Id)
        {
            TablesViewModel tablesViewModel = rMSDBContext.Tables.Where(x => x.Id.Equals(Id)).Select(x => new TablesViewModel
            {
                Id = x.Id,
                No = x.No,
                IsAvailable = x.IsAvailable.Equals(true)? "yes":"no",
                AvailablePerson =  x.AvailablePerson
            }).SingleOrDefault();

            return View(tablesViewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Update(TablesViewModel tablesViewModel)
        {
            try
            {
                TablesEntity tables = new TablesEntity()
                {
                    Id = tablesViewModel.Id,
                    No = tablesViewModel.No,
                    IsAvailable = tablesViewModel.IsAvailable.Equals("yes")?true:false,
                    AvailablePerson = tablesViewModel.AvailablePerson,
                    Ip = NetworkHelper.GetLocalIp()
                };
                rMSDBContext.Entry(tables).State = EntityState.Modified;
                rMSDBContext.SaveChanges();
                TempData["Msg"] = "update process is completed successfully.";
            }
            catch (Exception e)
            {
                TempData["Msg"] = "Error occured while updating tables. Reason: " + e.Message;
            }
            return RedirectToAction("List");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string Id)
        {
            try
            {
                var entity = rMSDBContext.Tables.Where(x => x.Id.Equals(Id)).SingleOrDefault();
                if (entity == null)
                {
                    TempData["Msg"] = "No data to delete.";
                }
                rMSDBContext.Tables.Remove(entity);
                rMSDBContext.SaveChanges();
                TempData["Msg"] = "Successfully deleted new tables.";
            }
            catch (Exception e)
            {
                TempData["Msg"] = "Error occured while deleting tables. Reason: " + e.Message;
            }
            return RedirectToAction("List");

        }
    }
}
