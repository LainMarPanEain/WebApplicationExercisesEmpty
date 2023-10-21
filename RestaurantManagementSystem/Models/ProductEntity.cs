using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagementSystem.Models
{
    [Table("Product")]
    public class ProductEntity:BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }

        [ForeignKey("CategoryId")]
        public string CategoryId { get; set; }
        public virtual CategoryEntity Category { get; set; } //why virtual? -don't want to produce col at db for this
        public decimal Price { get; set; }
        public bool IsTodaySpecial { get; set; }
        public bool IsAvailable { get; set; }
    }
}
