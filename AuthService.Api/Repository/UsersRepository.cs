using AuthService.Api.Models;

namespace AuthService.Api.Repository
{
    public class UsersRepository : IUsersRepository
    {
        private static readonly List<UserRegister> _users = new List<UserRegister>();
        private static int _counter = 2;
        public UsersRepository()
        {
            AddStaticUser();
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

        public bool RegisterUser(UserRegister obj)
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
            return true;
        }

        public bool LoginUser(UserLogin obj)
        {
            string email = obj.Email;
            string password = obj.Password;

            foreach (var user in _users)
            {
                if ((user.Email == email) && (user.Password == password))
                {
                    //return token
                    return true;
                }
            }
            return false;
        }
    }
}
