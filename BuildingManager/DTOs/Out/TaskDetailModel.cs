using Task = Domain.Task;

namespace DTOs.Out
{
    public class TaskDetailModel
    {
        public TaskDetailModel(Task task)
        {
            Id = task.Id;
            CategoryId = task.CategoryId;
            ApartmentId = task.ApartmentId;
            StaffId = task.StaffId;
            Description = task.Description;
            CreationDate = task.CreationDate;
            StartDate = task.StartDate;
            EndDate = task.EndDate;
            Cost = task.Cost;
        }
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int ApartmentId { get; set; }
        public int? StaffId { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double Cost { get; set; }

        public override bool Equals(object? obj)
        {
            var otherTaskDetailModel = obj as TaskDetailModel;
            return Id == otherTaskDetailModel.Id;
        }
    }
}
