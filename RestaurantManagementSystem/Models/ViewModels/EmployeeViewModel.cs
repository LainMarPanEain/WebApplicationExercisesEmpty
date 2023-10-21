namespace RestaurantManagementSystem.Models.ViewModels
{
    public class EmployeeViewModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Nrc { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public DateTime DOB { get; set; }
        public DateTime JoinedDate { get; set; }
        public string PositionId { get; set; }
        public virtual PositionEntity Position { get; set; }
    }
}
