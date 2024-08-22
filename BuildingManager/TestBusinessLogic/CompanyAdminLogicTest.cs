using Domain;
using BusinessLogic;
using IBusinessLogic.Exceptions;
using IDataAccess;
using Moq;
using IBusinessLogic;
using DTOs.In;

namespace TestBusinessLogic;

[TestClass]
public class CompanyAdminLogicTest
{
    private Mock<IGenericRepository<CompanyAdmin>> _companyAdminRepository;
    private Mock<IGenericRepository<Administrator>> _administratorRepository;
    private Mock<IGenericRepository<Manager>> _managerRepository;
    private Mock<IGenericRepository<Staff>> _staffRepository;
    private CompanyAdminLogic? _companyAdminLogic;
    private Mock<ISessionLogic> _sessionLogic;
    private CompanyAdmin companyAdmin;

    [TestInitialize]
    public void Setup()
    {
        _companyAdminRepository = new Mock<IGenericRepository<CompanyAdmin>>(MockBehavior.Strict);
        _administratorRepository = new Mock<IGenericRepository<Administrator>>(MockBehavior.Strict);
        _managerRepository = new Mock<IGenericRepository<Manager>>(MockBehavior.Strict);
        _staffRepository = new Mock<IGenericRepository<Staff>>(MockBehavior.Strict); ;
        _sessionLogic = new Mock<ISessionLogic>(MockBehavior.Strict);
        var dto = new CompanyAdminLogicDTO(
            _staffRepository.Object,
            _administratorRepository.Object,
            _managerRepository.Object,
            _companyAdminRepository.Object,
            _sessionLogic.Object);

        _companyAdminLogic = new CompanyAdminLogic(dto);

        companyAdmin = new CompanyAdmin
        {
            Id = 1,
            Email = "companyAdmin@companyAdmin.com",
            Name = "CompanyAdmin",
            LastName = "CompanyAdmin",
            Password = "Test.1234"
        };
    }


