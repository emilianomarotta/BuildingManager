using Domain;
using IBusinessLogic;
using BusinessLogic;
using Moq;
using DTOs.Out;
using DTOs.In;
using Microsoft.AspNetCore.Mvc;
using WebAPI;

namespace TestWebAPI;
[TestClass]
public class UserControllerTest
{
    private Mock<UserLogic> _userLogic;
    private Mock<ISessionLogic> _sessionLogic;
    private UserController _userController;
    private User user;
    private Session session;

    [TestInitialize]
    public void Setup()
    {
        _sessionLogic = new Mock<ISessionLogic>(MockBehavior.Strict);
        _userLogic = new Mock<UserLogic>();
        _userController = new UserController(_sessionLogic.Object, _userLogic.Object);
        user = new Administrator
        {
            Id = 1,
            Name = "admin",
            LastName = "lastname",
            Email = "test@test.com",
            Password = "Test.1234"
        };
        session = new Session
        {
            Id = 1,
            Token = new Guid(),
            UserId = 1,
            User = user

        };
    }

    [TestMethod]
    public void CreateOkSession()
    {
        SessionCreateModel sessionCreateModel = new SessionCreateModel
        {
            Email = "test@test.com",
            Password = "Test.1234"
        };
        string email = sessionCreateModel.Email;
        string password = sessionCreateModel.Password;
        _userLogic.Setup(x => x.GetByEmailAndPassword(sessionCreateModel.Email, sessionCreateModel.Password)).Returns(user);
        _sessionLogic.Setup(x => x.Create(It.IsAny<Session>())).Returns(session);
        var expectedSessionModel = new SessionDetailModel(session);
        var result = _userController.Create(sessionCreateModel);
        var okResult = result as CreatedAtActionResult;
        var sessionDetailModel = okResult.Value as SessionDetailModel;
        Assert.AreEqual(expectedSessionModel, sessionDetailModel);
    }

    [TestMethod]
    public void DeleteSession()
    {
        _sessionLogic.Setup(x => x.Delete(It.IsAny<int>())).Returns(true);
        var result = _userController.Delete(session.Id);
        var noContent = result as NoContentResult;
        _userLogic.VerifyAll();
        Assert.AreEqual(204, noContent.StatusCode);
    }

    [TestMethod]
    public void DeleteNotFoundSession()
    {
        _sessionLogic.Setup(x => x.Delete(It.IsAny<int>())).Returns(false);
        var result = _userController.Delete(session.Id);
        var notFound = result as NotFoundObjectResult;
        _userLogic.VerifyAll();
        Assert.AreEqual(404, notFound.StatusCode);
    }
}

