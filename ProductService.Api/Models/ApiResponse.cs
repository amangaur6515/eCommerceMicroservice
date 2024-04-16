namespace ProductService.Api.Models
{
    public class ApiResponse
    {
        public int productId { get; set; }
        public string productName { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public string size { get; set; }
        public string design { get; set; }
        public string ImageUrl { get; set; }
    }
}
