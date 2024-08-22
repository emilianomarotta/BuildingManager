using Domain;
using DTOs.In;
using DTOs.Out;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebAPI;

[ApiController]
[Route("api/apartments")]
[AuthenticationFilter("Manager")]
public class ApartmentController : ControllerBase
{
    private readonly IBusinessLogic<Apartment> _apartmentLogic;

    public ApartmentController(IBusinessLogic<Apartment> apartmentLogic)
    {
        _apartmentLogic = apartmentLogic;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var retrievedApartments = _apartmentLogic.GetAll();
        return Ok(retrievedApartments.Select(a => new ApartmentDetailModel(a)).ToList());
    }

    [HttpGet("{id}")]
    public IActionResult Show(int id)
    {
        var apartment = _apartmentLogic.GetById(id);
        if (apartment == null)
        {
            return NotFound((new { Message = "Apartment not found" }));
        }
        return Ok(new ApartmentDetailModel(apartment));
    }

    [HttpPost]
    public IActionResult Create([FromBody] ApartmentCreateModel newApartment)
    {
        var apartment = _apartmentLogic.Create(newApartment.ToEntity());
        if (apartment == null)
        {
            return BadRequest(new { Message = "Invalid data" });
        }
        return CreatedAtAction(null, new ApartmentDetailModel(apartment));
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] ApartmentPutModel updatedApartment)
    {
        var apartment = _apartmentLogic.Update(id, updatedApartment.ToEntity());
        if (apartment == null)
        {
            return NotFound(new { Message = "Apartment not found" });
        }
        return Ok(new ApartmentDetailModel(apartment));
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var apartmentDeleted = _apartmentLogic.Delete(id);
        if (apartmentDeleted == false)
        {
            return NotFound(new { Message = "Apartment not found" });
        }
        return NoContent();
    }
}