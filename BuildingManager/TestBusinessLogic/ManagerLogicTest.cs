using Domain;
using BusinessLogic;
using IBusinessLogic.Exceptions;
using IBusinessLogic;
using IDataAccess;
using Moq;
using DTOs.In;

namespace TestBusinessLogic;

[TestClass]
public class ManagerLogicTest
{
    private Mock<IGenericRepository<Manager>> _managerRepository;
    private Mock<IGenericRepository<Administrator>> _administratorRepository;
    private Mock<IGenericRepository<Staff>> _staffRepository;
    private Mock<IGenericRepository<CompanyAdmin>> _companyAdminRepository;
    private Mock<ISessionLogic> _sessionLogic;
    private ManagerLogic? _managerLogic;
    private Manager manager;

    [TestInitialize]
    public void Setup()
    {
        _managerRepository = new Mock<IGenericRepository<Manager>>(MockBehavior.Strict);
        _administratorRepository = new Mock<IGenericRepository<Administrator>>(MockBehavior.Strict);
        _staffRepository = new Mock<IGenericRepository<Staff>>(MockBehavior.Strict);
        _companyAdminRepository = new Mock<IGenericRepository<CompanyAdmin>>(MockBehavior.Strict);
        _sessionLogic = new Mock<ISessionLogic>(MockBehavior.Strict);

        var dto = new ManagerLogicDTO(
            _staffRepository.Object,
            _administratorRepository.Object,
            _managerRepository.Object,
            _companyAdminRepository.Object,
            _sessionLogic.Object);

        _managerLogic = new ManagerLogic(dto);
        manager = new Manager
        {
            Id = 1,
            Email = "manager@manager.com",
            Name = "Manager",
            LastName = "Manager",
            Password = "Test.1234"
        };
    }


