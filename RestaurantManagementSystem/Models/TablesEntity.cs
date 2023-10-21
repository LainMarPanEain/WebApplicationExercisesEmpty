using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagementSystem.Models
{
    [Table("Tables")]
    public class TablesEntity:BaseEntity
    {
        public int No { get; set; }

        public bool IsAvailable { get; set; }

        public int AvailablePerson { get; set; }
    }
}
