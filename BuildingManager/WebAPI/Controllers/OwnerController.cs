using Domain;
using DTOs.In;
using DTOs.Out;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebAPI;


[ApiController]
[Route("api/owners")]
[AuthenticationFilter("Manager")]
public class OwnerController : ControllerBase
{
    private readonly IBusinessLogic<Owner> _ownerLogic;
    
    public OwnerController(IBusinessLogic<Owner> ownerLogicObject)
    {
        _ownerLogic = ownerLogicObject;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        var retrievedOwners = _ownerLogic.GetAll();
        return Ok(retrievedOwners.Select(o => new OwnerDetailModel(o)).ToList());
    }
    
    [HttpGet("{id}")]
    public IActionResult Show(int id)
    {
        var owner = _ownerLogic.GetById(id);
        if (owner == null)
        {
            return NotFound((new { Message = "Owner not found" }));
        }
        return Ok(new OwnerDetailModel(owner));
    }
    
    [HttpPost]
    public IActionResult Create([FromBody] OwnerCreateModel ownerCreateModel)
    {
        var owner = _ownerLogic.Create(ownerCreateModel.ToEntity());
        if (owner == null)
        {
            return BadRequest(new { Message = "Invalid data" });
        }
        return CreatedAtAction(null, new OwnerDetailModel(owner));
    }
    
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] OwnerCreateModel ownerCreateModel)
    {
        var owner = _ownerLogic.Update(id, ownerCreateModel.ToEntity());
        if (owner == null)
        {
            return NotFound(new { Message = "Owner not found" });
        }
        return Ok(new OwnerDetailModel(owner));
    }
    
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var owner = _ownerLogic.Delete(id);
        if (owner == null)
        {
            return NotFound(new { Message = "Owner not found" });
        }
        return NoContent();
    }
}