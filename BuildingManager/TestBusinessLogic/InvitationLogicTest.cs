using Domain;
using Domain.DataTypes;
using BusinessLogic;
using IBusinessLogic.Exceptions;
using IDataAccess;
using Moq;
using DTOs.In;

namespace TestBusinessLogic;

[TestClass]
public class InvitationLogicTest
{
    private Mock<IGenericRepository<Invitation>> _invitationRepository;
    private Mock<IGenericRepository<Administrator>> _administratorRepository;
    private Mock<IGenericRepository<Manager>> _managerRepository;
    private Mock<IGenericRepository<Staff>> _staffRepository;
    private Mock<IGenericRepository<CompanyAdmin>> _companyAdminRepository;
    private InvitationLogic? _invitationLogic;
    private Invitation invitation;

    [TestInitialize]
    public void Setup()
    {
        _invitationRepository = new Mock<IGenericRepository<Invitation>>(MockBehavior.Strict);
        _administratorRepository = new Mock<IGenericRepository<Administrator>>(MockBehavior.Strict);
        _managerRepository = new Mock<IGenericRepository<Manager>>(MockBehavior.Strict);
        _staffRepository = new Mock<IGenericRepository<Staff>>(MockBehavior.Strict);
        _companyAdminRepository = new Mock<IGenericRepository<CompanyAdmin>>(MockBehavior.Strict);

        var dto = new InvitationLogicDTO(
            _staffRepository.Object,
            _administratorRepository.Object,
            _managerRepository.Object,
            _companyAdminRepository.Object,
            _invitationRepository.Object);
        _invitationLogic = new InvitationLogic(dto);
        invitation = new Invitation
        {
            Id = 1,
            Name = "Test",
            Email = "test@test.com",
            Expiration = DateTime.Today.AddDays(2),
            Role = Role.Manager
        };
    }

