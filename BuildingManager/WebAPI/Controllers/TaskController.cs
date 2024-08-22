using DTOs.In;
using DTOs.Out;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;
using Task = Domain.Task;

namespace WebAPI
{
    [ApiController]
    [Route("api/tasks")]
    public class TaskController : ControllerBase
    {
        private readonly IBusinessLogic<Task> _taskLogic;

        public TaskController(IBusinessLogic<Task> taskLogic)
        {
            _taskLogic = taskLogic;
        }

        [HttpGet]
        [AuthenticationFilter("Manager|Staff")]
        public IActionResult Index([FromQuery] int? categoryId = null)
        {
            var retrievedTasks = categoryId.HasValue ?
                _taskLogic.GetAll().Where(task => task.CategoryId == categoryId.Value) :
                _taskLogic.GetAll();

            return Ok(retrievedTasks.Select(p => new TaskDetailModel(p)).ToList());
        }

        [HttpGet("{id}")]
        [AuthenticationFilter("Manager|Staff")]
        public IActionResult Show(int id)
        {
            var task = _taskLogic.GetById(id);
            if (task == null)
            {
                return NotFound(new { Message = "Task not found" });
            }
            return Ok(new TaskDetailModel(task));
        }

        [HttpPost]
        [AuthenticationFilter("Manager")]
        public IActionResult Create([FromBody] TaskCreateModel newTask)
        {
            var task = _taskLogic.Create(newTask.ToEntity());
            if (task == null)
            {
                return BadRequest(new { Message = "Invalid data" });
            }
            return CreatedAtAction(null, new TaskDetailModel(task));
        }

        [HttpPut("{id}/start")]
        [AuthenticationFilter("Manager")]
        public IActionResult Start(int id, [FromBody] TaskStartModel taskStartModel)
        {
            var task = _taskLogic.GetById(id);
            if (task == null)
            {
                return NotFound(new { Message = "Task not found" });
            }
            if (task.StaffId != null)
            {
                return BadRequest(new { Message = "Task already started" });
            }
            task.StartDate = DateTime.Now;
            task.StaffId = taskStartModel.StaffId;
            task = _taskLogic.Update(id, task);
            return Ok(new TaskDetailModel(task));
        }

        [HttpPut("{id}/finish")]
        [AuthenticationFilter("Staff")]
        public IActionResult Finish(int id, [FromBody] TaskEndModel taskEndModel)
        {
            var task = _taskLogic.GetById(id);
            if (task == null)
            {
                return NotFound(new { Message = "Task not found" });
            }
            if (task.StartDate == null)
            {
                return BadRequest(new { Message = "Task not started" });
            }
            if (task.EndDate != null)
            {
                return BadRequest(new { Message = "Task already completed" });
            }
            if (taskEndModel.Cost < 0)
            {
                return BadRequest(new { Message = "Task cost must be equal or greater than 0" });
            }
            task.EndDate = DateTime.Now;
            task.Cost = taskEndModel.Cost;
            task = _taskLogic.Update(id, taskEndModel.ToEntity());
            return Ok(new TaskDetailModel(task));
        }

        [HttpDelete("{id}")]
        [AuthenticationFilter("Manager")]
        public IActionResult Delete(int id)
        {
            var taskDeleted = _taskLogic.Delete(id);
            if (!taskDeleted)
            {
                return NotFound(new { Message = "Task not found" });
            }
            return NoContent();
        }
    }
}