using AuthenticationService.Api.Models;

namespace AuthenticationService.Api.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterUser(UserRegister obj);
        UserResponseViewModel LoginUser(UserLogin obj);
    }
}