    [TestMethod]
    public void CreateInvitation()
    {
        List<Invitation> invitations = new List<Invitation>();
        List<Administrator> administrators = new List<Administrator>();
        List<Manager> managers = new List<Manager>();
        List<Staff> staffs = new List<Staff>();
        List<CompanyAdmin> companyAdmins = new List<CompanyAdmin>();
        _invitationRepository.Setup(i => i.GetAll<Invitation>()).Returns(invitations);
        _administratorRepository!.Setup(a => a.GetAll<Administrator>()).Returns(administrators);
        _managerRepository!.Setup(m => m.GetAll<Manager>()).Returns(managers);
        _staffRepository!.Setup(s => s.GetAll<Staff>()).Returns(staffs);
        _companyAdminRepository!.Setup(s => s.GetAll<CompanyAdmin>()).Returns(companyAdmins);
        _invitationRepository.Setup(x => x.Insert(invitation));

        var newInvitation = _invitationLogic.Create(invitation);

        _invitationRepository.VerifyAll();
        Assert.AreEqual(invitation, newInvitation);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void CreateInvitationAlreadyExistManagerEmail()
    {
        Manager manager = new Manager
        {
            Id = 1,
            Email = "test@test.com",
        };
        List<Manager> managers = new List<Manager> { manager };
        List<Staff> staffs = new List<Staff>();
        _managerRepository!.Setup(m => m.GetAll<Manager>()).Returns(managers);
        _staffRepository!.Setup(s => s.GetAll<Staff>()).Returns(staffs);

        var newInvitation = _invitationLogic.Create(invitation);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void CreateInvitationAlreadyExistAdministratorEmail()
    {
        Administrator administrator = new Administrator
        {
            Id = 1,
            Email = "test@test.com",
        };
        List<Manager> managers = new List<Manager>();
        List<Staff> staffs = new List<Staff>();
        List<Administrator> administrators = new List<Administrator> { administrator };
        _administratorRepository!.Setup(a => a.GetAll<Administrator>()).Returns(administrators);
        _managerRepository!.Setup(m => m.GetAll<Manager>()).Returns(managers);
        _staffRepository!.Setup(s => s.GetAll<Staff>()).Returns(staffs);

        var newInvitation = _invitationLogic.Create(invitation);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void CreateInvitationAlreadyExistStaffEmail()
    {
        Staff staff = new Staff
        {
            Id = 1,
            Email = "test@test.com",
        };
        List<Staff> staffs = new List<Staff> { staff };
        _staffRepository!.Setup(s => s.GetAll<Staff>()).Returns(staffs);

        var newInvitation = _invitationLogic.Create(invitation);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void CreateInvitationAlreadyExistCompanyAdminEmail()
    {
        CompanyAdmin companyAdmin = new CompanyAdmin
        {
            Id = 1,
            Email = "test@test.com",
        };
        List<Manager> managers = new List<Manager>();
        List<Staff> staffs = new List<Staff>();
        List<Administrator> administrators = new List<Administrator>();
        List<CompanyAdmin> companyAdmins = new List<CompanyAdmin> { companyAdmin };
        _administratorRepository!.Setup(a => a.GetAll<Administrator>()).Returns(administrators);
        _companyAdminRepository!.Setup(s => s.GetAll<CompanyAdmin>()).Returns(companyAdmins);
        _managerRepository!.Setup(m => m.GetAll<Manager>()).Returns(managers);
        _staffRepository!.Setup(s => s.GetAll<Staff>()).Returns(staffs);

        var newInvitation = _invitationLogic.Create(invitation);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void InvitationPendingToEmail()
    {
        string invitationEmail = invitation.Email;
        List<Invitation> invitations = new List<Invitation> { invitation };
        List<Administrator> administrators = new List<Administrator>();
        List<Manager> managers = new List<Manager>();
        List<Staff> staffs = new List<Staff>();
        List<CompanyAdmin> companyAdmins = new List<CompanyAdmin>();
        _invitationRepository.Setup(i => i.GetAll<Invitation>()).Returns(invitations);
        _administratorRepository!.Setup(a => a.GetAll<Administrator>()).Returns(administrators);
        _managerRepository!.Setup(m => m.GetAll<Manager>()).Returns(managers);
        _staffRepository!.Setup(s => s.GetAll<Staff>()).Returns(staffs);
        _companyAdminRepository!.Setup(s => s.GetAll<CompanyAdmin>()).Returns(companyAdmins);

        var newInvitation = _invitationLogic.Create(invitation);
    }

    [TestMethod]
    public void GetAllInviations()
    {
        List<Invitation> invitations = new List<Invitation> { invitation };
        _invitationRepository.Setup(i => i.GetAll<Invitation>()).Returns(invitations);
        List<Invitation> actualInvitations = _invitationLogic.GetAll();

        _invitationRepository.VerifyAll();
        CollectionAssert.AreEqual(invitations, actualInvitations);
    }

    [TestMethod]
    public void GetInvitationById()
    {
        int invitationId = 1;
        List<Invitation> invitations = new List<Invitation> { invitation };
        _invitationRepository!.Setup(o => o.Get(o => o.Id == invitationId, null)).Returns(invitation!);
        Invitation returnedInvitation = _invitationLogic.GetById(invitationId);

        _invitationRepository.VerifyAll();
        Assert.AreEqual(invitation, returnedInvitation);
    }

    [TestMethod]
    public void GetNonExistintInvitationById()
    {
        int invitationId = 1;
        List<Invitation> invitations = new List<Invitation>();
        _invitationRepository!.Setup(o => o.Get(o => o.Id == invitationId, null)).Returns((Invitation)null!);
        Invitation returnedInvitation = _invitationLogic.GetById(invitationId);

        _invitationRepository.VerifyAll();
        Assert.IsNull(returnedInvitation);
    }

    [TestMethod]
    public void DeteleExistingInvitation()
    {
        int invitationId = 1;
        _invitationRepository.Setup(o => o.Get(o => o.Id == invitationId, null)).Returns(invitation);
        _invitationRepository.Setup(o => o.Delete(invitation));

        bool invitationDeletd = _invitationLogic.Delete(invitationId);

        _administratorRepository.VerifyAll();
        Assert.IsTrue(invitationDeletd);
    }

    [TestMethod]
    public void UpdateAcceptedInvitation()
    {
        int invitationId = 1;
        Invitation updatedInvitation = new Invitation
        {
            Id = 1,
            Name = "Test",
            Email = "test@test.com",

            Status = Status.Accepted
        };
        _invitationRepository.Setup(o => o.Get(o => o.Id == invitationId, null)).Returns(invitation);
        _invitationRepository.Setup(o => o.Update(invitation));

        Invitation expectedInvitation = _invitationLogic.Update(invitationId, updatedInvitation);

        _invitationRepository.VerifyAll();
        Assert.AreEqual(invitation.Status, expectedInvitation.Status);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public void UpdateNotPendingInvitation()
    {
        int invitationId = 1;

        invitation.Status = Status.Rejected;
        Invitation updatedInvitation = new Invitation
        {
            Id = 1,
            Name = "Test",
            Email = "test@test.com",
            Status = Status.Accepted
        };
        _invitationRepository.Setup(o => o.Get(o => o.Id == invitationId, null)).Returns(invitation);

        Invitation expectedInvitation = _invitationLogic.Update(invitationId, updatedInvitation);

        _invitationRepository.VerifyAll();
        Assert.AreEqual(invitation.Status, expectedInvitation.Status);
    }

    [TestMethod]
    [ExpectedException(typeof(InconsistentDataException))]
    public void UpdateInvitationEmailMismatch()
    {
        int invitationId = 1;
        invitation.Expiration = DateTime.Today.AddDays(2);
        Invitation updatedInvitation = new Invitation
        {
            Id = 1,
            Name = "Test",
            Email = "otherTest@test.com",
            Status = Status.Accepted
        };
        _invitationRepository.Setup(o => o.Get(o => o.Id == invitationId, null)).Returns(invitation);

        Invitation expectedInvitation = _invitationLogic.Update(invitationId, updatedInvitation);

        _invitationRepository.VerifyAll();
        Assert.AreEqual(invitation.Status, expectedInvitation.Status);
    }

    [TestMethod]
    public void DeteleNotExistingInvitation()
    {
        int invitationId = 1;
        _invitationRepository.Setup(o => o.Get(o => o.Id == invitationId, null)).Returns((Invitation)null);

        bool invitationDeletd = _invitationLogic.Delete(invitationId);

        _invitationRepository.VerifyAll();
        Assert.IsFalse(invitationDeletd);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void DeteleAcceptedInvitation()
    {
        int invitationId = 1;
        invitation.Status = Status.Accepted;
        _invitationRepository.Setup(o => o.Get(o => o.Id == invitationId, null)).Returns(invitation);

        bool invitationDeletd = _invitationLogic.Delete(invitationId);
    }
}