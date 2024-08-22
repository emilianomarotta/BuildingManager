using Domain;
using BusinessLogic;
using IBusinessLogic.Exceptions;
using IDataAccess;
using Moq;
using IBusinessLogic;
using DTOs.In;

namespace TestBusinessLogic;

[TestClass]
public class AdministratorLogicTest
{
    private Mock<IGenericRepository<Administrator>> _administratorRepository;
    private Mock<IGenericRepository<Manager>> _managerRepository;
    private Mock<IGenericRepository<Staff>> _staffRepository;
    private Mock<IGenericRepository<CompanyAdmin>> _companyAdminRepository;
    private AdministratorLogic? _administratorLogic;
    private Mock<ISessionLogic> _sessionLogic;
    private Administrator administrator;

    [TestInitialize]
    public void Setup()
    {
        _administratorRepository = new Mock<IGenericRepository<Administrator>>(MockBehavior.Strict);
        _managerRepository = new Mock<IGenericRepository<Manager>>(MockBehavior.Strict);
        _staffRepository = new Mock<IGenericRepository<Staff>>(MockBehavior.Strict);
        _companyAdminRepository = new Mock<IGenericRepository<CompanyAdmin>>(MockBehavior.Strict);
        _sessionLogic = new Mock<ISessionLogic>(MockBehavior.Strict);
        var dto = new AdministratorLogicDTO(
            _staffRepository.Object,
            _administratorRepository.Object,
            _managerRepository.Object,
            _companyAdminRepository.Object,
            _sessionLogic.Object);

        _administratorLogic = new AdministratorLogic(dto);
        administrator = new Administrator
        {
            Id = 1,
            Email = "administrator@administrator.com",
            Name = "Administrator",
            LastName = "Administrator",
            Password = "Test.1234"
        };
    }


