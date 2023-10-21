namespace RMSReportHelpers
{
    public class ProductReportModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string IsTodaySpecial { get; set; }
        public string IsAvailable { get; set; }
        public string Category { get; set; }
    }
}