    [TestMethod]
    public void CreateCompanyAdmin()
    {
        List<CompanyAdmin> companyAdmins = new List<CompanyAdmin>();
        List<Administrator> administrators = new List<Administrator>();
        List<Manager> managers = new List<Manager>();
        List<Staff> staffs = new List<Staff>();
        _companyAdminRepository!.Setup(a => a.Insert(companyAdmin!));
        _companyAdminRepository!.Setup(a => a.GetAll<CompanyAdmin>()).Returns(companyAdmins);
        _managerRepository!.Setup(m => m.GetAll<Manager>()).Returns(managers);
        _administratorRepository!.Setup(m => m.GetAll<Administrator>()).Returns(administrators);
        _staffRepository!.Setup(s => s.GetAll<Staff>()).Returns(staffs);
        var newCompanyAdmin = _companyAdminLogic!.Create(companyAdmin!);

        _companyAdminRepository.VerifyAll();
        Assert.AreEqual(companyAdmin, newCompanyAdmin);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void CreateAlreadyExistingCompanyAdmin()
    {
        List<CompanyAdmin> companyAdmins = new List<CompanyAdmin> { companyAdmin };
        List<Administrator> administrators = new List<Administrator>();
        List<Manager> managers = new List<Manager>();
        List<Staff> staffs = new List<Staff>();
        _companyAdminRepository!.Setup(o => o.GetAll<CompanyAdmin>()).Returns(companyAdmins);
        _managerRepository!.Setup(m => m.GetAll<Manager>()).Returns(managers);
        _staffRepository!.Setup(s => s.GetAll<Staff>()).Returns(staffs);
        _administratorRepository!.Setup(s => s.GetAll<Administrator>()).Returns(administrators);

        var newCompanyAdminCopy = _companyAdminLogic!.Create(companyAdmin!);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void CreateAlreadyExistingStaff()
    {
        Building building = new Building();

        Staff staff = new Staff()
        {
            Id = 1,
            Email = "companyAdmin@companyAdmin.com",
            Name = "Staff",
            LastName = "Staff",
            Password = "Test.1234"
        };

        List<Staff> staffs = new List<Staff> { staff };
        _staffRepository!.Setup(s => s.GetAll<Staff>()).Returns(staffs);

        var newCompanyAdminCopy = _companyAdminLogic!.Create(companyAdmin!);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void CreateAlreadyExistingManager()
    {
        Manager manager = new Manager()
        {
            Id = 1,
            Email = "companyAdmin@companyAdmin.com",
            Name = "Manager",
            LastName = "Manager",
            Password = "Test.1234"
        };
        List<CompanyAdmin> companyAdmins = new List<CompanyAdmin> { };
        List<Administrator> administrators = new List<Administrator> { };
        List<Staff> staffs = new List<Staff> { };
        List<Manager> managers = new List<Manager> { manager };
        _staffRepository!.Setup(s => s.GetAll<Staff>()).Returns(staffs);
        _managerRepository!.Setup(m => m.GetAll<Manager>()).Returns(managers);
        var newCompanyAdminCopy = _companyAdminLogic!.Create(companyAdmin!);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void CreateAlreadyExistingAdministrator()
    {
        Administrator administrator = new Administrator()
        {
            Id = 1,
            Email = "companyAdmin@companyAdmin.com",
            Name = "Admin",
            LastName = "Admin",
            Password = "Test.1234"
        };
        List<CompanyAdmin> companyAdmins = new List<CompanyAdmin> { };
        List<Administrator> administrators = new List<Administrator> { administrator };
        List<Staff> staffs = new List<Staff> { };
        List<Manager> managers = new List<Manager> { };
        _staffRepository!.Setup(s => s.GetAll<Staff>()).Returns(staffs);
        _managerRepository!.Setup(m => m.GetAll<Manager>()).Returns(managers);
        _administratorRepository!.Setup(m => m.GetAll<Administrator>()).Returns(administrators);
        _companyAdminRepository!.Setup(o => o.GetAll<CompanyAdmin>()).Returns(companyAdmins);
        var newCompanyAdminCopy = _companyAdminLogic!.Create(companyAdmin!);
    }

    [TestMethod]
    public void GetAllCompanyAdmins()
    {
        List<CompanyAdmin> companyAdmins = new List<CompanyAdmin> { companyAdmin };
        _companyAdminRepository!.Setup(o => o.GetAll<CompanyAdmin>()).Returns(companyAdmins);
        List<CompanyAdmin> actualCompanyAdmins = _companyAdminLogic.GetAll();

        _companyAdminRepository.VerifyAll();
        CollectionAssert.AreEqual(companyAdmins, actualCompanyAdmins);
    }

    [TestMethod]
    public void GetCompanyAdminById()
    {
        int companyAdminId = 1;
        List<CompanyAdmin> companyAdmins = new List<CompanyAdmin> { companyAdmin };
        _companyAdminRepository!.Setup(o => o.Get(o => o.Id == companyAdminId, null)).Returns(companyAdmin!);
        CompanyAdmin returnedCompanyAdmin = _companyAdminLogic.GetById(companyAdminId);

        _companyAdminRepository.VerifyAll();
        Assert.AreEqual(companyAdmin, returnedCompanyAdmin);
    }

    [TestMethod]
    public void GetNonExistingCompanyAdminById()
    {
        int companyAdminId = 1;
        _companyAdminRepository!.Setup(o => o.Get(o => o.Id == companyAdminId, null)).Returns((CompanyAdmin)null);
        CompanyAdmin returnedCompanyAdmin = _companyAdminLogic.GetById(companyAdminId);
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)companyAdmin);

        _companyAdminRepository.VerifyAll();
        Assert.IsNull(returnedCompanyAdmin);
    }

    [TestMethod]
    public void UpdateCompanyAdminInformation()
    {
        int companyAdminId = 1;
        CompanyAdmin companyAdminCopy = new CompanyAdmin
        {
            Id = 1,
            Email = "companyAdmin@companyAdmin.com",
            Name = "companyAdminName",
            LastName = "companyAdminLastname",
            Password = "Test.1234"
        };

        _companyAdminRepository.Setup(o => o.Get(o => o.Id == companyAdminId, null)).Returns(companyAdmin);
        _companyAdminRepository.Setup(o => o.Update(companyAdmin));
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)companyAdmin);

        CompanyAdmin updatedCompanyAdmin = _companyAdminLogic.Update(companyAdminId, companyAdminCopy);

        _companyAdminRepository.VerifyAll();
        Assert.AreEqual(companyAdminCopy, updatedCompanyAdmin);
    }

