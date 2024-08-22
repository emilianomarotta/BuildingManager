using Domain;
using BusinessLogic;
using IBusinessLogic.Exceptions;
using IDataAccess;
using Moq;
using System.ComponentModel.Design;
using IBusinessLogic;
using DTOs.In;
using System.Linq.Expressions;

namespace TestBusinessLogic;

[TestClass]
public class BuildingLogicTest
{
    private Mock<IGenericRepository<Building>> _buildingRepository;
    private Mock<IGenericRepository<Manager>> _managerRepository;
    private Mock<IGenericRepository<Company>> _companyRepository;
    private Mock<ISessionLogic> _sessionLogic;
    private BuildingLogic? _buildingLogic;
    private Building building;
    private Company company;
    private Manager manager;
    private CompanyAdmin companyAdmin;


    [TestInitialize]
    public void Setup()
    {
        _buildingRepository = new Mock<IGenericRepository<Building>>(MockBehavior.Strict);
        _managerRepository = new Mock<IGenericRepository<Manager>>(MockBehavior.Strict);
        _companyRepository = new Mock<IGenericRepository<Company>>(MockBehavior.Strict);
        _sessionLogic = new Mock<ISessionLogic>(MockBehavior.Strict);

        var dto = new BuildingLogicDTO(
            _buildingRepository.Object,
            _managerRepository.Object,
            _companyRepository.Object,
            _sessionLogic.Object);

        _buildingLogic = new BuildingLogic(dto);

        companyAdmin = new CompanyAdmin
        {
            Id = 2
        };
        company = new Company
        {
            Id = 1,
            Name = "name",
            CompanyAdminId = companyAdmin.Id,
            companyAdmin = companyAdmin
        };
        manager = new Manager
        {
            Id = 1
        };
        building = new Building
        {
            Id = 1,
            Name = "Test",
            ManagerId = 1,
            Manager = manager,
            Address = "Test, Test 1234",
            Location = "-12.3456, -12.3456",
            CompanyId = 1,
            Company = company,
            Fees = 1234
        };
    }

