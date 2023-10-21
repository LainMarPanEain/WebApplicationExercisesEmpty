using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagementSystem.Models.ViewModels
{
    public class OrderViewModel
    {
        public string Id { get; set; }
        public string No { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeNo { get; set; }
        public virtual EmployeeEntity Employee { get; set; }
        public string IsParcel { get; set; }
        public string Status { get; set; }
        public string TableId { get; set; }
        public int TableNo { get; set; }
        public virtual TablesEntity Table { get; set; }
        public DateTime OrderAt { get; set; }
        public virtual OrderDetailViewModel[] OrderDetails { get; set; }
        public int Quantity { get; set; }
        public string Remark { get; set; }
        //public virtual ProductViewModel[] Products { get; set; }
        public List<ProductViewModel> Products { get; internal set; }
    }
}
