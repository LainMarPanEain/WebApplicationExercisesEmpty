using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.DAO;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Models.ViewModels;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.Controllers
{
    [Authorize]
    public class OrderProcessController : Controller
    {
        private readonly RMSDBContext _rMSDBContext;

        public OrderProcessController(RMSDBContext rMSDBContext) 
        {
            _rMSDBContext = rMSDBContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            ViewBag.Products = _rMSDBContext.Products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Price = p.Price
            }).ToList();
            ViewBag.Tables = _rMSDBContext.Tables.Where(x=>x.IsAvailable).Select(t => new TablesViewModel
            {
                Id=t.Id,
                No=t.No,
                IsAvailable=t.IsAvailable==true?"yes":"no",
                AvailablePerson=t.AvailablePerson
            }).ToList();
            ViewBag.Employees = _rMSDBContext.Employees.Where(x=> x.Position.Name.Equals("Waiter")).Select(e => new EmployeeViewModel
            {
                Id=e.Id,
                Code=e.Code,
                Name=e.Code+":"+e.Name
            }).ToList();
            var orders = _rMSDBContext.Orders.Where(x => x.IsPaid==false).Select(x => new OrderViewModel
            {
                Id = x.Id,
                No = x.No,
                IsParcel = x.IsParcel==true?"Yes":"No",
                Status = x.Status,
                Employee = x.Employee,
                EmployeeId = x.EmployeeId,
                Table = x.Table,
                TableId = x.TableId
            }).OrderBy(o=>o.No).ToList();
            return View(orders);
        }

        public IActionResult Entry() { return View(); }
        public JsonResult GetUnitPriceByProductId(string id)
        {
            var Product = _rMSDBContext.Products.Where(x => x.Id.Equals(id)).FirstOrDefault();
            return Json(Product.Price);
        }
        [HttpPost]
        public JsonResult Entry(OrderViewModel anOrder)
        {
            try
            {
                var entity = new OrderEntity()
                {
                    Id = Guid.NewGuid().ToString(),//for new id when uer create the record 36 char GUID  , UUID ,  (primary key)
                    No = anOrder.No,
                    TableId = anOrder.TableId,
                    EmployeeId = anOrder.EmployeeId,
                    IsParcel = anOrder.IsParcel.Equals("Yes")?true:false,
                    Status = anOrder.Status,
                    Ip = NetworkHelper.GetLocalIp(),
                    IsPaid = false
                };
                //adding the record to the Orders of db context
                _rMSDBContext.Orders.Add(entity);

                var orderDetails = new List<OrderDetailEntity>();
                foreach (var detail in anOrder.OrderDetails)
                {
                    OrderDetailEntity orderDetail = new OrderDetailEntity()
                    {
                        Id = Guid.NewGuid().ToString(),//order master Id (primary key)
                        OrderId = entity.Id,//get the foreign key 
                        ProductId = detail.ProductId,
                        Quantity = detail.Quantity,
                        Remark = detail.Remark,
                        Ip = NetworkHelper.GetLocalIp()
                    };
                    orderDetails.Add(orderDetail);//collecting the order details
                }
                _rMSDBContext.OrderDetails.AddRange(orderDetails);//adding the records to the OrderDetails of db context

                var table = _rMSDBContext.Tables.Where(x => x.Id.Equals(anOrder.TableId)).SingleOrDefault();
                if (table is not null)
                {
                    table.IsAvailable = false;
                    _rMSDBContext.Entry(table).State = EntityState.Modified;
                }

                _rMSDBContext.SaveChanges();//finally actually save to the database 
                return Json(new { response = "success" });
            }
            catch (Exception ex)
            {
                ViewBag.Msg = "Error occur when record is created because of " + ex.Message;
                return Json(new { response = "error" });
            }
        }

        public IActionResult SpecialItemList()
        {
            ViewBag.SpecialItems = _rMSDBContext.Products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Price = p.Price
            }).ToList();
            return View();
        }
        public IActionResult Delete(string Id)
        {
            try
            {
                var order = _rMSDBContext.Orders.Where(x => x.Id.Equals(Id)).SingleOrDefault();
                var orderDetails = _rMSDBContext.OrderDetails.Where(x => x.OrderId.Equals(Id)).ToList();
                if (order == null || orderDetails.Count<0)
                {
                    TempData["Msg"] = "No data to delete.";
                    return RedirectToAction("List");
                }
                _rMSDBContext.OrderDetails.RemoveRange(orderDetails);
                _rMSDBContext.Orders.Remove(order);
                _rMSDBContext.SaveChanges();
                TempData["Msg"] = "Successfully deleted the order.";
            }
            catch (Exception e)
            {
                TempData["Msg"] = "Error occured while deleting orders. Reason: " + e.Message;
            }
            return RedirectToAction("List");
        }
        
        public IActionResult Detail(string Id)
        {
            OrderViewModel orderAndOrderDetail = _rMSDBContext.Orders.Where(x => x.Id == Id).Select(o => new OrderViewModel
            {
                No = o.No,
                TableNo = o.Table.No,
                IsParcel = o.IsParcel.Equals(true) ? "YES" : "NO",
                EmployeeNo = o.Employee.Code + ":" + o.Employee.Name,
                OrderDetails = _rMSDBContext.OrderDetails.Where(od => od.OrderId == Id).Select(s => new OrderDetailViewModel
                {
                    Products = _rMSDBContext.Products.Where(p => p.Id == s.ProductId).Select(pp => new ProductViewModel
                    {
                        Code = pp.Code,
                        Name = pp.Name,
                        Category = pp.Category,
                        Price = pp.Price
                    }).ToList(),
                    Quantity = s.Quantity,
                    Remark = s.Remark,
                }).ToArray()
            }).SingleOrDefault();
            return View(orderAndOrderDetail);
        }

    }
}
