using Domain;
using BusinessLogic;
using IBusinessLogic.Exceptions;
using IDataAccess;
using Moq;
using IBusinessLogic;
using DTOs.In;

namespace TestBusinessLogic;

[TestClass]
public class ApartmentLogicTest
{
    private Mock<IGenericRepository<Apartment>> _apartmentRepository;
    private Mock<IGenericRepository<Building>> _buildingRepository;
    private Mock<IGenericRepository<Owner>> _ownerRepository;
    private Mock<ISessionLogic> _sessionLogic;
    private ApartmentLogic _apartmentLogic;
    private Apartment apartment;
    private Building building;
    private Owner owner;
    private Manager manager;
    private Company company;

    [TestInitialize]
    public void Initialize()
    {
        _buildingRepository = new Mock<IGenericRepository<Building>>(MockBehavior.Strict);
        _apartmentRepository = new Mock<IGenericRepository<Apartment>>(MockBehavior.Strict);
        _ownerRepository = new Mock<IGenericRepository<Owner>>(MockBehavior.Strict);
        _sessionLogic = new Mock<ISessionLogic>();

        var dto = new ApartmentLogicDTO(
            _apartmentRepository.Object,
            _buildingRepository.Object,
            _ownerRepository.Object,
            _sessionLogic.Object);

        _apartmentLogic = new ApartmentLogic(dto);

        company = new Company
        {
            Id = 1,
            Name = "Company"
        };

        manager = new Manager
        {
            Id = 1,
            Email = "manager@manager.com",
            Name = "Manager",
            LastName = "Manager",
            Password = "Test.1234"
        };

        building = new Building
        {
            Id = 1,
            Name = "Building",
            Address = "City, Street 1234",
            Location = "-12.345678, -12.345678",
            Manager = manager,
            ManagerId = manager.Id,
            Company = company,
            Fees = 100,
            Apartments = new List<Apartment>()
        };

        owner = new Owner
        {
            Id = 1,
            Email = "owner@owner.com",
            Name = "Owner",
            LastName = "Owner"
        };

        apartment = new Apartment
        {
            Id = 1,
            Building = building,
            BuildingId = building.Id,
            Owner = owner,
            OwnerId = owner.Id,
            Number = 1,
            Floor = 1,
            Bedrooms = 1,
            Bathrooms = 1,
            Balcony = true
        };
    }

    [TestMethod]
    public void GetAllApartments()
    {
        List<Apartment> apartments = new List<Apartment> { apartment };
        List<Building> buildings = new List<Building> { building };
        _apartmentRepository!.Setup(a => a.GetAll<Apartment>()).Returns(apartments);
        _buildingRepository!.Setup(a => a.GetAll<Building>()).Returns(buildings);
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        var result = _apartmentLogic!.GetAll();

        CollectionAssert.AreEqual(apartments, result);
    }

    [TestMethod]
    public void GetApartmentById()
    {
        int apartmentId = 1;
        int buildingId = 1;
        List<Building> buildings = new List<Building> { building };
        _apartmentRepository!.Setup(a => a.Get(b => b.Id == apartmentId, null)).Returns(apartment);
        _buildingRepository!.Setup(a => a.Get(b => b.Id == buildingId, null)).Returns(building);
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        var result = _apartmentLogic!.GetById(apartmentId);
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);


