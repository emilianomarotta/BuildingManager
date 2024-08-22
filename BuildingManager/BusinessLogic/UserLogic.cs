using Domain;
using IDataAccess;

namespace BusinessLogic
{
    public class UserLogic
    {

        private readonly IUserRepository<User> _userRepository;
        public UserLogic()
        {
        }
        public UserLogic(IUserRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public virtual User GetByEmailAndPassword(string email, string password)
        {
            return _userRepository.FindByEmailAndPassword(email, password);
        }
    }
}
