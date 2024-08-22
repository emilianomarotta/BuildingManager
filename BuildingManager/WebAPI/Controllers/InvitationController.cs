using Domain;
using Domain.DataTypes;
using DTOs.In;
using DTOs.Out;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebAPI;

[ApiController]
[Route("api/invitations")]
public class InvitationController : ControllerBase
{
    private readonly IBusinessLogic<Invitation> _invitationLogic;
    private readonly IBusinessLogic<Manager> _managerLogic;
    private readonly IBusinessLogic<CompanyAdmin> _companyAdminLogic;

    public InvitationController(IBusinessLogic<Invitation> invitationLogic, IBusinessLogic<Manager> managerLogic, IBusinessLogic<CompanyAdmin> companyAdminLogic)
    {
        _invitationLogic = invitationLogic;
        _managerLogic = managerLogic;
        _companyAdminLogic = companyAdminLogic;
    }

    [HttpGet]
    [AuthenticationFilter("Administrator")]
    public IActionResult Index()
    {
        var retrievedInvitations = _invitationLogic.GetAll();
        return Ok(retrievedInvitations.Select(i => new InvitationDetailModel(i)).ToList());
    }

    [HttpGet("{id}")]
    [AuthenticationFilter("Administrator")]
    public IActionResult Show(int id)
    {
        var invitation = _invitationLogic.GetById(id);
        if (invitation == null)
        {
            return NotFound((new { Message = "Invitation not found" }));
        }
        return Ok(new InvitationDetailModel(invitation));
    }

    [HttpPost]
    [AuthenticationFilter("Administrator")]
    public IActionResult Create([FromBody] InvitationCreateModel invitationCreateModel)
    {
        var invitation = _invitationLogic.Create(invitationCreateModel.ToEntity());
        if (invitation == null)
        {
            return BadRequest(new { Message = "Invalid data" });
        }
        return CreatedAtAction(null, new InvitationDetailModel(invitation));
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] InvitationPutModel invitationPutModel)
    {
        var invitation = _invitationLogic.Update(id, invitationPutModel.ToEntity());
        if (invitation == null)
        {
            return NotFound(new { Message = "Invitation not found" });
        }
        if (invitationPutModel.Status == Status.Accepted)
        {
            ProcessAcceptedInvitation(invitation, invitationPutModel.Password);
        }
        return Ok(new InvitationDetailModel(invitation));
    }

    [HttpDelete("{id}")]
    [AuthenticationFilter("Administrator")]
    public IActionResult Delete(int id)
    {
        var invitation = _invitationLogic.Delete(id);
        if (invitation == null)
        {
            return NotFound(new { Message = "Invitation not found" });
        }
        return NoContent();
    }

    private void ProcessAcceptedInvitation(Invitation invitation, string password)
    {
        if (invitation.Role == Role.CompanyAdmin)
        {
            CreateCompanyAdmin(invitation, password);
        }
        else
        {
            CreateManager(invitation, password);
        }
    }

    private void CreateCompanyAdmin(Invitation invitation, string password)
    {
        var companyAdminCreateModel = new CompanyAdminCreateModel
        {
            Name = invitation.Name,
            Email = invitation.Email,
            Password = password
        };
        _companyAdminLogic.Create(companyAdminCreateModel.ToEntity());
    }

    private void CreateManager(Invitation invitation, string password)
    {
        var managerCreateModel = new ManagerCreateModel
        {
            Name = invitation.Name,
            Email = invitation.Email,
            Password = password
        };
        _managerLogic.Create(managerCreateModel.ToEntity());
    }
}