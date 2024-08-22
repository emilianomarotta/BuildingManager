using Task = Domain.Task;

namespace DTOs.In
{
    public class TaskCreateModel
    {
        public int CategoryId { get; set; }
        public int ApartmentId { get; set; }
        public string Description { get; set; }

        public Task ToEntity()
        {
            return new Task
            {
                CategoryId = this.CategoryId,
                ApartmentId = this.ApartmentId,
                Description = this.Description
            };
        }
    }
}
