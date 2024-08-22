using Domain;
using DTOs.In;
using DTOs.Out;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebAPI
{
    [ApiController]
    [Route("api/administrators")]
    [AuthenticationFilter("Administrator")]
    public class AdministratorController : ControllerBase
    {
        private readonly IBusinessLogic<Administrator> _administratorLogic;

        public AdministratorController(IBusinessLogic<Administrator> administratorLogic)
        {
            _administratorLogic = administratorLogic;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var retrievedAdministrators = _administratorLogic.GetAll();
            return Ok(retrievedAdministrators.Select(p => new AdministratorDetailModel(p)).ToList());
        }

        [HttpGet("{id}")]
        public IActionResult Show(int id)
        {
            var administrator = _administratorLogic.GetById(id);
            if (administrator == null)
            {
                return NotFound((new { Message = "Administrator not found" }));
            }
            return Ok(new AdministratorDetailModel(administrator));
        }

        [HttpPost]
        public IActionResult Create([FromBody] AdministratorCreateModel newAdministrator)
        {
            var administrator = _administratorLogic.Create(newAdministrator.ToEntity());
            if (administrator == null)
            {
                return BadRequest(new { Message = "Invalid data" });
            }
            return CreatedAtAction(null, new AdministratorDetailModel(administrator));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] AdministratorCreateModel updatedAdministrator)
        {
            var administrator = _administratorLogic.Update(id, updatedAdministrator.ToEntity());
            if (administrator == null)
            {
                return NotFound(new { Message = "Administrator not found" });
            }
            return Ok(new AdministratorDetailModel(administrator));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var administratorDeleted = _administratorLogic.Delete(id);
            if (!administratorDeleted)
            {
                return NotFound(new { Message = "Administrator not found" });
            }
            return NoContent();
        }
    }
}