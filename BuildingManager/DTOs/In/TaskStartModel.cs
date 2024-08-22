using Task = Domain.Task;

namespace DTOs.In
{
    public class TaskStartModel
    {
        public int StaffId { get; set; }

        public Task ToEntity()
        {
            return new Task
            {
                StartDate = DateTime.Now,
                StaffId = this.StaffId
            };
        }
    }
}
