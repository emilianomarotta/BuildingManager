using Domain;
using DTOs.In;
using DTOs.Out;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebAPI
{
    [ApiController]
    [Route("api/staffs")]
    [AuthenticationFilter("Manager")]
    public class StaffController : ControllerBase
    {
        private readonly IBusinessLogic<Staff> _staffLogic;

        public StaffController(IBusinessLogic<Staff> staffLogic)
        {
            _staffLogic = staffLogic;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var retrievedStaffs = _staffLogic.GetAll();
            return Ok(retrievedStaffs.Select(p => new StaffDetailModel(p)).ToList());
        }

        [HttpGet("{id}")]
        public IActionResult Show(int id)
        {
            var staff = _staffLogic.GetById(id);
            if (staff == null)
            {
                return NotFound((new { Message = "Staff not found" }));
            }
            return Ok(new StaffDetailModel(staff));
        }

        [HttpPost]
        public IActionResult Create([FromBody] StaffCreateModel newStaff)
        {
            var staff = _staffLogic.Create(newStaff.ToEntity());
            if (staff == null)
            {
                return BadRequest(new { Message = "Invalid data" });
            }
            return CreatedAtAction(null, new StaffDetailModel(staff));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] StaffCreateModel updatedStaff)
        {
            var staff = _staffLogic.Update(id, updatedStaff.ToEntity());
            if (staff == null)
            {
                return NotFound(new { Message = "Staff not found" });
            }
            return Ok(new StaffDetailModel(staff));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var staffDeleted = _staffLogic.Delete(id);
            if (!staffDeleted)
            {
                return NotFound(new { Message = "Staff not found" });
            }
            return NoContent();
        }
    }
}