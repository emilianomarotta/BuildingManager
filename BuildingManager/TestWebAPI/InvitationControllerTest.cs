using Domain;
using Domain.DataTypes;
using IBusinessLogic;
using Moq;
using DTOs.Out;
using DTOs.In;
using Microsoft.AspNetCore.Mvc;
using WebAPI;

namespace TestWebAPI;

[TestClass]
public class InvitationControllerTest
{
    private Mock<IBusinessLogic<Invitation>> _invitationLogic;
    private Mock<IBusinessLogic<Manager>> _managerLogic;
    private Mock<IBusinessLogic<CompanyAdmin>> _companyAdminLogic;
    private InvitationController _invitationController;
    private Invitation invitation;

    [TestInitialize]
    public void Setup()
    {
        _invitationLogic = new Mock<IBusinessLogic<Invitation>>(MockBehavior.Strict);
        _managerLogic = new Mock<IBusinessLogic<Manager>>(MockBehavior.Strict);
        _companyAdminLogic = new Mock<IBusinessLogic<CompanyAdmin>>(MockBehavior.Strict);
        _invitationController = new InvitationController(_invitationLogic.Object, _managerLogic.Object, _companyAdminLogic.Object);
        invitation = new Invitation
        {
            Id = 1,
            Name = "Invitation",
            Email = "invitation@invitation.com",
            Expiration = DateTime.Today.AddDays(1)
        };
    }
    
    [TestMethod]
    public void GetAllInvitations()
    {
        List<Invitation> invitations = new List<Invitation> { invitation };
        _invitationLogic.Setup(x => x.GetAll()).Returns(invitations);

        var expectedContent = invitations.Select(i => new InvitationDetailModel(i)).ToList();

        var result = _invitationController.Index();
        var okResult = result as OkObjectResult;
        var actualContent = okResult.Value as List<InvitationDetailModel>;

        _invitationLogic.VerifyAll();
        CollectionAssert.AreEqual(expectedContent, actualContent);
    }
    
    [TestMethod]
    public void GetOkTest()
    {
        List<Invitation> invitations = new List<Invitation> { invitation };
        _invitationLogic.Setup(o => o.GetById(It.IsAny<int>())).Returns(invitations.First());
        var expectedInvitationModel = new InvitationDetailModel(invitations.First());

        var result = _invitationController.Show(invitations.First().Id);
        var okResult = result as OkObjectResult;
        var actualInvitationModel = okResult.Value as InvitationDetailModel;

        _invitationLogic.VerifyAll();
        Assert.AreEqual(expectedInvitationModel, actualInvitationModel);
    }
    
    [TestMethod]
    public void CreateInvitation()
    {
        InvitationCreateModel invitationCreateModel = new InvitationCreateModel
        {
            Name = invitation.Name,
            Email = invitation.Email,
            Expiration = invitation.Expiration
        };
        _invitationLogic.Setup(x => x.Create(It.IsAny<Invitation>())).Returns(invitation);

        var expectedInvitationModel = new InvitationDetailModel(invitation);

        var result = _invitationController.Create(invitationCreateModel);
        var okResult = result as CreatedAtActionResult;
        var actualInvitationModel = okResult.Value as InvitationDetailModel;

        _invitationLogic.VerifyAll();
        Assert.AreEqual(expectedInvitationModel, actualInvitationModel);
    }
    
    [TestMethod]
    public void UpdateInvitation()
    {
        InvitationPutModel invitationPutModel = new InvitationPutModel
        {
            Email = "invitation@invitation.com",
            Password = "Test.1234",
            Status = Status.Accepted
        };
        _invitationLogic.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<Invitation>())).Returns(invitation);
        _managerLogic.Setup(x => x.Create(It.IsAny<Manager>())).Returns(new Manager());

        var expectedInvitationModel = new InvitationDetailModel(invitation);

        var result = _invitationController.Update(invitation.Id, invitationPutModel);
        var okResult = result as OkObjectResult;
        var actualInvitationModel = okResult.Value as InvitationDetailModel;

        _invitationLogic.VerifyAll();
        Assert.AreEqual(expectedInvitationModel, actualInvitationModel);
    }

    [TestMethod]
    public void DeleteInvitation()
    {
        _invitationLogic.Setup(x => x.Delete(It.IsAny<int>())).Returns(true);

        var expectedInvitationModel = new InvitationDetailModel(invitation);

        var result = _invitationController.Delete(invitation.Id);
        var okResult = result as NoContentResult;
        
        _invitationLogic.VerifyAll();
        Assert.AreEqual(204, okResult.StatusCode);
    }
    
}