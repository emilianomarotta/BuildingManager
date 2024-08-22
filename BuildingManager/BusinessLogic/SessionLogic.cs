using Domain;
using IBusinessLogic;
using IBusinessLogic.Exceptions;
using IDataAccess;

namespace BusinessLogic;

public class SessionLogic : ISessionLogic
{
    private readonly IUserRepository<User> _userRepository;
    private readonly IGenericRepository<Session> _sessionRepository;

    private User? _currentUser;

    public SessionLogic(IUserRepository<User> userRepository, IGenericRepository<Session> sessionRepository)
    {
        _userRepository = userRepository;
        _sessionRepository = sessionRepository;
    }

    public User? GetCurrentUser(Guid? token = null)
    {
        if (token == null)
        {
            return _currentUser;
        }
        _currentUser = _userRepository.FindByToken(token.Value);
        return _currentUser;
    }

    public Session GetUserByEmailAndPassword(string email, string password)
    {
        var user = _userRepository.FindByEmailAndPassword(email, password);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }
        Session session = new Session
        {
            User = user,
            UserId = user.Id,
            Role = GetRoleForUser(user)
        };
        return session;
    }

    public Session Create(Session session)
    {
        session.Role = GetRoleForUser(session.User);
        _sessionRepository.Insert(session);
        return session;
    }
    public bool Delete(int id)
    {
        var session = _sessionRepository.Get(session => session.Id == id);
        if (session == null)
        {
            return false;
        }
        _currentUser = GetCurrentUser();
        if (_currentUser.Id != session.UserId)
        {
            throw new UnauthorizedException("Unauthorized to delete this session");
        }
        _sessionRepository.Delete(session);
        return true;
    }
    private string GetRoleForUser(User user)
    {
        if (user is Administrator)
        {
            return "Administrator";
        }
        if (user is Staff)
        {
            return "Staff";
        }
        if (user is CompanyAdmin)
        {
            return "CompanyAdmin";
        }
        return "Manager";
    }
}