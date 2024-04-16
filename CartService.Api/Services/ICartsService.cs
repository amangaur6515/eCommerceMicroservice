using CartService.Api.Models;

namespace CartService.Api.Services
{
    public interface ICartsService
    {
        List<(int,int)> GetUserCart(string email);
        Task<bool> AddToCart(AddToCart addToCartObj,string jwtToken);
       
    }
}