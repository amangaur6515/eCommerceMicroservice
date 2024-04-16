using AuthenticationService.Api.Models;
using AuthenticationService.Api.Repository;
using AuthenticationService.Api.Services;

namespace AuthenticationService.Api.Services
{
    public class AuthService:IAuthService
    {
        private readonly IUsersRepository _usersRepository;
        public AuthService(IUsersRepository usersRepository)
        {
            _usersRepository =usersRepository;
        }
        public async Task<bool> RegisterUser(UserRegister obj)
        {
            
            bool res=await _usersRepository.RegisterUser(obj);
            return res; 
        }
        public UserResponseViewModel LoginUser(UserLogin obj)
        {
            UserResponseViewModel res=_usersRepository.LoginUser(obj);
            return res;
        }
    }
}
