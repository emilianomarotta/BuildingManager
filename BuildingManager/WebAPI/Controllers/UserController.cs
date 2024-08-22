using BusinessLogic;
using Domain;
using DTOs.In;
using DTOs.Out;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebAPI;

[ApiController]
[Route("api/sessions")]
public class UserController : ControllerBase
{
    private readonly ISessionLogic _sessionLogic;
    private readonly UserLogic _userLogic;


    public UserController(ISessionLogic sessionLogic, UserLogic userLogic)
    {
        _sessionLogic = sessionLogic;
        _userLogic = userLogic;
    }

    [HttpPost]
    public IActionResult Create([FromBody] SessionCreateModel sessionCreateModel)
    {
        var user = _userLogic.GetByEmailAndPassword(sessionCreateModel.Email, sessionCreateModel.Password);
        if (user == null)
        {
            return BadRequest(new { Message = "Invalid email or password" });
        }
        Session session = new Session
        {
            User = user,
            UserId = user.Id,
        };
        var sessionOK = _sessionLogic.Create(session);
        return CreatedAtAction(null, new SessionDetailModel(sessionOK));
    }
    [HttpDelete("{id}")]
    [AuthenticationFilter("Staff|Manager|Administrator|CompanyAdmin")]
    public IActionResult Delete(int id)
    {
        var deleted = _sessionLogic.Delete(id);
        if (!deleted)
        {
            return NotFound(new { Message = "Session not found" });
        }
        return NoContent();
    }
}
