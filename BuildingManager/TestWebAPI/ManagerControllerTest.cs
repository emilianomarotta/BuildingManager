using Domain;
using IBusinessLogic;
using Moq;
using DTOs.Out;
using DTOs.In;
using Microsoft.AspNetCore.Mvc;
using WebAPI;

namespace TestWebAPI;

[TestClass]
public class ManagerControllerTest
{
    private Mock<IBusinessLogic<Manager>> _managerLogic;
    private ManagerController _managerController;
    private Manager manager;


    [TestInitialize]
    public void Setup()
    {
        _managerLogic = new Mock<IBusinessLogic<Manager>>(MockBehavior.Strict);
        _managerController = new ManagerController(_managerLogic.Object);
        manager = new Manager
        {
            Id = 1,
            Name = "Manager",
            LastName = "Manager",
            Email = "manager@manager.com"
        };
    }

    [TestMethod]
    public void GetAllManagers()
    {
        List<Manager> managers = new List<Manager> { manager };
        _managerLogic.Setup(x => x.GetAll()).Returns(managers);

        var expectedContent = managers.Select(a => new ManagerDetailModel(a)).ToList();

        var result = _managerController.Index();
        var okResult = result as OkObjectResult;
        var actualContent = okResult.Value as List<ManagerDetailModel>;

        _managerLogic.VerifyAll();
        CollectionAssert.AreEqual(expectedContent, actualContent);
    }

    [TestMethod]
    public void GetOkTest()
    {
        List<Manager> managers = new List<Manager> { manager };
        _managerLogic.Setup(o => o.GetById(It.IsAny<int>())).Returns(managers.First());
        var expectedManagerModel = new ManagerDetailModel(managers.First());

        var result = _managerController.Show(managers.First().Id);
        var okResult = result as OkObjectResult;
        var managerDetailModel = okResult.Value as ManagerDetailModel;

        _managerLogic.VerifyAll();
        Assert.AreEqual(expectedManagerModel, managerDetailModel);
    }

    [TestMethod]
    public void UpdateManager()
    {
        ManagerPutModel manager;
        manager = new ManagerPutModel
        {
            Name = "Diego",
            LastName = "Maradona",
            Email = "anotherEmail@test.com",
            Password = "Test.56789"
        };
        _managerLogic.Setup(o => o.Update(It.IsAny<int>(), manager.ToEntity())).Returns(manager.ToEntity());
        var expectedManagerModel = new ManagerDetailModel(manager.ToEntity());

        var result = _managerController.Update(manager.ToEntity().Id, manager);
        var okResult = result as OkObjectResult;
        var managerDetailModel = okResult.Value as ManagerDetailModel;
        _managerLogic.VerifyAll();
        Assert.AreEqual(expectedManagerModel, managerDetailModel);
    }

    [TestMethod]
    public void DeleteManager()
    {
        _managerLogic.Setup(o => o.Delete(It.IsAny<int>())).Returns(true);
        var result = _managerController.Delete(manager.Id);
        var noContent = result as NoContentResult;
        _managerLogic.VerifyAll();
        Assert.AreEqual(204, noContent.StatusCode);
    }

    [TestMethod]
    public void DeleteNotFoundManager()
    {
        _managerLogic.Setup(o => o.Delete(It.IsAny<int>())).Returns(false);
        var result = _managerController.Delete(manager.Id);
        var notFound = result as NotFoundObjectResult;
        _managerLogic.VerifyAll();
        Assert.AreEqual(404, notFound.StatusCode);
    }
}