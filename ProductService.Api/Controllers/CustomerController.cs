using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Api.Models;
using ProductService.Api.Services;
using System.Runtime.InteropServices;

namespace ProductService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles="Customer,Admin")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("GetAllProducts")]
        public IActionResult GetAllProducts()
        {
            return Ok(_customerService.GetAllProducts());
        }

        [HttpGet("GetProductDetail/{id}")]
        public async Task<IActionResult> GetProductDetail([FromRoute]int id)
        {
            var jwtToken = HttpContext.Request.Headers["Authorization"];
            
            var res=await _customerService.GetProductDetail(id,jwtToken);
            if (res != null)
                return Ok(res);
            return BadRequest(new {Message="Invalid Id"});
        }

        [HttpGet("GetStock/{id}")]
        public IActionResult GetStock([FromRoute] int id)
        {
            ProductStock stock = _customerService.GetStock(id);
            if (stock != null)
                return Ok(stock);

            return BadRequest(new { Message = "Product does not exist" });
        }
    }
}
