using Domain;
using IBusinessLogic;
using IBusinessLogic.Exceptions;
using Moq;
using DTOs.Out;
using DTOs.In;
using Microsoft.AspNetCore.Mvc;
using WebAPI;

namespace TestWebAPI;

[TestClass]
public class OwnerControllerTest
{
    private Mock<IBusinessLogic<Owner>> _ownerLogic;
    private OwnerController _ownerController;
    private Owner owner;

    [TestInitialize]
    public void Setup()
    {
        _ownerLogic = new Mock<IBusinessLogic<Owner>>();
        _ownerController = new OwnerController(_ownerLogic.Object);
        owner = new Owner
        {
            Id = 1,
            Name = "Owner",
            LastName = "Owner",
            Email = "owner@owner.com"
        };
    }
    
    [TestMethod]
    public void GetAllOwners()
    {
        List<Owner> owners = new List<Owner> { owner };
        _ownerLogic.Setup(x => x.GetAll()).Returns(owners);

        var expectedContent = owners.Select(o => new OwnerDetailModel(o)).ToList();

        var result = _ownerController.Index();
        var okResult = result as OkObjectResult;
        var actualContent = okResult.Value as List<OwnerDetailModel>;

        _ownerLogic.VerifyAll();
        CollectionAssert.AreEqual(expectedContent, actualContent);
    }
    
    [TestMethod]
    public void GetOkTest()
    {
        List<Owner> owners = new List<Owner> { owner };
        _ownerLogic.Setup(o => o.GetById(It.IsAny<int>())).Returns(owners.First());
        var expectedOwnerModel = new OwnerDetailModel(owners.First());

        var result = _ownerController.Show(owners.First().Id);
        var okResult = result as OkObjectResult;
        var actualOwnerModel = okResult.Value as OwnerDetailModel;

        _ownerLogic.VerifyAll();
        Assert.AreEqual(expectedOwnerModel, actualOwnerModel);
    }
    
    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public void GetNotFoundTest()
    {
        _ownerLogic.Setup(o => o.GetById(It.IsAny<int>())).Throws(new NotFoundException("Owner not found"));
        var result = _ownerController.Show(1);

        _ownerLogic.VerifyAll();
    }

    [TestMethod]
    public void CreateOwner()
    {
        var ownerCreateModel = new OwnerCreateModel
        {
            Name = "Owner",
            LastName = "Owner",
            Email = "owner@owner.com"
        };
        var ownerToCreate = ownerCreateModel.ToEntity();
        _ownerLogic.Setup(o => o.Create(ownerToCreate)).Returns(owner);

        var expectedOwnerModel = new OwnerDetailModel(owner);

        var result = _ownerController.Create(ownerCreateModel);
        var okResult = result as CreatedAtActionResult;
        var actualOwnerModel = okResult.Value as OwnerDetailModel;

        _ownerLogic.VerifyAll();
        Assert.AreEqual(expectedOwnerModel, actualOwnerModel);
    }

    [TestMethod]
    public void UpdateOwner()
    {
        var ownerCreateModel = new OwnerCreateModel
        {
            Name = "Owner",
            LastName = "Owner",
            Email = "owner@owner.com"
        };
        var ownerToUpdate = ownerCreateModel.ToEntity();
        _ownerLogic.Setup(o => o.Update(It.IsAny<int>(), ownerToUpdate)).Returns(owner);

        var expectedOwnerModel = new OwnerDetailModel(owner);

        var result = _ownerController.Update(owner.Id, ownerCreateModel);
        var okResult = result as OkObjectResult;
        var actualOwnerModel = okResult.Value as OwnerDetailModel;

        _ownerLogic.VerifyAll();
        Assert.AreEqual(expectedOwnerModel, actualOwnerModel);
    }
    
    [TestMethod]
    public void DeleteOwner()
    {
        int ownerId = 1;
        _ownerLogic.Setup(o => o.Delete(It.IsAny<int>())).Returns(true);

        var result = _ownerController.Delete(ownerId);
        var okResult = result as NoContentResult;

        _ownerLogic.VerifyAll();
        Assert.AreEqual(204, okResult.StatusCode);
    }
}