using System.ComponentModel.DataAnnotations;

namespace AuthService.Api.Models
{
    public class UserRegister
    {
        public int Id { get; set; }
        public string Role { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
