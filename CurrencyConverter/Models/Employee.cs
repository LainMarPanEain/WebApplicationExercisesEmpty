namespace CurrencyConverter.Models
{
    public class Employee
    {
        //enapsulated properties variables
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //Has-A relationship
        public HomeAddress Address { get; set; }
    }
}