    [TestMethod]
    public void UpdateCompanyAdminEmailToDifferentOne()
    {
        int companyAdminId = 1;
        CompanyAdmin companyAdminCopy = new CompanyAdmin
        {
            Id = 1,
            Email = "companyAdmin@email.com",
            Name = "companyAdmin",
            LastName = "companyAdmin",
            Password = "Test.1234"
        };
        List<CompanyAdmin> companyAdmins = new List<CompanyAdmin> { };
        List<Administrator> administrators = new List<Administrator> { };
        List<Manager> managers = new List<Manager>();
        List<Staff> staffs = new List<Staff>();
        _companyAdminRepository.Setup(o => o.GetAll<CompanyAdmin>()).Returns(companyAdmins);
        _managerRepository!.Setup(m => m.GetAll<Manager>()).Returns(managers);
        _staffRepository!.Setup(s => s.GetAll<Staff>()).Returns(staffs);
        _administratorRepository!.Setup(s => s.GetAll<Administrator>()).Returns(administrators);
        _companyAdminRepository.Setup(o => o.Get(o => o.Id == companyAdminId, null)).Returns(companyAdmin);
        _companyAdminRepository.Setup(o => o.Update(companyAdmin));
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)companyAdmin);
        CompanyAdmin updatedCompanyAdmin = _companyAdminLogic.Update(companyAdminId, companyAdminCopy);

        _companyAdminRepository.VerifyAll();
        Assert.AreEqual(companyAdminCopy, updatedCompanyAdmin);
    }

    [TestMethod]
    [ExpectedException(typeof(UnauthorizedException))]
    public void UpdateOtherCompanyAdmin()
    {
        int companyAdminId = 2;
        CompanyAdmin companyAdminCopy = new CompanyAdmin
        {
            Id = 2,
            Email = "companyAdmin@email.com",
            Name = "companyAdmin",
            LastName = "companyAdmin",
            Password = "Test.1234"
        };
        List<CompanyAdmin> companyAdmins = new List<CompanyAdmin> { };
        List<Administrator> administrators = new List<Administrator> { };
        List<Manager> managers = new List<Manager>();
        List<Staff> staffs = new List<Staff>();
        _companyAdminRepository.Setup(o => o.GetAll<CompanyAdmin>()).Returns(companyAdmins);
        _managerRepository!.Setup(m => m.GetAll<Manager>()).Returns(managers);
        _staffRepository!.Setup(s => s.GetAll<Staff>()).Returns(staffs);
        _administratorRepository!.Setup(s => s.GetAll<Administrator>()).Returns(administrators);
        _companyAdminRepository.Setup(o => o.Get(o => o.Id == companyAdminId, null)).Returns(companyAdmin);
        _companyAdminRepository.Setup(o => o.Update(companyAdmin));
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)companyAdmin);
        CompanyAdmin updatedCompanyAdmin = _companyAdminLogic.Update(companyAdminId, companyAdminCopy);

        _companyAdminRepository.VerifyAll();
        Assert.AreEqual(companyAdminCopy, updatedCompanyAdmin);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void UpdateCompanyAdminEmailToAlreadyExistingOne()
    {
        int companyAdminId = 1;
        CompanyAdmin companyAdminCopy = new CompanyAdmin
        {
            Id = 1,
            Email = "email@email.com",
            Name = "companyAdmin",
            LastName = "companyAdmin",
            Password = "Test.1234"
        };

        CompanyAdmin sameEmailCompanyAdmin = new CompanyAdmin
        {
            Id = 1,
            Email = "companyAdmin@companyAdmin.com",
            Name = "companyAdmin",
            LastName = "companyAdmin",
            Password = "Test.1234"
        };

        List<CompanyAdmin> companyAdmins = new List<CompanyAdmin> { companyAdmin, companyAdminCopy };
        List<Manager> managers = new List<Manager>();
        List<Staff> staffs = new List<Staff>();
        _companyAdminRepository.Setup(o => o.GetAll<CompanyAdmin>()).Returns(companyAdmins);
        _managerRepository!.Setup(m => m.GetAll<Manager>()).Returns(managers);
        _staffRepository!.Setup(s => s.GetAll<Staff>()).Returns(staffs);
        _companyAdminRepository.Setup(o => o.Get(o => o.Id == companyAdminId, null)).Returns(companyAdminCopy);
        _companyAdminRepository.Setup(o => o.Update(companyAdminCopy));
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)companyAdmin);

        CompanyAdmin updatedCompanyAdmin = _companyAdminLogic.Update(companyAdminId, sameEmailCompanyAdmin);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public void UpdateNonExistingCompanyAdmin()
    {
        int companyAdminId = 1;
        CompanyAdmin companyAdminCopy = new CompanyAdmin
        {
            Id = 1,
            Email = "email@email.com",
            Name = "companyAdmin",
            LastName = "companyAdmin",
            Password = "Test.1234"
        };

        _companyAdminRepository.Setup(o => o.Get(o => o.Id == companyAdminId, null)).Returns((CompanyAdmin)null);
        _companyAdminRepository.Setup(o => o.Update(companyAdmin));
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)companyAdmin);

        CompanyAdmin updatedCompanyAdmin = _companyAdminLogic.Update(companyAdminId, companyAdminCopy);
    }

    [TestMethod]
    public void DeleteExistingCompanyAdmin()
    {
        int companyAdminId = 1;
        _companyAdminRepository.Setup(o => o.Get(o => o.Id == companyAdminId, null)).Returns(companyAdmin);
        _companyAdminRepository.Setup(o => o.Delete(companyAdmin));

        bool companyAdminDeleted = _companyAdminLogic.Delete(companyAdminId);

        _companyAdminRepository.VerifyAll();
        Assert.IsTrue(companyAdminDeleted);
    }

    [TestMethod]
    public void DeleteNonExistingCompanyAdmin()
    {
        int companyAdminId = 1;
        _companyAdminRepository.Setup(o => o.Get(o => o.Id == companyAdminId, null)).Returns((CompanyAdmin)null);

        bool companyAdminDeleted = _companyAdminLogic.Delete(companyAdminId);

        _companyAdminRepository.VerifyAll();
        Assert.IsFalse(companyAdminDeleted);
    }
}