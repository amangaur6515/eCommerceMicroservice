using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductDetailEntity;
using ProductDetailService.Api.Repository;

namespace ProductDetailService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Customer,Admin")]
    public class ProductDetailController : ControllerBase
    {
        private readonly IProductDetailRepository _productDetailRepository;
        public ProductDetailController(IProductDetailRepository productDetailRepository)
        {
            _productDetailRepository = productDetailRepository;
        }

        [HttpGet("GetAllProductDetail")]

        public IActionResult GetAllProducts()
        {
            return Ok(_productDetailRepository.GetProducts());
        }

        [HttpGet("GetProductDetail/{id}")]
        public IActionResult GetProduct([FromRoute]int id)
        {
            var p = _productDetailRepository.GetProductDetail(id);
            if(p == null)
            {
                return BadRequest(new { Message = "Invalid id" });
            }
            return Ok(p);
        }
    }
}
