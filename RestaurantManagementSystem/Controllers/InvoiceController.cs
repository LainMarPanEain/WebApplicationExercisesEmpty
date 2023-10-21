using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.DAO;
using RestaurantManagementSystem.Models.ViewModels;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Utilities;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace RestaurantManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class InvoiceController:Controller
    {
        private readonly RMSDBContext _rMSDBContext;

        public InvoiceController(RMSDBContext rMSDBContext)
        {
            _rMSDBContext = rMSDBContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        //public IActionResult Entry() { return View(); }
        public IActionResult Entry(string OrderId)
        {
            ViewBag.Employees = _rMSDBContext.Employees.Where(x=> x.Position.Name.Equals("Cashier")).Select(s=> new EmployeeViewModel
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();
            InvoiceViewModel orderToPay = (from o in _rMSDBContext.Orders 
                                                join od in _rMSDBContext.OrderDetails on o.Id equals od.OrderId
                                                join p in _rMSDBContext.Products on od.ProductId equals p.Id
                                           where o.Id==OrderId select new InvoiceViewModel
                                           {
                                               OrderId=OrderId,
                                               OrderNo=o.No,
                                               TableNo=o.Table.No,
                                               TotalAmount=(od.Quantity*p.Price),
                                               No = "INV"+DateTime.Now.ToString("ddMMyyyyHHmmssffffff")
                                           }).FirstOrDefault();
            return View(orderToPay);
        }
        [HttpPost]
        public IActionResult Entry(InvoiceViewModel anInvoice)
        {
            try
            {
                var entity = new InvoiceEntity()
                {
                    Id = Guid.NewGuid().ToString(),//for new id when uer create the record 36 char GUID  , UUID ,  (primary key)
                    No = anInvoice.No,
                    OrderId = anInvoice.OrderId,
                    //OrderNo = anInvoice.OrderNo,
                    EmployeeId = anInvoice.EmployeeId,
                    PaymentWith = anInvoice.PaymentWith,
                    TotalAmount = anInvoice.TotalAmount,
                    Ip = NetworkHelper.GetLocalIp()
                };
                //adding the record to the Orders of db context
                _rMSDBContext.Invoices.Add(entity);

                var order = _rMSDBContext.Orders.Where(x => x.Id.Equals(anInvoice.OrderId)).SingleOrDefault();
                if (order is not null)
                {
                    order.IsPaid = true;
                    _rMSDBContext.Entry(order).State = EntityState.Modified;
                }

                _rMSDBContext.SaveChanges();//finally actually save to the database 
                TempData["Msg"] = "Payment Successfull.";
                
                //return Json(new { response = "success" });
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Error occur when record is created because of " + ex.Message;
                //return Json(new { response = "error" });
            }
            return RedirectToAction("List");
        }
        public IActionResult List()
        {
            IQueryable<InvoiceViewModel> invoices = _rMSDBContext.Invoices.Select(x => new InvoiceViewModel
            {
                No = x.No,
                OrderId = x.OrderId,
                OrderNo = x.Order.No,
                EmployeeId = x.EmployeeId,
                Employee = x.Employee,
                PaymentWith = x.PaymentWith,
                TotalAmount = x.TotalAmount
            });
            return View(invoices);
        }
        public IActionResult Edit(string Id)
        {
            ViewBag.Employees = _rMSDBContext.Employees.Where(x => x.Position.Name.Equals("Cashier")).Select(s => new EmployeeViewModel
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();
            InvoiceViewModel invoiceViewModel = _rMSDBContext.Invoices.Where(x => x.Id.Equals(Id)).Select(x => new InvoiceViewModel
            {
                Id = x.Id,
                No = x.No,
                PaymentWith = x.PaymentWith,
                TotalAmount = x.TotalAmount,
                Employee = x.Employee
            }).SingleOrDefault();

            return View(invoiceViewModel);
        }
        [HttpPost]
        public IActionResult Update(InvoiceViewModel invoiceViewModel)
        {
            try
            {
                InvoiceEntity invoiceEntity = new InvoiceEntity()
                {
                    Id = invoiceViewModel.Id,
                    No = invoiceViewModel.No,
                    
                    Ip = NetworkHelper.GetLocalIp()
                };
                _rMSDBContext.Entry(invoiceEntity).State = EntityState.Modified;
                _rMSDBContext.SaveChanges();
                TempData["Msg"] = "update process is completed successfully.";
            }
            catch (Exception e)
            {
                TempData["Msg"] = "Error occured while updating invoices. Reason: " + e.Message;
            }
            return RedirectToAction("List");
        }
        
        public IActionResult Delete(string Id)
        {
            try
            {
                var entity = _rMSDBContext.Invoices.Where(x => x.Id.Equals(Id)).SingleOrDefault();
                if (entity == null)
                {
                    TempData["Msg"] = "No data to delete.";
                }
                _rMSDBContext.Invoices.Remove(entity);
                _rMSDBContext.SaveChanges();
                TempData["Msg"] = "Successfully deleted the invoice.";
            }
            catch (Exception e)
            {
                TempData["Msg"] = "Error occured while deleting tables. Reason: " + e.Message;
            }
            return RedirectToAction("List");
        }
    }
}
