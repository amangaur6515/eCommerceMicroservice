using AuthenticationService.Api.Models;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Eventing.Reader;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserRegisteredEntity.cs;

namespace AuthenticationService.Api.Repository
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IConfiguration _configuration;
        private static readonly List<UserRegister> _users = new List<UserRegister>();
        private static int _counter = 2;
        public UsersRepository(IConfiguration configuration,IPublishEndpoint publishEndpoint)
        {
            AddStaticUser();
            _configuration = configuration;
            _publishEndpoint = publishEndpoint;
        }
        private void AddStaticUser()
        {
            UserRegister user1 = new UserRegister()
            {
                Id = 1,
                Name = "Aman Gaur",
                Email = "amangaur6515@gmail.com",
                Password = "Aman@12345",
                Role = "Admin"
            };
            UserRegister user2 = new UserRegister()
            {
                Id = 2,
                Name = "Test",
                Email = "test@gmail.com",
                Password = "Test@12345",
                Role = "Customer"
            };
            _users.Add(user1);
            _users.Add(user2);
        }
        public async Task<bool> RegisterUser(UserRegister obj)
        {
            string email = obj.Email;
            foreach (var user in _users)
            {
                if (user.Email == email)
                {
                    return false;
                }
            }
            obj.Id = _counter + 1;
            _counter = _counter + 1;
            _users.Add(obj);
            //publish message to assign cart
            await _publishEndpoint.Publish<User>(obj);
            return true;
        }
        public UserResponseViewModel LoginUser(UserLogin obj)
        {
            string email = obj.Email;
            string password = obj.Password;

            foreach (var user in _users)
            {
                if ((user.Email == email) && (user.Password == password))
                {
                    //generate token
                    string token =  GenerateToken(obj);
                    return new UserResponseViewModel
                    {
                        Message = token,
                        IsSuccess = true,

                    };
                   
                }
            }
            return new UserResponseViewModel
            {
                Message = "Invalid credential",
                IsSuccess = false,

            };
        }

        private string  GenerateToken(UserLogin userObj)
        {
            //find the  user 
            var existingUser = new UserRegister();
            foreach (var user in _users)
            {
                if (user.Email == userObj.Email)
                {
                    existingUser= user;
                }
            }
            // Get the roles of the user
            var roles = existingUser.Role;
            //create claims
            var claimsIdentity = new ClaimsIdentity( new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, userObj.Email),
                new Claim(ClaimTypes.Role, roles),
            });
            

            //get the key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claimsIdentity.Claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenAsString;
        }

        
    }
}
