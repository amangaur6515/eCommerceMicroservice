namespace ProductDetailService.Api.Models
{
    public class ProductDetail
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public string Size { get; set; }
        public string Design { get; set; }
        public string ImageUrl { get; set; }
    }
}