    [TestMethod]
    public void CreateAdministrator()
    {
        List<Administrator> administrators = new List<Administrator>();
        List<CompanyAdmin> companyAdmins = new List<CompanyAdmin>();
        List<Manager> managers = new List<Manager>();
        List<Staff> staffs = new List<Staff>();
        _administratorRepository!.Setup(a => a.Insert(administrator!));
        _administratorRepository!.Setup(a => a.GetAll<Administrator>()).Returns(administrators);
        _managerRepository!.Setup(m => m.GetAll<Manager>()).Returns(managers);
        _companyAdminRepository!.Setup(m => m.GetAll<CompanyAdmin>()).Returns(companyAdmins);
        _staffRepository!.Setup(s => s.GetAll<Staff>()).Returns(staffs);
        var newAdministrator = _administratorLogic!.Create(administrator!);

        _administratorRepository.VerifyAll();
        Assert.AreEqual(administrator, newAdministrator);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void CreateAlreadyExistingAdministrator()
    {
        List<Administrator> administrators = new List<Administrator> { administrator };
        List<Manager> managers = new List<Manager>();
        List<Staff> staffs = new List<Staff>();
        _administratorRepository!.Setup(o => o.GetAll<Administrator>()).Returns(administrators);
        _managerRepository!.Setup(m => m.GetAll<Manager>()).Returns(managers);
        _staffRepository!.Setup(s => s.GetAll<Staff>()).Returns(staffs);

        var newAdministratorCopy = _administratorLogic!.Create(administrator!);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void CreateAlreadyExistingStaff()
    {
        Building building = new Building();

        Staff staff = new Staff()
        {
            Id = 1,
            Email = "administrator@administrator.com",
            Name = "Administrator",
            LastName = "Administrator",
            Password = "Test.1234"
        };

        List<Staff> staffs = new List<Staff> { staff };
        _staffRepository!.Setup(s => s.GetAll<Staff>()).Returns(staffs);

        var newAdministratorCopy = _administratorLogic!.Create(administrator!);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void CreateAlreadyExistingManager()
    {
        Manager manager = new Manager()
        {
            Id = 1,
            Email = "administrator@administrator.com",
            Name = "Administrator",
            LastName = "Administrator",
            Password = "Test.1234"
        };

        List<Staff> staffs = new List<Staff> { };
        List<Manager> managers = new List<Manager> { manager };
        _staffRepository!.Setup(s => s.GetAll<Staff>()).Returns(staffs);
        _managerRepository!.Setup(m => m.GetAll<Manager>()).Returns(managers);

        var newAdministratorCopy = _administratorLogic!.Create(administrator!);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void CreateAlreadyExistingCompanyAdmin()
    {
        CompanyAdmin companyAdmin = new CompanyAdmin()
        {
            Id = 1,
            Email = "administrator@administrator.com",
            Name = "Administrator",
            LastName = "Administrator",
            Password = "Test.1234"
        };
        List<Administrator> administrators = new List<Administrator> { };
        List<Manager> managers = new List<Manager>();
        List<CompanyAdmin> companyAdmins = new List<CompanyAdmin> { companyAdmin };
        List<Staff> staffs = new List<Staff>();
        _administratorRepository!.Setup(o => o.GetAll<Administrator>()).Returns(administrators);
        _companyAdminRepository!.Setup(m => m.GetAll<CompanyAdmin>()).Returns(companyAdmins);
        _managerRepository!.Setup(m => m.GetAll<Manager>()).Returns(managers);
        _staffRepository!.Setup(s => s.GetAll<Staff>()).Returns(staffs);

        var newAdministratorCopy = _administratorLogic!.Create(administrator!);
    }

    [TestMethod]
    public void GetAllAdministrators()
    {
        List<Administrator> administrators = new List<Administrator> { administrator };
        _administratorRepository!.Setup(o => o.GetAll<Administrator>()).Returns(administrators);
        List<Administrator> actualAdministrators = _administratorLogic.GetAll();

        _administratorRepository.VerifyAll();
        CollectionAssert.AreEqual(administrators, actualAdministrators);
    }

    [TestMethod]
    public void GetAdministratorById()
    {
        int administratorId = 1;
        List<Administrator> administrators = new List<Administrator> { administrator };
        _administratorRepository!.Setup(o => o.Get(o => o.Id == administratorId, null)).Returns(administrator!);
        Administrator returnedAdministrator = _administratorLogic.GetById(administratorId);

        _administratorRepository.VerifyAll();
        Assert.AreEqual(administrator, returnedAdministrator);
    }

    [TestMethod]
    public void GetNonExistingAdministratorById()
    {
        int administratorId = 1;
        _administratorRepository!.Setup(o => o.Get(o => o.Id == administratorId, null)).Returns((Administrator)null);
        Administrator returnedAdministrator = _administratorLogic.GetById(administratorId);
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)administrator);

        _administratorRepository.VerifyAll();
        Assert.IsNull(returnedAdministrator);
    }

    [TestMethod]
    public void UpdateAdministratorInformation()
    {
        int administratorId = 1;
        Administrator administratorCopy = new Administrator
        {
            Id = 1,
            Email = "administrator@administrator.com",
            Name = "administratorName",
            LastName = "administratorLastname",
            Password = "Test.1234"
        };

        _administratorRepository.Setup(o => o.Get(o => o.Id == administratorId, null)).Returns(administrator);
        _administratorRepository.Setup(o => o.Update(administrator));
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)administrator);

        Administrator updatedAdministrator = _administratorLogic.Update(administratorId, administratorCopy);

        _administratorRepository.VerifyAll();
        Assert.AreEqual(administratorCopy, updatedAdministrator);
    }

