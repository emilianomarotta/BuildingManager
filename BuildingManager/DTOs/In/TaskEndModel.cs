using Domain;
using Task = Domain.Task;

namespace DTOs.In
{
    public class TaskEndModel
    {
        public double Cost { get; set; }

        public Task ToEntity()
        {
            return new Task
            {
                EndDate = DateTime.Now,
                Cost = this.Cost
            };
        }
    }
}
