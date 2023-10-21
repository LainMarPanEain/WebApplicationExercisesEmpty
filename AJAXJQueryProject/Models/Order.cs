namespace AJAXJQueryProject.Models
{
    public class Order
    {
        public string Id { get; set; } 
        public int Qty { get; set; }
        public DateTime OrderTime { get; set; }= DateTime.Now;
    }
}