        Assert.AreEqual(apartment, result);
    }

    [TestMethod]
    public void CreateApartment()
    {
        int apartmentId = 1;
        int buildingId = 1;
        int ownerId = 1;
        List<Apartment> apartments = new List<Apartment>();
        _apartmentRepository!.Setup(a => a.Insert(apartment!));
        _apartmentRepository!.Setup(a => a.Get(b => b.Id == apartmentId, null)).Returns((Apartment)null);
        _buildingRepository!.Setup(a => a.Get(b => b.Id == buildingId, null)).Returns(building);
        _ownerRepository!.Setup(c => c.Get(o => o.Id == ownerId, null)).Returns(owner!);
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        var newApartment = _apartmentLogic!.Create(apartment!);

        _buildingRepository.VerifyAll();
        Assert.AreEqual(apartment, newApartment);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public void CreateApartmentBuildingNotFound()
    {
        int apartmentId = 1;
        int buildingId = 1;
        int ownerId = 1;
        List<Apartment> apartments = new List<Apartment>();
        _apartmentRepository!.Setup(a => a.Get(b => b.Id == apartmentId, null)).Returns((Apartment)null);
        _buildingRepository!.Setup(a => a.Get(b => b.Id == buildingId, null)).Returns((Building)null);
        _ownerRepository!.Setup(c => c.Get(o => o.Id == ownerId, null)).Returns(owner!);
        _apartmentLogic!.Create(apartment!);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public void CreateApartmentOwnerNotFound()
    {
        int apartmentId = 1;
        int buildingId = 1;
        int ownerId = 1;
        List<Apartment> apartments = new List<Apartment>();
        _apartmentRepository!.Setup(a => a.Get(b => b.Id == apartmentId, null)).Returns((Apartment)null);
        _buildingRepository!.Setup(a => a.Get(b => b.Id == buildingId, null)).Returns(building);
        _ownerRepository!.Setup(c => c.Get(o => o.Id == ownerId, null)).Returns((Owner)null);
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        _apartmentLogic!.Create(apartment!);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void CreateApartmentAlreadyExists()
    {
        int apartmentId = 1;
        int buildingId = 1;
        int ownerId = 1;
        _apartmentRepository!.Setup(a => a.Get(b => b.Id == apartmentId, null)).Returns(apartment);
        _buildingRepository!.Setup(a => a.Get(b => b.Id == buildingId, null)).Returns(building);
        _ownerRepository!.Setup(c => c.Get(o => o.Id == ownerId, null)).Returns(owner!);
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        _apartmentLogic!.Create(apartment!);
    }

    [TestMethod]
    public void UpdateApartment()
    {
        int apartmentId = 1;
        int buildingId = 1;
        int newOwnerId = 2;

        Owner newOwner = new Owner
        {
            Id = 2,
            Name = "newOwner",
            LastName = "newOwner",
            Email = "newOwner@newOwner.com"
        };

        Apartment updatedApartment = new Apartment
        {
            Id = 1,
            Building = building,
            BuildingId = building.Id,
            Owner = newOwner,
            OwnerId = newOwner.Id,
            Number = 1,
            Floor = 1,
            Bedrooms = 1,
            Bathrooms = 1,
            Balcony = true
        };

        _apartmentRepository!.Setup(a => a.Get(b => b.Id == apartmentId, null)).Returns(apartment);
        _buildingRepository!.Setup(a => a.Get(b => b.Id == buildingId, null)).Returns(building);
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        _ownerRepository!.Setup(c => c.Get(o => o.Id == newOwnerId, null)).Returns(newOwner!);
        _apartmentRepository!.Setup(o => o.Update(apartment!));
        var newApartment = _apartmentLogic!.Update(apartmentId, updatedApartment!);

        _buildingRepository.VerifyAll();
        Assert.AreEqual(apartment, newApartment);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public void UpdateApartmentNotFound()
    {
        int apartmentId = 1;
        _apartmentRepository!.Setup(a => a.Get(b => b.Id == apartmentId, null)).Returns((Apartment)null);
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        _apartmentLogic!.Update(apartmentId, apartment!);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public void UpdateApartmentOwnerNotFound()
    {
        int apartmentId = 1;
        int buildingId = 1;
        int newOwnerId = 2;

        Owner newOwner = new Owner
        {
            Id = 2,
            Name = "newOwner",
            LastName = "newOwner",
            Email = "newOwner@newOwner.com"
        };

        Apartment updatedApartment = new Apartment
        {
            Id = 1,
            Building = building,
            BuildingId = building.Id,
            Owner = newOwner,
            OwnerId = newOwner.Id,
            Number = 1,
            Floor = 1,
            Bedrooms = 1,
            Bathrooms = 1,
            Balcony = true
        };

        _apartmentRepository!.Setup(a => a.Get(b => b.Id == apartmentId, null)).Returns(apartment);
        _ownerRepository!.Setup(c => c.Get(o => o.Id == newOwnerId, null)).Returns((Owner)null);
        _buildingRepository!.Setup(a => a.Get(b => b.Id == buildingId, null)).Returns(building);
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        _apartmentLogic!.Update(apartmentId, updatedApartment!);
    }

    [TestMethod]
    public void DeleteApartment()
    {
        int apartmentId = 1;
        int buildingId = 1;
        _apartmentRepository!.Setup(a => a.Get(b => b.Id == apartmentId, null)).Returns(apartment);
        _apartmentRepository!.Setup(o => o.Delete(apartment!));
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        _buildingRepository!.Setup(a => a.Get(b => b.Id == buildingId, null)).Returns(building);
        var deleted = _apartmentLogic!.Delete(apartmentId);

        _buildingRepository.VerifyAll();
        Assert.IsTrue(deleted);
    }

    [TestMethod]
    public void DeleteApartmentNotFound()
    {
        int apartmentId = 1;
        _apartmentRepository!.Setup(a => a.Get(b => b.Id == apartmentId, null)).Returns((Apartment)null);
        var deleted = _apartmentLogic!.Delete(apartmentId);

        Assert.IsFalse(deleted);
    }
}