    [TestMethod]
    public void CreateManager()
    {
        List<Manager> managers = new List<Manager>();
        List<Administrator> administrators = new List<Administrator>();
        List<Staff> staffs = new List<Staff>();
        List<CompanyAdmin> companyAdmins = new List<CompanyAdmin>();
        _managerRepository!.Setup(a => a.Insert(manager!));
        _managerRepository!.Setup(a => a.GetAll<Manager>()).Returns(managers);
        _administratorRepository!.Setup(m => m.GetAll<Administrator>()).Returns(administrators);
        _companyAdminRepository!.Setup(m => m.GetAll<CompanyAdmin>()).Returns(companyAdmins);
        _staffRepository!.Setup(s => s.GetAll<Staff>()).Returns(staffs);
        var newManager = _managerLogic!.Create(manager!);

        _managerRepository.VerifyAll();
        Assert.AreEqual(manager, newManager);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void CreateAlreadyExistingAdministrator()
    {
        List<Manager> managers = new List<Manager> { manager };
        List<Administrator> administrators = new List<Administrator>();
        List<Staff> staffs = new List<Staff>();
        _managerRepository!.Setup(o => o.GetAll<Manager>()).Returns(managers);
        _administratorRepository!.Setup(m => m.GetAll<Administrator>()).Returns(administrators);
        _staffRepository!.Setup(s => s.GetAll<Staff>()).Returns(staffs);

        var newManagerCopy = _managerLogic!.Create(manager!);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void CreateAlreadyExistingStaff()
    {
        Building building = new Building();

        Staff staff = new Staff()
        {
            Id = 1,
            Email = "manager@manager.com",
            Name = "Manager",
            LastName = "Manager",
            Password = "Test.1234",
        };

        List<Staff> staffs = new List<Staff> { staff };
        _staffRepository!.Setup(s => s.GetAll<Staff>()).Returns(staffs);

        var newManagerCopy = _managerLogic!.Create(manager!);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void CreateAlreadyExistingManager()
    {
        Manager manager = new Manager()
        {
            Id = 1,
            Email = "manager@manager.com",
            Name = "Manager",
            LastName = "Manager",
            Password = "Test.1234"
        };

        List<Staff> staffs = new List<Staff> { };
        List<Manager> managers = new List<Manager> { manager };
        _staffRepository!.Setup(s => s.GetAll<Staff>()).Returns(staffs);
        _managerRepository!.Setup(m => m.GetAll<Manager>()).Returns(managers);

        var newManagerCopy = _managerLogic!.Create(manager!);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void CreateAlreadyExistingCompanyAdmin()
    {
        CompanyAdmin companyAdmin = new CompanyAdmin()
        {
            Id = 1,
            Email = "manager@manager.com",
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

        var newManagerCopy = _managerLogic!.Create(manager!);
    }

    [TestMethod]
    public void GetAllManagers()
    {
        List<Manager> managers = new List<Manager> { manager };
        _managerRepository!.Setup(o => o.GetAll<Manager>()).Returns(managers);
        List<Manager> actualManagers = _managerLogic.GetAll();

        _managerRepository.VerifyAll();
        CollectionAssert.AreEqual(managers, actualManagers);
    }

    [TestMethod]
    public void GetManagerById()
    {
        int managerId = 1;
        List<Manager> managers = new List<Manager> { manager };
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        _managerRepository!.Setup(o => o.Get(o => o.Id == managerId, null)).Returns(manager!);
        Manager returnedManager = _managerLogic.GetById(managerId);

        _managerRepository.VerifyAll();
        Assert.AreEqual(manager, returnedManager);
    }

    [TestMethod]
    public void GetNonExistingManagerById()
    {
        int managerId = 1;
        _managerRepository!.Setup(o => o.Get(o => o.Id == managerId, null)).Returns((Manager)null);
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        Manager returnedManager = _managerLogic.GetById(managerId);

        _managerRepository.VerifyAll();
        Assert.IsNull(returnedManager);
    }

    [TestMethod]
    public void UpdateManagerInformation()
    {
        int managerId = 1;
        Manager managerCopy = new Manager
        {
            Id = 1,
            Email = "manager@manager.com",
            Name = "managerName",
            LastName = "managerLastname",
            Password = "Test.1234"
        };
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        _managerRepository.Setup(o => o.Get(o => o.Id == managerId, null)).Returns(manager);
        _managerRepository.Setup(o => o.Update(manager));

        Manager updatedManager = _managerLogic.Update(managerId, managerCopy);

        _managerRepository.VerifyAll();
        Assert.AreEqual(managerCopy, updatedManager);
    }

    [TestMethod]
    public void UpdateManagerEmailToDifferentOne()
    {
        int managerId = 1;
        Manager managerCopy = new Manager
        {
            Id = 1,
            Email = "manager@email.com",
            Name = "manager",
            LastName = "manager",
            Password = "Test.1234"
        };
        List<Manager> managers = new List<Manager> { };
        List<Administrator> administrators = new List<Administrator>();
        List<Staff> staffs = new List<Staff>();
        List<CompanyAdmin> companyAdmins = new List<CompanyAdmin> { };
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        _managerRepository.Setup(o => o.GetAll<Manager>()).Returns(managers);
        _administratorRepository!.Setup(m => m.GetAll<Administrator>()).Returns(administrators);
        _staffRepository!.Setup(s => s.GetAll<Staff>()).Returns(staffs);
        _companyAdminRepository!.Setup(s => s.GetAll<CompanyAdmin>()).Returns(companyAdmins);
        _managerRepository.Setup(o => o.Get(o => o.Id == managerId, null)).Returns(manager);
        _managerRepository.Setup(o => o.Update(manager));

        Manager updatedManager = _managerLogic.Update(managerId, managerCopy);

        _managerRepository.VerifyAll();
        Assert.AreEqual(managerCopy, updatedManager);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void UpdateManagerEmailToAlreadyExistingOne()
    {
        int managerId = 1;
        Manager managerCopy = new Manager
        {
            Id = 1,
            Email = "email@email.com",
            Name = "manager",
            LastName = "manager",
            Password = "Test.1234"
        };

        Manager sameEmailManager = new Manager
        {
            Id = 1,
            Email = "manager@manager.com",
            Name = "manager",
            LastName = "manager",
            Password = "Test.1234"
        };

        List<Manager> managers = new List<Manager> { manager, managerCopy };
        List<Administrator> administrators = new List<Administrator>();
        List<Staff> staffs = new List<Staff>();
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        _managerRepository.Setup(o => o.GetAll<Manager>()).Returns(managers);
        _administratorRepository!.Setup(m => m.GetAll<Administrator>()).Returns(administrators);
        _staffRepository!.Setup(s => s.GetAll<Staff>()).Returns(staffs);
        _managerRepository.Setup(o => o.Get(o => o.Id == managerId, null)).Returns(managerCopy);
        _managerRepository.Setup(o => o.Update(managerCopy));

        Manager updatedManager = _managerLogic.Update(managerId, sameEmailManager);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public void UpdateNonExistingManager()
    {
        int managerId = 1;
        Manager managerCopy = new Manager
        {
            Id = 1,
            Email = "email@email.com",
            Name = "manager",
            LastName = "manager",
            Password = "Test.1234"
        };
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        _managerRepository.Setup(o => o.Get(o => o.Id == managerId, null)).Returns((Manager)null);
        _managerRepository.Setup(o => o.Update(manager));

        Manager updatedManager = _managerLogic.Update(managerId, managerCopy);
    }

    [TestMethod]
    public void DeleteExistingManager()
    {
        int managerId = 1;
        _managerRepository.Setup(o => o.Get(o => o.Id == managerId, null)).Returns(manager);
        _managerRepository.Setup(o => o.Delete(manager));

        bool managerDeleted = _managerLogic.Delete(managerId);

        _managerRepository.VerifyAll();
        Assert.IsTrue(managerDeleted);
    }

    [TestMethod]
    public void DeleteNonExistingManager()
    {
        int managerId = 1;
        _managerRepository.Setup(o => o.Get(o => o.Id == managerId, null)).Returns((Manager)null);

        bool managerDeleted = _managerLogic.Delete(managerId);

        _managerRepository.VerifyAll();
        Assert.IsFalse(managerDeleted);
    }
}