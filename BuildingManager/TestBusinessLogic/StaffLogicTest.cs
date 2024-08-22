using Domain;
using BusinessLogic;
using IBusinessLogic.Exceptions;
using IDataAccess;
using Moq;
using IBusinessLogic;
using DTOs.In;

namespace TestBusinessLogic;

[TestClass]
public class StaffLogicTest
{
    private Mock<IGenericRepository<Staff>> _staffRepository;
    private Mock<IGenericRepository<Administrator>> _administratorRepository;
    private Mock<IGenericRepository<Manager>> _managerRepository;
    private Mock<IGenericRepository<CompanyAdmin>> _companyAdminRepository;
    private Mock<ISessionLogic> _sessionLogic;
    private StaffLogic? _staffLogic;
    private Staff staff;
    private Building building;
    private Manager manager;

    [TestInitialize]
    public void Setup()
    {
        _staffRepository = new Mock<IGenericRepository<Staff>>(MockBehavior.Strict);
        _administratorRepository = new Mock<IGenericRepository<Administrator>>(MockBehavior.Strict);
        _managerRepository = new Mock<IGenericRepository<Manager>>(MockBehavior.Strict);
        _companyAdminRepository = new Mock<IGenericRepository<CompanyAdmin>>(MockBehavior.Strict);
        _sessionLogic = new Mock<ISessionLogic>(MockBehavior.Strict);
        var dto = new StaffLogicDTO(
            _staffRepository.Object,
            _administratorRepository.Object,
            _managerRepository.Object,
            _companyAdminRepository.Object,
            _sessionLogic.Object);

        _staffLogic = new StaffLogic(dto);

        staff = new Staff
        {
            Id = 1,
            Email = "staff@staff.com",
            Name = "Staff",
            LastName = "Staff",
            Password = "Test.1234",
        };
        manager = new Manager
        {
            Id = 1
        };
    }

