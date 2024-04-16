using AuthenticationService.Api.Models;
using AuthenticationService.Api.Repository;
using AuthenticationService.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authenticationService;
        
        public AuthenticationController(IAuthService authenticationService)
        {
            _authenticationService = authenticationService;
            
        }

        [HttpPost("Register")]
        public async  Task<IActionResult> Register([FromBody] UserRegister obj)
        {
            if(ModelState.IsValid)
            {
                bool res=await _authenticationService.RegisterUser(obj);
                if(res)
                {
                    return Ok(new { Message = "User Registered Successfully" });
                }
                else
                {
                    return BadRequest(new { Message = "User Already Exists." });
                }

            }
            ModelState.AddModelError("", "invalid");
            return BadRequest(ModelState);

        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserLogin obj)
        {
            if (ModelState.IsValid)
            {
                UserResponseViewModel res=_authenticationService.LoginUser(obj);
                return Ok(res);
            }
            ModelState.AddModelError("", "invalid");
            return BadRequest(ModelState);

        }
    }
}
