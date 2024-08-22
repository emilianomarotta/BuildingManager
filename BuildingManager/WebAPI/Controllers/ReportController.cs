using Domain;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;
using Task = Domain.Task;
namespace WebAPI
{
    [ApiController]
    [Route("api/reports")]
    [AuthenticationFilter("Manager")]
    public class ReportController : ControllerBase
    {
        private readonly IBusinessLogic<Task> _taskLogic;
        private readonly IBusinessLogic<Staff> _staffLogic;
        private readonly ISessionLogic _sessionLogic;
        public ReportController(IBusinessLogic<Task> taskLogic, ISessionLogic sessionLogic, IBusinessLogic<Staff> staffLogic)
        {
            _taskLogic = taskLogic;
            _sessionLogic = sessionLogic;
            _staffLogic = staffLogic;
        }
        [HttpGet("byBuilding")]
        [AuthenticationFilter("Manager")]
        public IActionResult GetReportsByBuilding(int? buildingId = null)
        {
            var currentUser = _sessionLogic.GetCurrentUser();
            if (currentUser is not Manager)
            {
                return Unauthorized();
            }

            IEnumerable<Task> tasks;
            if (buildingId.HasValue)
            {
                tasks = _taskLogic.GetAll()
                    .Where(t => t.Apartment.BuildingId == buildingId.Value);
            }
            else
            {
                tasks = _taskLogic.GetAll();
            }

            var reportData = tasks.GroupBy(t => t.Apartment.Building)
                .Select(g => new
                {
                    Building = g.Key.Name,
                    OpenTasks = g.Count(t => t.StartDate == null && t.EndDate == null),
                    InProgressTasks = g.Count(t => t.StartDate != null && t.EndDate == null),
                    ClosedTasks = g.Count(t => t.EndDate != null)
                })
                .ToList();

            return Ok(reportData);
        }

        [HttpGet("byStaff")]
        [AuthenticationFilter("Manager")]
        public IActionResult GetTasksByStaff(int? staffId = null)
        {
            var currentUser = _sessionLogic.GetCurrentUser();
            if (currentUser is not Manager)
            {
                return Unauthorized();
            }

            IEnumerable<Task> tasks;
            if (staffId.HasValue)
            {
                tasks = _taskLogic.GetAll()
                    .Where(t => t.StaffId == staffId.Value);
            }
            else
            {
                tasks = _taskLogic.GetAll().Where(t=> t.StaffId != null);
            }
            var reportData = tasks.GroupBy(t => t.StaffId)
                .Select(g => new
                {
                    StaffName = _staffLogic.GetById((int)g.Key).Name,
                    OpenTasks = g.Count(t => t.StartDate == null && t.EndDate == null),
                    InProgressTasks = g.Count(t => t.StartDate != null && t.EndDate == null),
                    ClosedTasks = g.Count(t => t.EndDate != null),
                    AverageCloseTime = Math.Round(g.Where(t => t.EndDate != null).Average(t => (t.EndDate - t.StartDate)?.TotalHours) ?? 0) + "hs"
                })
                .ToList();

            return Ok(reportData);
        }
    }
}
