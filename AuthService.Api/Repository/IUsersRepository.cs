using AuthService.Api.Models;

namespace AuthService.Api.Repository
{
    public interface IUsersRepository
    {
        bool LoginUser(UserLogin obj);
        bool RegisterUser(UserRegister obj);
    }
}