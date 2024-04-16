using CartService.Api.Models;
using CartService.Api.Repository;
using CartService.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CartService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Customer,Admin")]
    public class CartController : ControllerBase
    {
        
        private readonly ICartsService _cartsService;
        public CartController(ICartsService cartsService)
        {
           _cartsService = cartsService;
        }

        [HttpGet("GetUserCart/{email}")]
        public IActionResult GetUserCart([FromRoute]string email)
        {
            var cart=_cartsService.GetUserCart(email);
            if (cart == null)
            {
                return BadRequest(new { Message = "User does not exist" });
            }
            var cartResponse = cart.Select(item => new
            {
                ProductId = item.Item1,
                Quantity = item.Item2,
            }).ToList();

            return Ok(cartResponse);
            
            
        }
        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart([FromBody]AddToCart addToCartObj)
        {
            var jwtToken = HttpContext.Request.Headers["Authorization"];
            bool res=await _cartsService.AddToCart(addToCartObj,jwtToken);
            if (res)
            {
                return Ok(new { Message = "Item added to cart" });
            }
            return BadRequest(new { Message = "User doesn't exist or Item Out of stock" });

        }
    }
}