    [TestMethod]
    public void CreateBuilding()
    {
        int? managerId = 1;
        int companyId = 1;
        int companyAdminId = companyAdmin.Id;
        List<Building> buildings = new List<Building>();
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)companyAdmin);
        _buildingRepository!.Setup(a => a.Insert(building!));
        _buildingRepository!.Setup(a => a.GetAll<Building>()).Returns(buildings);
        _managerRepository!.Setup(m => m.Get(o => o.Id == managerId, null)).Returns(manager);
        _companyRepository!.Setup(c => c.Get(o => o.Id == companyId, null)).Returns(company!);
        _companyRepository.Setup(c => c.Get(o => o.CompanyAdminId == companyAdminId, null)).Returns(company);
        var newBuilding = _buildingLogic!.Create(building!);

        _buildingRepository.VerifyAll();
        Assert.AreEqual(building, newBuilding);
    }
    [TestMethod]
    public void CreateBuildingWithoutManager()
    {
        int? managerId = 1;
        int companyId = 1;
        int companyAdminId = companyAdmin.Id;
        building.ManagerId = null;
        building.Manager = null;
        List<Building> buildings = new List<Building>();
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)companyAdmin);
        _buildingRepository!.Setup(a => a.Insert(building!));
        _buildingRepository!.Setup(a => a.GetAll<Building>()).Returns(buildings);
        _companyRepository!.Setup(c => c.Get(o => o.Id == companyId, null)).Returns(company!);
        _companyRepository.Setup(c => c.Get(o => o.CompanyAdminId == companyAdminId, null)).Returns(company);
        var newBuilding = _buildingLogic!.Create(building!);

        _buildingRepository.VerifyAll();
        Assert.IsNull(building.Manager);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void CreateBuildingAlreadyExist()
    {
        List<Building> buildings = new List<Building> { building };
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        _buildingRepository!.Setup(a => a.Insert(building!));
        _buildingRepository!.Setup(a => a.GetAll<Building>()).Returns(buildings);
        var newBuilding = _buildingLogic!.Create(building!);

        _buildingRepository.VerifyAll();
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public void CreateBuildingNotFoundManager()
    {
        int? managerId = 1;
        int companyId = 1;
        List<Building> buildings = new List<Building>();
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        _buildingRepository!.Setup(a => a.Insert(building!));
        _buildingRepository!.Setup(a => a.GetAll<Building>()).Returns(buildings);
        _managerRepository!.Setup(m => m.Get(o => o.Id == managerId, null)).Returns((Manager)null);
        _companyRepository!.Setup(c => c.Get(o => o.Id == companyId, null)).Returns(company!);

        var newBuilding = _buildingLogic!.Create(building!);

        _buildingRepository.VerifyAll();
        Assert.AreEqual(building, newBuilding);
    }

    [TestMethod]
    public void GetAllBuildings()
    {
        List<Building> buildings = new List<Building> { building };
        int companyAdminId = companyAdmin.Id;
        int? managerId = building.ManagerId;
        int companyId = company.Id;
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)companyAdmin);
        _companyRepository.Setup(c => c.Get(o => o.CompanyAdminId == companyAdminId, null)).Returns(company);
        _companyRepository.Setup(c => c.Get(o => o.Id == companyId, null)).Returns(company);
        _buildingRepository!.Setup(a => a.GetAll<Building>()).Returns(buildings);
        _managerRepository!.Setup(m => m.Get(o => o.Id == managerId, null)).Returns(manager);
        List<Building> actualBuildings = _buildingLogic.GetAll();

        _buildingRepository.VerifyAll();
        CollectionAssert.AreEqual(buildings, actualBuildings);
    }

    [TestMethod]
    public void GetBuildingById()
    {
        int buildingId = 1;
        int companyAdminId = 2;
        int? managerId = 1;
        int? managerBuildingId = 1;
        int companyId = 1;
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)companyAdmin);
        _companyRepository.Setup(c => c.Get(o => o.CompanyAdminId == companyAdminId, null)).Returns(company);
        _companyRepository.Setup(c => c.Get(o => o.Id == companyId, null)).Returns(company);
        _managerRepository!.Setup(m => m.Get(o => o.Id == managerId, null)).Returns(manager);
        _buildingRepository.Setup(o => o.Get(It.Is<Expression<Func<Building, bool>>>(expr => expr.Compile()(building)), null)).Returns(building);
        Building returnedBuilding = _buildingLogic.GetById(buildingId);

        _buildingRepository.VerifyAll();

        Assert.AreEqual(building, returnedBuilding);
    }

    [TestMethod]
    public void GetNonExistingBuildingById()
    {
        int buildingId = 1;
        int companyAdminId = 2;
        int companyId = 1;
        int managerId = 1;
        int? managerBuildingId = 1;
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)companyAdmin);
        _companyRepository.Setup(c => c.Get(o => o.CompanyAdminId == companyAdminId, null)).Returns(company);
        _buildingRepository.Setup(o => o.Get(
            It.Is<Expression<Func<Building, bool>>>(expr =>
                expr.Compile()(building)
            ),
            null
        )).Returns((Building)null);
        Building returnedBuilding = _buildingLogic.GetById(buildingId);

        _buildingRepository.VerifyAll();

        Assert.IsNull(returnedBuilding);
    }

    [TestMethod]
    public void UpdateBuildingInformation()
    {
        int buildingId = 1;
        int newCompanyId = 2;
        Company newCompany = new Company
        {
            Id = 2
        };
        Building buildingCopy = new Building
        {
            Id = 1,
            Name = "Name",
            Manager = manager,
            Address = "Test, Test 1234",
            Location = "-12.3456, -12.3456",
            CompanyId = 2,
            Company = newCompany,
            Fees = 4000
        };
        int companyAdminId = companyAdmin.Id;
        int companyId = company.Id;
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)companyAdmin);
        _companyRepository.Setup(c => c.Get(o => o.CompanyAdminId == companyAdminId, null)).Returns(company);
        _companyRepository!.Setup(c => c.Get(o => o.Id == newCompanyId, null)).Returns(newCompany);
        _buildingRepository.Setup(o => o.Get(o => o.Id == buildingId, null)).Returns(building);
        _buildingRepository.Setup(o => o.Update(building));

        Building updatedBuilding = _buildingLogic.Update(buildingId, buildingCopy);

        _buildingRepository.VerifyAll();
        Assert.AreEqual(buildingCopy.Fees, updatedBuilding.Fees);
    }

    [TestMethod]
    public void DeleteExistingBuilding()
    {
        int buildingId = 1;
        int companyAdminId = companyAdmin.Id;
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)companyAdmin);
        _companyRepository.Setup(c => c.Get(o => o.CompanyAdminId == companyAdminId, null)).Returns(company);
        _buildingRepository.Setup(o => o.Get(o => o.Id == buildingId, null)).Returns(building);
        _buildingRepository.Setup(o => o.Delete(building));
        bool buildingDeleted = _buildingLogic.Delete(buildingId);

        _buildingRepository.VerifyAll();
        Assert.IsTrue(buildingDeleted);
    }

    [TestMethod]
    public void DeleteNonExistingBuilding()
    {
        int buildingId = 1;
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        _buildingRepository.Setup(o => o.Get(o => o.Id == buildingId, null)).Returns((Building)null);

        bool buildingDeleted = _buildingLogic.Delete(buildingId);

        _buildingRepository.VerifyAll();
        Assert.IsFalse(buildingDeleted);
    }


}
