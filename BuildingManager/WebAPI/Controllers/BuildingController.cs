using Domain;
using DTOs.In;
using DTOs.Out;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebAPI;

[ApiController]
[Route("api/buildings")]

public class BuildingController : ControllerBase
{
    private readonly IBusinessLogic<Building> _buildingLogic;

    public BuildingController(IBusinessLogic<Building> buildingLogic)
    {
        _buildingLogic = buildingLogic;
    }

    [HttpGet]
    [AuthenticationFilter("CompanyAdmin|Manager")]
    public IActionResult Index()
    {
        var retrievedBuildings = _buildingLogic.GetAll();
        return Ok(retrievedBuildings.Select(p => new BuildingDetailModel(p)).ToList());
    }

    [HttpGet("{id}")]
    [AuthenticationFilter("CompanyAdmin|Manager")]
    public IActionResult Show(int id)
    {
        var building = _buildingLogic.GetById(id);
        if (building == null)
        {
            return NotFound((new { Message = "Building not found" }));
        }
        return Ok(new BuildingDetailModel(building));
    }

    [HttpPost]
    [AuthenticationFilter("CompanyAdmin")]
    public IActionResult Create([FromBody] BuildingCreateModel newBuilding)
    {
        var building = _buildingLogic.Create(newBuilding.ToEntity());
        if (building == null)
        {
            return BadRequest(new { Message = "Invalid data" });
        }
        return Ok(new BuildingDetailModel(building));
    }

    [HttpPut("{id}")]
    [AuthenticationFilter("CompanyAdmin")]
    public IActionResult Update(int id, [FromBody] BuildingPutModel updatedBuilding)
    {
        var building = _buildingLogic.Update(id, updatedBuilding.ToEntity());
        if (building == null)
        {
            return NotFound(new { Message = "Building not found" });
        }
        return Ok(new BuildingDetailModel(building));
    }

    [HttpDelete("{id}")]
    [AuthenticationFilter("CompanyAdmin")]
    public IActionResult Delete(int id)
    {
        var building = _buildingLogic.Delete(id);
        if (building == null)
        {
            return NotFound(new { Message = "Building not found" });
        }
        return NoContent();
    }
    
}