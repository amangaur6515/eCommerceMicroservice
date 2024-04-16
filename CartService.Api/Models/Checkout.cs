using System.ComponentModel.DataAnnotations;

namespace CartService.Api.Models
{
    public class Checkout
    {
        [Required]
        public string Email { get; set; }
    }
}
