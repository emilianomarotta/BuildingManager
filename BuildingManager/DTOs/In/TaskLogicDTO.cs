using Domain;
using IDataAccess;
using IBusinessLogic;
using Task = Domain.Task;

namespace DTOs.In
{
    public class TaskLogicDTO
    {
        public IGenericRepository<Task> TaskRepository { get; set; }
        public IGenericRepository<Category> CategoryRepository { get; set; }
        public IGenericRepository<Staff> StaffRepository { get; set; }
        public IGenericRepository<Apartment> ApartmentRepository { get; set; }
        public IGenericRepository<Building> BuildingRepository { get; set; }
        public ISessionLogic SessionLogic { get; set; }

        public TaskLogicDTO(
            IGenericRepository<Task> _taskRepository,
            IGenericRepository<Category> _categoryRepository,
            IGenericRepository<Staff> _staffRepository,
            IGenericRepository<Apartment> _apartmentRepository,
            IGenericRepository<Building> _buildingRepository,
            ISessionLogic _sessionLogic)
        {
            TaskRepository = _taskRepository;
            CategoryRepository = _categoryRepository;
            StaffRepository = _staffRepository;
            ApartmentRepository = _apartmentRepository;
            BuildingRepository = _buildingRepository;
            SessionLogic = _sessionLogic;
        }
    }
}