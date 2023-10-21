using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagementSystem.Models.ViewModels
{
    public class InvoiceViewModel
    {
        public string Id { get; set; }
        public string No { get; set; }
        public string PaymentWith { get; set; }
        public string OrderId { get; set; }
        public virtual OrderEntity Order { get; set; }
        public string OrderNo { get; set; }
        public virtual TablesEntity Tables { get; set; }
        public string TableId { get; set; }
        public int TableNo { get; set; }
        public decimal TotalAmount { get; set; }
        public string EmployeeId { get; set; }
        public virtual EmployeeEntity Employee { get; set;}

    }
}
