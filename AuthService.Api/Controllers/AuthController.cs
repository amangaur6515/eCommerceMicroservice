using AuthService.Api.Models;
using AuthService.Api.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;
        public AuthController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] UserRegister obj)
        {
            if (ModelState.IsValid)
            {
                bool res = _usersRepository.RegisterUser(obj);
                if (res)
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
                bool res = _usersRepository.LoginUser(obj);
                if (res)
                {
                    return Ok();
                }
            }
            ModelState.AddModelError("", "invalid");
            return BadRequest(ModelState);

        }
    }
}
