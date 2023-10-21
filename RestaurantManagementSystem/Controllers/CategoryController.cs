using Microsoft.AspNetCore.Mvc;
using RestaurantManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using RestaurantManagementSystem.Services;

namespace RestaurantManagementSystem.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
           
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Entry() => View();

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Entry(CategoryViewModel categoryViewModel)
        {
            try
            {
                _categoryService.Create(categoryViewModel);
                ViewBag.Msg = "Successfully added new category.";
            }
            catch (Exception e)
            {
                ViewBag.Msg = "Error occured while inserting new category. Reason: " + e.Message;
                throw;
            }
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string Id)
        {
            try
            {
                _categoryService.Delete(Id);
                TempData["Msg"] = "delete process is completed successfully.";
            }
            catch (Exception e)
            {
                TempData["Msg"] = "error occur when record is deleted.";
            }
            return RedirectToAction("List");

        }
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(string Id)
        {
            var viewModel = _categoryService.GetById(Id);
            return View(viewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Update(CategoryViewModel viewModel)
        {
            try
            {
                _categoryService.Update(viewModel);
                TempData["Msg"] = "update process is completed successfully.";
            }
            catch (Exception e)
            {
                TempData["Msg"] = "error occur when record is updated.";
            }
            return RedirectToAction("List");
        }
    }
}