    [TestMethod]
    public void UpdateAdministratorEmailToDifferentOne()
    {
        int administratorId = 1;
        Administrator administratorCopy = new Administrator
        {
            Id = 1,
            Email = "administrator@email.com",
            Name = "administrator",
            LastName = "administrator",
            Password = "Test.1234"
        };
        List<Administrator> administrators = new List<Administrator> { };
        List<Manager> managers = new List<Manager>();
        List<Staff> staffs = new List<Staff>();
        List<CompanyAdmin> companyAdmins = new List<CompanyAdmin> { };
        _administratorRepository.Setup(o => o.GetAll<Administrator>()).Returns(administrators);
        _managerRepository!.Setup(m => m.GetAll<Manager>()).Returns(managers);
        _companyAdminRepository!.Setup(m => m.GetAll<CompanyAdmin>()).Returns(companyAdmins);
        _staffRepository!.Setup(s => s.GetAll<Staff>()).Returns(staffs);
        _administratorRepository.Setup(o => o.Get(o => o.Id == administratorId, null)).Returns(administrator);
        _administratorRepository.Setup(o => o.Update(administrator));
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)administrator);
        Administrator updatedAdministrator = _administratorLogic.Update(administratorId, administratorCopy);

        _administratorRepository.VerifyAll();
        Assert.AreEqual(administratorCopy, updatedAdministrator);
    }

    [TestMethod]
    [ExpectedException(typeof(UnauthorizedException))]
    public void UpdateOtherAdministrator()
    {
        int administratorId = 2;
        Administrator administratorCopy = new Administrator
        {
            Id = 2,
            Email = "administrator@email.com",
            Name = "administrator",
            LastName = "administrator",
            Password = "Test.1234"
        };
        List<Administrator> administrators = new List<Administrator> { };
        List<Manager> managers = new List<Manager>();
        List<Staff> staffs = new List<Staff>();
        _administratorRepository.Setup(o => o.GetAll<Administrator>()).Returns(administrators);
        _managerRepository!.Setup(m => m.GetAll<Manager>()).Returns(managers);
        _staffRepository!.Setup(s => s.GetAll<Staff>()).Returns(staffs);
        _administratorRepository.Setup(o => o.Get(o => o.Id == administratorId, null)).Returns(administrator);
        _administratorRepository.Setup(o => o.Update(administrator));
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)administrator);
        Administrator updatedAdministrator = _administratorLogic.Update(administratorId, administratorCopy);

        _administratorRepository.VerifyAll();
        Assert.AreEqual(administratorCopy, updatedAdministrator);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void UpdateAdministratorEmailToAlreadyExistingOne()
    {
        int administratorId = 1;
        Administrator administratorCopy = new Administrator
        {
            Id = 1,
            Email = "email@email.com",
            Name = "administrator",
            LastName = "administrator",
            Password = "Test.1234"
        };

        Administrator sameEmailAdministrator = new Administrator
        {
            Id = 1,
            Email = "administrator@administrator.com",
            Name = "administrator",
            LastName = "administrator",
            Password = "Test.1234"
        };

        List<Administrator> administrators = new List<Administrator> { administrator, administratorCopy };
        List<Manager> managers = new List<Manager>();
        List<Staff> staffs = new List<Staff>();
        _administratorRepository.Setup(o => o.GetAll<Administrator>()).Returns(administrators);
        _managerRepository!.Setup(m => m.GetAll<Manager>()).Returns(managers);
        _staffRepository!.Setup(s => s.GetAll<Staff>()).Returns(staffs);
        _administratorRepository.Setup(o => o.Get(o => o.Id == administratorId, null)).Returns(administratorCopy);
        _administratorRepository.Setup(o => o.Update(administratorCopy));
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)administrator);

        Administrator updatedAdministrator = _administratorLogic.Update(administratorId, sameEmailAdministrator);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public void UpdateNonExistingAdministrator()
    {
        int administratorId = 1;
        Administrator administratorCopy = new Administrator
        {
            Id = 1,
            Email = "email@email.com",
            Name = "administrator",
            LastName = "administrator",
            Password = "Test.1234"
        };

        _administratorRepository.Setup(o => o.Get(o => o.Id == administratorId, null)).Returns((Administrator)null);
        _administratorRepository.Setup(o => o.Update(administrator));
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)administrator);

        Administrator updatedAdministrator = _administratorLogic.Update(administratorId, administratorCopy);
    }

    [TestMethod]
    public void DeleteExistingAdministrator()
    {
        int administratorId = 1;
        _administratorRepository.Setup(o => o.Get(o => o.Id == administratorId, null)).Returns(administrator);
        _administratorRepository.Setup(o => o.Delete(administrator));

        bool administratorDeleted = _administratorLogic.Delete(administratorId);

        _administratorRepository.VerifyAll();
        Assert.IsTrue(administratorDeleted);
    }

    [TestMethod]
    public void DeleteNonExistingAdministrator()
    {
        int administratorId = 1;
        _administratorRepository.Setup(o => o.Get(o => o.Id == administratorId, null)).Returns((Administrator)null);

        bool administratorDeleted = _administratorLogic.Delete(administratorId);

        _administratorRepository.VerifyAll();
        Assert.IsFalse(administratorDeleted);
    }
}