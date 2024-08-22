using Domain;
using IDataAccess;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class UserRepository : IUserRepository<User>
    {
        protected DbContext Context { get; set; }
        public UserRepository(DbContext context)
        {
            Context = context;
        }

        public User? FindByToken(Guid token)
        {
            var session = Context.Set<Session>().FirstOrDefault(s => s.Token == token);
            if(session == null)
            {
                return null;
            }
            var user = FindByID(session.UserId);
            return user;
        }
        private User? FindByID(int id)
        {
            var user = Context.Set<User>().FirstOrDefault(u => u.Id == id);
            return user;
        }
        public User? FindByEmailAndPassword(string email, string password)
        {
            var user = Context.Set<User>().FirstOrDefault(u => u.Email == email && u.Password == password);
            return user;
        }
    }
}