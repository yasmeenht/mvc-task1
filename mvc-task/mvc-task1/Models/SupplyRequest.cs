namespace mvc_task1.Models
{
    public class SupplyRequest
    {
        public int RequestId { get; set; }
        public List<Product> Products { get; set; } = new();
        public decimal Subtotal { get; set; }
        public decimal Fee { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalTotal { get; set; }
        public string Status { get; set; } = "Confirmed";
    }
}
