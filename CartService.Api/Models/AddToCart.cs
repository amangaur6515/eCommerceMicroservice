using System.ComponentModel.DataAnnotations;

namespace CartService.Api.Models
{
    public class AddToCart
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public int ProductId { get; set; } 
        public int Quantity { get; set; }


    }
}
