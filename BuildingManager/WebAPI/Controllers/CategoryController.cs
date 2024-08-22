using Domain;
using DTOs.In;
using DTOs.Out;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebAPI;

[ApiController]
[Route("api/categories")]
public class CategoryController : ControllerBase
{

    private readonly IBusinessLogic<Category> _categoryLogic;
    
    public CategoryController(IBusinessLogic<Category> categoryLogicObject)
    {
        _categoryLogic = categoryLogicObject;
    }

    [AuthenticationFilter("Administrator | Manager")]
    [HttpGet]
    public IActionResult Index()
    {
        var retrievedCategories = _categoryLogic.GetAll();
        return Ok(retrievedCategories.Select(c => new CategoryDetailModel(c)).ToList());
    }

    [AuthenticationFilter("Administrator | Manager")]
    [HttpGet("{id}")]
    public IActionResult Show(int id)
    {
        var category = _categoryLogic.GetById(id);
        if (category == null)
        {
            return NotFound((new { Message = "Category not found" }));
        }
        return Ok(new CategoryDetailModel(category));
    }

    [AuthenticationFilter("Administrator")]
    [HttpPost]
    public IActionResult Create([FromBody] CategoryCreateModel categoryCreateModel)
    {
        var category = _categoryLogic.Create(categoryCreateModel.ToEntity());
        if (category == null)
        {
            return BadRequest(new { Message = "Invalid data" });
        }
        return CreatedAtAction(null, new CategoryDetailModel(category));
    }

    [AuthenticationFilter("Administrator")]
    [HttpPut("{id}")] 
    public IActionResult Update(int id, [FromBody] CategoryCreateModel categoryCreateModel)
    {
        var category = _categoryLogic.Update(id, categoryCreateModel.ToEntity());
        if (category == null)
        {
            return NotFound(new { Message = "Category not found" });
        }
        return Ok(new CategoryDetailModel(category));
    }

    [AuthenticationFilter("Administrator")]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var deleteResult = _categoryLogic.Delete(id);
        if (deleteResult == false)
        {
            return NotFound(new { Message = "Category not found" });
        }
        return NoContent();
    }
}   