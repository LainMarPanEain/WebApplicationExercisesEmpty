using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagementSystem.Models
{
    [Table("Orders")]
    public class OrderEntity:BaseEntity
    {
        public string No { get; set; }

        [ForeignKey("EmployeeId")]
        public string EmployeeId { get; set; }
        public virtual EmployeeEntity Employee { get; set; }
        public bool IsParcel { get; set; }
        public string Status { get; set; }

        [ForeignKey("TableId")]
        public string TableId { get; set; }
        public virtual TablesEntity Table { get; set; }
        public bool IsPaid { get; set; }
    }
}
