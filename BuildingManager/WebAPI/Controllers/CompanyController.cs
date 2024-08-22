using Domain;
using DTOs.In;
using DTOs.Out;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebAPI
{
    [ApiController]
    [Route("api/companies")]
    [AuthenticationFilter("CompanyAdmin")]
    public class CompanyController : ControllerBase
    {
        private readonly IBusinessLogic<Company> _companyLogic;

        public CompanyController(IBusinessLogic<Company> companyLogic)
        {
            _companyLogic = companyLogic;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var retrievedCompanies = _companyLogic.GetAll();
            return Ok(retrievedCompanies.Select(p => new CompanyDetailModel(p)).ToList());
        }

        [HttpGet("{id}")]
        public IActionResult Show(int id)
        {
            var company = _companyLogic.GetById(id);
            if (company == null)
            {
                return NotFound((new { Message = "Company not found" }));
            }
            return Ok(new CompanyDetailModel(company));
        }

        [HttpPost]
        public IActionResult Create([FromBody] CompanyCreateModel newcompany)
        {
            var company = _companyLogic.Create(newcompany.ToEntity());
            if (company == null)
            {
                return BadRequest(new { Message = "Invalid data" });
            }
            return CreatedAtAction(null, new CompanyDetailModel(company));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CompanyCreateModel companyCreateModel)
        {
            var company = _companyLogic.Update(id, companyCreateModel.ToEntity());
            if (company == null)
            {
                return NotFound(new { Message = "Company not found" });
            }
            return Ok(new CompanyDetailModel(company));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var companyDeleted = _companyLogic.Delete(id);
            if (!companyDeleted)
            {
                return NotFound(new { Message = "Company not found" });
            }
            return NoContent();
        }
    }
}