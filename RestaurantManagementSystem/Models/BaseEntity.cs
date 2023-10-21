using System.ComponentModel.DataAnnotations;

namespace RestaurantManagementSystem.Models
{
    public class BaseEntity
    {
        [Key]
        public string Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; }
        public string Ip { get; set; }
    }
}