    [TestMethod]
    public void CreateStaff()
    {
        List<Staff> staffs = new List<Staff>();
        List<Administrator> administrators = new List<Administrator>();
        List<Manager> managers = new List<Manager>();
        List<CompanyAdmin> companyAdmins = new List<CompanyAdmin>();
        _staffRepository!.Setup(a => a.Insert(staff!));
        _staffRepository!.Setup(a => a.GetAll<Staff>()).Returns(staffs);
        _administratorRepository!.Setup(m => m.GetAll<Administrator>()).Returns(administrators);
        _managerRepository!.Setup(s => s.GetAll<Manager>()).Returns(managers);
        _companyAdminRepository!.Setup(s => s.GetAll<CompanyAdmin>()).Returns(companyAdmins);
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        var newStaff = _staffLogic!.Create(staff!);

        _staffRepository.VerifyAll();
        Assert.AreEqual(staff, newStaff);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void CreateAlreadyExistingAdministrator()
    {
        Administrator administrator = new Administrator
        {
            Id = 1,
            Email = "staff@staff.com",
            Name = "Administrator",
            LastName = "Administrator",
            Password = "Test.1234"
        };
        List<Staff> staffs = new List<Staff> { };
        List<Administrator> administrators = new List<Administrator> { administrator };
        List<Manager> managers = new List<Manager>();
        _staffRepository!.Setup(o => o.GetAll<Staff>()).Returns(staffs);
        _administratorRepository!.Setup(m => m.GetAll<Administrator>()).Returns(administrators);
        _managerRepository!.Setup(s => s.GetAll<Manager>()).Returns(managers);
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);

        var newStaffCopy = _staffLogic!.Create(staff!);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void CreateAlreadyExistingStaff()
    {
        Staff staff = new Staff()
        {
            Id = 1,
            Email = "staff@staff.com",
            Name = "Staff",
            LastName = "Staff",
            Password = "Test.1234",
        };

        List<Staff> staffs = new List<Staff> { staff };
        _staffRepository!.Setup(s => s.GetAll<Staff>()).Returns(staffs);
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);

        var newStaffCopy = _staffLogic!.Create(staff!);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void CreateAlreadyExistingManager()
    {
        Manager manager = new Manager()
        {
            Id = 1,
            Email = "staff@staff.com",
            Name = "Manager",
            LastName = "Manager",
            Password = "Test.1234"
        };
        List<Staff> staffs = new List<Staff> { };
        List<Manager> managers = new List<Manager> { manager };
        _staffRepository!.Setup(s => s.GetAll<Staff>()).Returns(staffs);
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        _managerRepository!.Setup(m => m.GetAll<Manager>()).Returns(managers);

        var newStaffCopy = _staffLogic!.Create(staff!);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void CreateAlreadyExistingCompanyAdmin()
    {
        CompanyAdmin companyAdmin = new CompanyAdmin()
        {
            Id = 1,
            Email = "staff@staff.com",
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
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        _staffRepository!.Setup(s => s.GetAll<Staff>()).Returns(staffs);

        var newStaffCopy = _staffLogic!.Create(staff!);
    }

    [TestMethod]
    public void GetAllStaffs()
    {

        List<Staff> staffs = new List<Staff> { staff };
        List<Building> buildings = new List<Building> { building };
        _staffRepository!.Setup(o => o.GetAll<Staff>()).Returns(staffs);
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        List<Staff> actualStaffs = _staffLogic.GetAll();

        _staffRepository.VerifyAll();
        CollectionAssert.AreEqual(staffs, actualStaffs);
    }

    [TestMethod]
    public void GetStaffById()
    {
        int staffId = 1;
        List<Staff> staffs = new List<Staff> { staff };
        _staffRepository!.Setup(o => o.Get(o => o.Id == staffId, null)).Returns(staff!);
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        Staff returnedStaff = _staffLogic.GetById(staffId);

        _staffRepository.VerifyAll();
        Assert.AreEqual(staff, returnedStaff);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public void GetNonExistingStaffById()
    {
        int staffId = 1;
        _staffRepository!.Setup(o => o.Get(o => o.Id == staffId, null)).Returns((Staff)null);
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        Staff returnedStaff = _staffLogic.GetById(staffId);

        _staffRepository.VerifyAll();
        Assert.IsNull(returnedStaff);
    }

    [TestMethod]
    public void UpdateStaffInformation()
    {
        int staffId = 1;
        Staff staffCopy = new Staff
        {
            Id = 1,
            Email = "staff@staff.com",
            Name = "staffName",
            LastName = "staffLastname",
            Password = "Test.1234",
        };

        _staffRepository.Setup(o => o.Get(o => o.Id == staffId, null)).Returns(staff);
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        _staffRepository.Setup(o => o.Update(staff));

        Staff updatedStaff = _staffLogic.Update(staffId, staffCopy);

        _staffRepository.VerifyAll();
        Assert.AreEqual(staffCopy, updatedStaff);
    }

    [TestMethod]
    public void UpdateStaffEmailToDifferentOne()
    {
        int staffId = 1;
        Staff staffCopy = new Staff
        {
            Id = 1,
            Email = "staff@email.com",
            Name = "staff",
            LastName = "staff",
            Password = "Test.1234",
        };
        List<Staff> staffs = new List<Staff> { };
        List<Administrator> administrators = new List<Administrator>();
        List<Manager> managers = new List<Manager>();
        List<CompanyAdmin> companyAdmins = new List<CompanyAdmin>();
        _staffRepository.Setup(o => o.GetAll<Staff>()).Returns(staffs);
        _administratorRepository!.Setup(m => m.GetAll<Administrator>()).Returns(administrators);
        _managerRepository!.Setup(s => s.GetAll<Manager>()).Returns(managers);
        _companyAdminRepository!.Setup(s => s.GetAll<CompanyAdmin>()).Returns(companyAdmins);
        _staffRepository.Setup(o => o.Get(o => o.Id == staffId, null)).Returns(staff);
        _staffRepository.Setup(o => o.Update(staff));
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        Staff updatedStaff = _staffLogic.Update(staffId, staffCopy);

        _staffRepository.VerifyAll();
        Assert.AreEqual(staffCopy, updatedStaff);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void UpdateStaffEmailToAlreadyExistingOne()
    {
        int staffId = 2;
        Staff staffCopy = new Staff
        {
            Id = 2,
            Email = "email@email.com",
            Name = "staff",
            LastName = "staff",
            Password = "Test.1234",
        };

        Staff sameEmailStaff = new Staff
        {
            Id = 2,
            Email = "staff@staff.com",
            Name = "staff",
            LastName = "staff",
            Password = "Test.1234",
        };
        List<Staff> staffs = new List<Staff> { staff, staffCopy };
        List<Administrator> administrators = new List<Administrator>();
        List<Manager> managers = new List<Manager>();
        _staffRepository.Setup(o => o.GetAll<Staff>()).Returns(staffs);
        _administratorRepository!.Setup(m => m.GetAll<Administrator>()).Returns(administrators);
        _managerRepository!.Setup(s => s.GetAll<Manager>()).Returns(managers);
        _staffRepository.Setup(o => o.Get(o => o.Id == staffId, null)).Returns(staffCopy);
        _staffRepository.Setup(o => o.Update(staffCopy));
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);

        Staff updatedStaff = _staffLogic.Update(staffId, sameEmailStaff);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public void UpdateNonExistingStaff()
    {
        int staffId = 1;
        Staff staffCopy = new Staff
        {
            Id = 1,
            Email = "email@email.com",
            Name = "staff",
            LastName = "staff",
            Password = "Test.1234",
        };

        _staffRepository.Setup(o => o.Get(o => o.Id == staffId, null)).Returns((Staff)null);
        _staffRepository.Setup(o => o.Update(staff));
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        Staff updatedStaff = _staffLogic.Update(staffId, staffCopy);
    }

    [TestMethod]
    public void DeleteExistingStaff()
    {
        int staffId = 1;
        _staffRepository.Setup(o => o.Get(o => o.Id == staffId, null)).Returns(staff);
        _staffRepository.Setup(o => o.Delete(staff));
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);

        bool staffDeleted = _staffLogic.Delete(staffId);

        _staffRepository.VerifyAll();
        Assert.IsTrue(staffDeleted);
    }

    [TestMethod]
    public void DeleteNonExistingStaff()
    {
        int staffId = 1;
        _staffRepository.Setup(o => o.Get(o => o.Id == staffId, null)).Returns((Staff)null);
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);

        bool staffDeleted = _staffLogic.Delete(staffId);

        _staffRepository.VerifyAll();
        Assert.IsFalse(staffDeleted);
    }
}