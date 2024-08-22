using Domain;
using IBusinessLogic;
using Moq;
using DTOs.Out;
using DTOs.In;
using Microsoft.AspNetCore.Mvc;
using WebAPI;

namespace TestWebAPI;

[TestClass]
public class BuildingControllerTest
{
    private Mock<IBusinessLogic<Building>> _buildingLogic;
    private BuildingController _buildingController;
    private Building building;

    [TestInitialize]
    public void Setup()
    {
        _buildingLogic = new Mock<IBusinessLogic<Building>>(MockBehavior.Strict);
        _buildingController = new BuildingController(_buildingLogic.Object);
        building = new Building
        {
            Id = 1,
            Name = "Building",
            Address = "City, Address 1234",
            Company = new Company { Id = 1 },
            Manager = new Manager { Id = 1 },
            Fees = 100,
            Location = "-12.3456, -12.3456"
        };
    }

    [TestMethod]
    public void GetAllBuildings()
    {
        List<Building> buildings = new List<Building> { building };
        _buildingLogic.Setup(x => x.GetAll()).Returns(buildings);

        var expectedContent = buildings.Select(b => new BuildingDetailModel(b)).ToList();

        var result = _buildingController.Index();
        var okResult = result as OkObjectResult;
        var actualContent = okResult.Value as List<BuildingDetailModel>;

        _buildingLogic.VerifyAll();
        CollectionAssert.AreEqual(expectedContent, actualContent);
    }

    [TestMethod]
    public void GetOkTest()
    {
        List<Building> buildings = new List<Building> { building };
        _buildingLogic.Setup(o => o.GetById(It.IsAny<int>())).Returns(buildings.First());
        var expectedBuildingModel = new BuildingDetailModel(buildings.First());

        var result = _buildingController.Show(buildings.First().Id);
        var okResult = result as OkObjectResult;
        var actualBuildingModel = okResult.Value as BuildingDetailModel;

        _buildingLogic.VerifyAll();
        Assert.AreEqual(expectedBuildingModel, actualBuildingModel);
    }

    [TestMethod]
    public void CreateBuilding()
    {
        var buildingCreateModel = new BuildingCreateModel
        {
            Name = "Building",
            Address = "City, Address 1234",
            CompanyId = 1,
            ManagerId = 1,
            Fees = 100,
            Location = "-12.3456, -12.3456"
        };
        _buildingLogic.Setup(x => x.Create(It.IsAny<Building>())).Returns(building);

        var expectedBuildingModel = new BuildingDetailModel(building);

        var result = _buildingController.Create(buildingCreateModel);
        var okResult = result as OkObjectResult;
        var actualBuildingModel = okResult.Value as BuildingDetailModel;

        _buildingLogic.VerifyAll();
        Assert.AreEqual(expectedBuildingModel, actualBuildingModel);
    }

    [TestMethod]
    public void UpdateBuilding()
    {
        var buildingPutModel = new BuildingPutModel
        {
            Fees = 100,
        };
        _buildingLogic.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<Building>())).Returns(building);

        var expectedBuildingModel = new BuildingDetailModel(building);

        var result = _buildingController.Update(building.Id, buildingPutModel);
        var okResult = result as OkObjectResult;
        var actualBuildingModel = okResult.Value as BuildingDetailModel;

        _buildingLogic.VerifyAll();
        Assert.AreEqual(expectedBuildingModel, actualBuildingModel);
    }

    [TestMethod]
    public void DeleteBuilding()
    {
        _buildingLogic.Setup(x => x.Delete(It.IsAny<int>())).Returns(true);

        var result = _buildingController.Delete(building.Id);
        var noContentResult = result as NoContentResult;

        _buildingLogic.VerifyAll();
        Assert.IsNotNull(noContentResult);
    }
}