using Microsoft.AspNetCore.Mvc;
using RestaurantManagementSystem.DAO;
using RestaurantManagementSystem.Models.ViewModels;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Utilities;
using Microsoft.EntityFrameworkCore;

namespace RestaurantManagementSystem.Controllers
{
    public class PositionController : Controller
    {
        private readonly RMSDBContext rMSDBContext;

        public PositionController(RMSDBContext rMSDBContext)
        {
            this.rMSDBContext = rMSDBContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Entry()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Entry(PositionViewModel positionViewModel)
        {
            try
            {
                PositionEntity position = new PositionEntity()
                {
                    Code = positionViewModel.Code,
                    Name = positionViewModel.Name,
                    BasicSalary = positionViewModel.BasicSalary,
                    Id = Guid.NewGuid().ToString(),
                    Ip = NetworkHelper.GetLocalIp()
                };
                rMSDBContext.Positions.Add(position);
                rMSDBContext.SaveChanges();
                ViewBag.Msg = "Successfully added new position.";
            }
            catch (Exception e)
            {
                ViewBag.Msg = "Error occured while inserting new position. Reason: " + e.Message;
                throw;
            }
            return View();
        }
        public IActionResult List()
        {
            IList<PositionViewModel> positions = rMSDBContext.Positions.Select(x => new PositionViewModel
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                BasicSalary = x.BasicSalary
            }).OrderBy(o => o.Code).ToList();
            return View(positions);
        }
        public IActionResult Edit(string Id)
        {
            var viewModel = rMSDBContext.Positions.Where(x => x.Id.Equals(Id)).Select(x => new PositionViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                BasicSalary = x.BasicSalary
            }).SingleOrDefault();

            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Update(PositionViewModel positionViewModel)
        {
            try
            {
                PositionEntity position = new PositionEntity()
                {
                    Id = positionViewModel.Id,
                    Name = positionViewModel.Name,
                    Code = positionViewModel.Code,
                    BasicSalary = positionViewModel.BasicSalary,
                    Ip = NetworkHelper.GetLocalIp()
                };
                rMSDBContext.Entry(position).State = EntityState.Modified;
                rMSDBContext.SaveChanges();
                TempData["Msg"] = "update process is completed successfully.";
            }
            catch (Exception e)
            {
                TempData["Msg"] = "Error occured while updating position. Reason: " + e.Message;
            }
            return RedirectToAction("List");
        }
        public IActionResult Delete(string Id)
        {
            try
            {
                var entity = rMSDBContext.Positions.Where(x => x.Id.Equals(Id)).SingleOrDefault();
                if (entity == null)
                {
                    TempData["Msg"] = "No data to delete.";
                }
                rMSDBContext.Positions.Remove(entity);
                rMSDBContext.SaveChanges();
                TempData["Msg"] = "Successfully deleted new position.";
            }
            catch (Exception e)
            {
                TempData["Msg"] = "Error occured while deleting position. Reason: " + e.Message;
            }
            return RedirectToAction("List");

        }
    }
}
