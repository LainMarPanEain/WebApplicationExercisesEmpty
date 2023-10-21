using Microsoft.AspNetCore.Mvc;
using RestaurantManagementSystem.DAO;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Models.ViewModels;
using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Reporting.NETCore;
using RestaurantManagementSystem.Models.ReportModels;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace RestaurantManagementSystem.Controllers
{
    public class ProductController : Controller
    {
        private readonly RMSDBContext rMSDBContext;
        private readonly IWebHostEnvironment _webHostEnvironment; //to read rdlc file under wwwroot/reportFiles
        public CategoryEntity Category;

        public ProductController(RMSDBContext rMSDBContext, IWebHostEnvironment webHostEnvironment) 
        {
            this.rMSDBContext = rMSDBContext;
            _webHostEnvironment = webHostEnvironment;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {

            IList<ProductViewModel> products = rMSDBContext.Products.Select(x => new ProductViewModel
            //data exchange between View Model and Model >> DTO  
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                Price = x.Price,
                Category = x.Category,
                IsAvailable = x.IsAvailable == true ? "Yes" : "No",
                IsTodaySpecial = x.IsTodaySpecial == true ? "Yes" : "No"
            }).OrderBy(o => o.Code).ToList();
            return View(products);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Entry()
        {
            var categories = rMSDBContext.Categories.ToList();
            var productViewModel = new ProductViewModel
            {
                Categories = categories
            };

            return View(productViewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Entry(ProductViewModel productViewModel)
        {
            try
            {
                //can also use ternary operational operator to check condition for IsAvailable and IsTodaySpecial
                //can use like IsAvailable = IsAvailable.equals("yes")?true:false
                bool IsAvailable = false;
                bool IsTodaySpecial = false;
                if (productViewModel.IsTodaySpecial == "yes")
                {
                    IsAvailable = true;
                }
                if (productViewModel.IsAvailable == "yes")
                {
                    IsTodaySpecial = true;
                }
                var selectedCategory = rMSDBContext.Categories.FirstOrDefault(c => c.Id == productViewModel.CategoryId);

                ProductEntity product = new ProductEntity()
                {
                    Id = Guid.NewGuid().ToString(),//new id when create record everytime- 36 char
                    Code = productViewModel.Code,
                    Name = productViewModel.Name,
                    CategoryId = selectedCategory.Id,
                    Price = productViewModel.Price,
                    IsAvailable = IsAvailable,
                    IsTodaySpecial = IsTodaySpecial,
                    Ip = NetworkHelper.GetLocalIp()
                };
                rMSDBContext.Products.Add(product);
                rMSDBContext.SaveChanges();
                ViewBag.Msg = "Record created successfully.";
            }
            catch (Exception e)
            {
                ViewBag.Msg = "Error occured when recored create. Reason: " + e.Message;
                throw;
            }
            var categories = await rMSDBContext.Categories.ToListAsync();
            productViewModel.Categories = categories;

            return View(productViewModel);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string Id)
        {
            try
            {
                var product = rMSDBContext.Products.Where(x => x.Id.Equals(Id)).SingleOrDefault();
                if (product == null)
                {
                    TempData["Msg"] = "There is no recrod that you select.";
                }
                rMSDBContext.Products.Remove(product);// collect the data to remove
                rMSDBContext.SaveChanges();// remove the record from the database 
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
            ProductViewModel productViewModel = rMSDBContext.Products.Where(x => x.Id.Equals(Id)).Select(x => new ProductViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                Price = x.Price,
                //CategoryId = x.CategoryId,
                Category = x.Category,
                IsAvailable = x.IsAvailable == true ? "y" : "n",
                IsTodaySpecial = x.IsTodaySpecial == true ? "y" : "n"
            }).SingleOrDefault();
            ViewBag.categories = rMSDBContext.Categories.ToList();
            return View(productViewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Update(ProductViewModel productViewModel)
        {
            try
            {
                //DTO >> Data Transfer Object 
                var product = new ProductEntity()
                {
                    Id = productViewModel.Id,//not to generate new id because this is update processs 
                    Name = productViewModel.Name,//c101
                    Code = productViewModel.Code,
                    Price = productViewModel.Price,
                    IsAvailable = productViewModel.IsAvailable.Equals("yes") ? true : false,
                    IsTodaySpecial = productViewModel.IsTodaySpecial.Equals("yes") ? true : false,
                    CategoryId = productViewModel.CategoryId,
                    UpdatedDate = DateTime.Now,
                    Ip = NetworkHelper.GetLocalIp()
                };
                rMSDBContext.Entry(product).State = EntityState.Modified;//editing the record to the products of db context
                rMSDBContext.SaveChanges();// actually update to the database 
                TempData["Msg"] = "update process is completed successfully.";
            }
            catch (Exception e)
            {
                TempData["Msg"] = "error occur when record is updated.";
            }
            ViewBag.categories = rMSDBContext.Categories.ToList();
            return RedirectToAction("List");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult ProductDetailReport()
        {
            ViewBag.FromCode = rMSDBContext.Products.OrderBy(o => o.Code).ToList();
            ViewBag.ToCode = rMSDBContext.Products.OrderBy(o => o.Code).ToList();
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult ProductDetailReport(string FromCode, string ToCode)
        {
            var rdlcFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "ReportFiles", "ProductReport.rdlc");

            IList<ProductReportModel> products = rMSDBContext.Products.Where(p => p.Code.CompareTo(FromCode) >= 0 && p.Code.CompareTo(ToCode) <= 0)
                .Select(x => new ProductReportModel
                {
                    Code = x.Code,
                    Name = x.Name,
                    Price = x.Price,
                    IsTodaySpecial = x.IsTodaySpecial == true ? "Yes" : "No",
                    IsAvailable = x.IsAvailable == true ? "Yes" : "No",
                    Category = x.Category.Name
                }).ToList();
            Stream reportDefinition;
            using var fs = new FileStream(rdlcFilePath, FileMode.Open);
            reportDefinition = fs;
            LocalReport localReport = new LocalReport();
            localReport.LoadReportDefinition(reportDefinition);
            localReport.DataSources.Add(new ReportDataSource("ProductReportDataSet", products));
            localReport.SetParameters(new[] { new ReportParameter("ProdFromCodeToCodeReportParameter", $"From Code: {FromCode} To Code: {ToCode}") });
            fs.Dispose();
            return File(localReport.Render("pdf"), "application/pdf");
        }
    }
}
