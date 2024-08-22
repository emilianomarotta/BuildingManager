using Domain;
using BusinessLogic;
using IBusinessLogic.Exceptions;
using IDataAccess;
using Moq;

namespace TestBusinessLogic;

[TestClass]
public class SessionLogicTest
{
    private Mock<IUserRepository<User>> _userRepository;
    private Mock<IGenericRepository<Session>> _sessionRepository;
    private SessionLogic _sessionLogic;
    private User _user;
    private Session _session;


    [TestInitialize]
    public void Setup()
    {
        _userRepository = new Mock<IUserRepository<User>>(MockBehavior.Strict);
        _sessionRepository = new Mock<IGenericRepository<Session>>(MockBehavior.Strict);
        _sessionLogic = new SessionLogic(_userRepository.Object, _sessionRepository.Object);
        _user = new Administrator
        {
            Id = 1,
            Name = "User",
            LastName = "User",
            Email = "test@test.com",
            Password = "Test.1234"
        };
        _session = new Session
        {
            Id = 1,
            User = _user,
            UserId = _user.Id
        };
    }

    [TestMethod]
    public void GetCurrentUserTest()
    {
        _userRepository.Setup(x => x.FindByToken(It.IsAny<Guid>())).Returns(_user);
        var result = _sessionLogic.GetCurrentUser(Guid.NewGuid());
        Assert.AreEqual(_user, result);
        _userRepository.VerifyAll();
    }

    [TestMethod]
    public void GetUserByEmailAndPasswordTest()
    {
        _userRepository.Setup(x => x.FindByEmailAndPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(_user);
        string email = _user.Email;
        string password = _user.Password;
        var result = _sessionLogic.GetUserByEmailAndPassword(email, password);
        Assert.AreEqual(_session.Id, result.User.Id);
        _userRepository.VerifyAll();
    }
    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public void GetUserByEmailAndPasswordWrongTest()
    {
        _userRepository.Setup(x => x.FindByEmailAndPassword(It.IsAny<string>(), It.IsAny<string>())).Returns((User)null);
        string email = _user.Email;
        string password = _user.Password;
        var result = _sessionLogic.GetUserByEmailAndPassword(email, password);
    }

    [TestMethod]
    public void CreateTest()
    {
        _sessionRepository.Setup(x => x.Insert(It.IsAny<Session>()));
        var result = _sessionLogic.Create(_session);
        Assert.AreEqual(_session.Id, result.Id);
        _sessionRepository.VerifyAll();
    }
    [TestMethod]
    public void DeleteNotFoundSession()
    {
        int sessionId = _session.Id;
        _sessionRepository.Setup(x => x.Get(x => x.Id == sessionId, null)).Returns((Session)null);
        var result = _sessionLogic.Delete(sessionId);
        Assert.IsFalse(result);
        _sessionRepository.VerifyAll();
    }

}

