using Domain;

namespace IBusinessLogic
{
    public interface ISessionLogic
    {
        User? GetCurrentUser(Guid? token = null);
        Session GetUserByEmailAndPassword(string email, string password);
        Session Create(Session entity);
        bool Delete(int id);
    }
}