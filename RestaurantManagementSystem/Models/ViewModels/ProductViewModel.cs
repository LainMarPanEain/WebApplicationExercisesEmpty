namespace RestaurantManagementSystem.Models.ViewModels
{
    public class ProductViewModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        //public string Category { get; set; }
        //public string CategoryCode { get; set; }
        //public string CategoryName { get; set; }
        public List<CategoryEntity> Categories { get; set; }
        public decimal Price { get; set; }
        public string IsTodaySpecial { get; set; }
        public string IsAvailable { get; set; }
        public string CategoryId { get; set; }
        public virtual CategoryEntity Category { get; set; }
        //public string todaySpecial { get; set; }
        //public string available { get; set; }
    }
}
