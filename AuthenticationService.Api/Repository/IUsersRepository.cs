using AuthenticationService.Api.Models;

namespace AuthenticationService.Api.Repository
{
    public interface IUsersRepository
    {
        UserResponseViewModel LoginUser(UserLogin obj);
        Task<bool> RegisterUser(UserRegister obj);
    }
}