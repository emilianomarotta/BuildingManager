using Domain;
using IBusinessLogic;
using Moq;
using DTOs.Out;
using DTOs.In;
using Microsoft.AspNetCore.Mvc;
using WebAPI;

namespace TestWebAPI;

[TestClass]
public class ApartmentControllerTest
{
    private Mock<IBusinessLogic<Apartment>> _apartmentLogic;
    private ApartmentController _apartmentController;
    private Apartment apartment;
    
    [TestInitialize]
    public void Setup()
    {
        _apartmentLogic = new Mock<IBusinessLogic<Apartment>>(MockBehavior.Strict);
        _apartmentController = new ApartmentController(_apartmentLogic.Object);
        apartment = new Apartment
        {
            Id = 1,
            Floor = 1,
            Number = 1,
            BuildingId = 1,
            OwnerId = 1,
            Bedrooms = 1,
            Bathrooms = 1,
            Balcony = true
        };
    }
    
    [TestMethod]
    public void GetAllApartments()
    {
        List<Apartment> apartments = new List<Apartment> { apartment };
        _apartmentLogic.Setup(x => x.GetAll()).Returns(apartments);

        var expectedContent = apartments.Select(a => new ApartmentDetailModel(a)).ToList();

        var result = _apartmentController.Index();
        var okResult = result as OkObjectResult;
        var actualContent = okResult.Value as List<ApartmentDetailModel>;

        _apartmentLogic.VerifyAll();
        CollectionAssert.AreEqual(expectedContent, actualContent);
    }
    
    [TestMethod]
    public void GetOkTest()
    {
        List<Apartment> apartments = new List<Apartment> { apartment };
        _apartmentLogic.Setup(o => o.GetById(It.IsAny<int>())).Returns(apartments.First());
        var expectedApartmentModel = new ApartmentDetailModel(apartments.First());

        var result = _apartmentController.Show(apartments.First().Id);
        var okResult = result as OkObjectResult;
        var apartmentDetailModel = okResult.Value as ApartmentDetailModel;

        _apartmentLogic.VerifyAll();
        Assert.AreEqual(expectedApartmentModel, apartmentDetailModel);
    }
    
    [TestMethod]
    public void CreateApartment()
    {
        ApartmentCreateModel apartment = new ApartmentCreateModel
        {
            Floor = 1,
            Number = 1,
            BuildingId = 1,
            OwnerId = 1,
            Bedrooms = 1,
            Bathrooms = 1,
            Balcony = true
        };
        _apartmentLogic.Setup(o => o.Create(It.IsAny<Apartment>())).Returns(apartment.ToEntity());
        var expectedApartmentModel = new ApartmentDetailModel(apartment.ToEntity());

        var result = _apartmentController.Create(apartment);
        var okResult = result as CreatedAtActionResult;
        var apartmentDetailModel = okResult.Value as ApartmentDetailModel;

        _apartmentLogic.VerifyAll();
        Assert.AreEqual(expectedApartmentModel, apartmentDetailModel);
    }
    
    [TestMethod]
    public void UpdateApartment()
    {
        ApartmentPutModel apartment;
        apartment = new ApartmentPutModel
        {
            OwnerId = 1,
        };
        _apartmentLogic.Setup(o => o.Update(It.IsAny<int>(), apartment.ToEntity())).Returns(apartment.ToEntity());
        var expectedApartmentModel = new ApartmentDetailModel(apartment.ToEntity());

        var result = _apartmentController.Update(apartment.ToEntity().Id, apartment);
        var okResult = result as OkObjectResult;
        var apartmentDetailModel = okResult.Value as ApartmentDetailModel;
        _apartmentLogic.VerifyAll();
        Assert.AreEqual(expectedApartmentModel, apartmentDetailModel);
    }
    
    [TestMethod]
    public void DeleteBuilding()
    {
        _apartmentLogic.Setup(x => x.Delete(It.IsAny<int>())).Returns(true);

        var result = _apartmentController.Delete(apartment.Id);
        var noContentResult = result as NoContentResult;

        _apartmentLogic.VerifyAll();
        Assert.IsNotNull(noContentResult);
    }
}