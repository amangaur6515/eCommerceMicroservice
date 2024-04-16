using CartService.Api.Models;
using UserRegisteredEntity.cs;

namespace CartService.Api.Repository
{
    public interface ICartRepository
    {
        Task<bool> AssignCart(User obj);
        List<(int,int)> GetUserCart(string email);
        bool AddToCart(AddToCart addToCartObj);
    }
}
