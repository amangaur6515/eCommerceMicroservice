using System.ComponentModel.DataAnnotations;

namespace ProductService.Api.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Size { get; set; }
        [Required]
        public string Design { get;set; }
        [Required]
        public string ImageUrl { get; set; }

    }
}
