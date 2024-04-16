using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Api.Models;
using ProductService.Api.Services;

namespace ProductService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles  ="Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService; 
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromBody]Product product)
        {
            if(ModelState.IsValid)
            {
                bool res=await _adminService.AddProduct(product);
                if (res)
                {
                    return Ok(new {Message="Product Added Successfully"});
                }
            }
            ModelState.AddModelError("", "invalid");
            return BadRequest(ModelState);
        }
        [HttpGet("GetProducts")]
        public IActionResult GetProducts()
        {
            return Ok(_adminService.GetAllProducts());
        }

        [HttpPost("RemoveProduct/{id}")]
        public async Task<IActionResult> RemoveProduct([FromRoute]int id)
        {
            bool res=await _adminService.RemoveProduct(id);
            if (res)
            {
                return Ok(new { Message = "Product removed successfully" });
            }
            return BadRequest(new { Message = "Invalid ID" });
        }

        [HttpPost("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] int id,[FromBody]Product product)
        {
            bool res=await _adminService.UpdateProduct(id, product);
            if (res)
            {
                return Ok(new { Message = "Product updated successfully" });
            }
            return BadRequest(new { Message = "Could not update" });
        }

        
    }

}
