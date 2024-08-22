using Domain;
using DTOs.In;
using DTOs.Out;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebAPI;

[ApiController]
[Route("api/managers")]
public class ManagerController : ControllerBase
{

    private readonly IBusinessLogic<Manager> _managerLogic;

    public ManagerController(IBusinessLogic<Manager> managerLogicObject)
    {
        _managerLogic = managerLogicObject;
    }

    [HttpGet]
    [AuthenticationFilter("Administrator|CompanyAdmin")]
    public IActionResult Index()
    {
        var retrievedManagers = _managerLogic.GetAll();
        return Ok(retrievedManagers.Select(m => new ManagerDetailModel(m)).ToList());
    }

    [HttpGet("{id}")]
    [AuthenticationFilter("Administrator|CompanyAdmin|Manager")]
    public IActionResult Show(int id)
    {
        var manager = _managerLogic.GetById(id);
        if (manager == null)
        {
            return NotFound((new { Message = "Manager not found" }));
        }
        return Ok(new ManagerDetailModel(manager));
    }

    [HttpPut("{id}")]
    [AuthenticationFilter("Manager")]
    public IActionResult Update(int id, [FromBody] ManagerPutModel managerPutModel)
    {
        var manager = _managerLogic.Update(id, managerPutModel.ToEntity());
        if (manager == null)
        {
            return NotFound(new { Message = "Manager not found" });
        }
        return Ok(new ManagerDetailModel(manager));
    }

    [HttpDelete("{id}")]
    [AuthenticationFilter("Administrator")]
    public IActionResult Delete(int id)
    {
        var managerDeleted = _managerLogic.Delete(id);
        if (managerDeleted == false)
        {
            return NotFound(new { Message = "Manager not found" });
        }
        return NoContent();
    }
}