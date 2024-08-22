using Domain;
using DTOs.In;
using IBusinessLogic;
using IBusinessLogic.Exceptions;
using IDataAccess;
using Task = Domain.Task;

namespace BusinessLogic;

public class TaskLogic : IBusinessLogic<Task>
{
    private readonly IGenericRepository<Task> _taskRepository;
    private readonly IGenericRepository<Category> _categoryRepository;
    private readonly IGenericRepository<Staff> _staffRepository;
    private readonly IGenericRepository<Apartment> _apartmentRepository;
    private readonly IGenericRepository<Building> _buildingRepository;
    private readonly ISessionLogic _sessionLogic;

    public TaskLogic(TaskLogicDTO dto)
    {
        _taskRepository = dto.TaskRepository;
        _categoryRepository = dto.CategoryRepository;
        _staffRepository = dto.StaffRepository;
        _apartmentRepository = dto.ApartmentRepository;
        _buildingRepository = dto.BuildingRepository;
        _sessionLogic = dto.SessionLogic;
    }

    public List<Task> GetAll()
    {
        var currentUser = _sessionLogic.GetCurrentUser();
        if (currentUser is Manager)
        {
            return GetTasksForCurrentUser(currentUser).ToList();
        }
        return _taskRepository.GetAll<Task>().Where(t => t.StaffId == currentUser.Id).ToList();
    }

    public Task GetById(int id)
    {
        var currentUser = _sessionLogic.GetCurrentUser();
        if (currentUser is Manager)
        {
            return GetTasksForCurrentUser(currentUser).FirstOrDefault(t => t.Id == id);
        }
        return _taskRepository.Get(task => task.Id == id && task.StaffId == currentUser.Id);
    }

    public Task Create(Task task)
    {
        var currentUser = _sessionLogic.GetCurrentUser();
        ValidateTaskNotExists(task.Id);
        ValidateCategoryAndApartmentExist(task.CategoryId, task.ApartmentId);
        ValidateUserHasPermissionToCreateTask(currentUser, task.ApartmentId);

        task.CreationDate = DateTime.Now;
        _taskRepository.Insert(task);
        return task;
    }

    public Task Update(int id, Task updatedTask)
    {
        var currentUser = _sessionLogic.GetCurrentUser();
        var task = GetTaskById(id);
        ValidateStaffAssignment(updatedTask.StaffId);
        ValidateUserAuthorizationForUpdate(currentUser, task);

        UpdateTaskProperties(task, updatedTask);

        _taskRepository.Update(task);
        return task;
    }

    public bool Delete(int id)
    {
        var currentUser = _sessionLogic.GetCurrentUser();
        var task = GetTaskById(id);
        if (task == null)
        {
            return false;
        }
        ValidateUserHasPermissionToCreateTask(currentUser, task.ApartmentId);
        _taskRepository.Delete(task);
        return true;
    }

    private Task GetTaskById(int id)
    {
        var task = _taskRepository.Get(t => t.Id == id);
        if (task == null)
        {
            throw new NotFoundException("Task not found");
        }
        return task;
    }

    private void ValidateTaskNotExists(int taskId)
    {
        if (TaskExists(taskId))
        {
            throw new AlreadyExistsException("Task already exists");
        }
    }

    private bool TaskExists(int id)
    {
        return _taskRepository.Get(task => task.Id == id) != null;
    }

    private void ValidateCategoryAndApartmentExist(int categoryId, int apartmentId)
    {
        if (!CategoryExists(categoryId) || !ApartmentExists(apartmentId))
        {
            throw new NotFoundException("Task must have a category and apartment");
        }
    }

    private bool CategoryExists(int categoryId)
    {
        return _categoryRepository.Get(category => category.Id == categoryId) != null;
    }

    private bool ApartmentExists(int apartmentId)
    {
        return _apartmentRepository.Get(apartment => apartment.Id == apartmentId) != null;
    }

    private void ValidateStaffAssignment(int? staffId)
    {
        if (staffId.HasValue && !StaffExists(staffId.Value))
        {
            throw new NotFoundException("Staff assigned does not exist");
        }
    }

    private bool StaffExists(int staffId)
    {
        return _staffRepository.Get(staff => staff.Id == staffId) != null;
    }

    private void ValidateUserHasPermissionToCreateTask(User currentUser, int apartmentId)
    {
        var apartment = _apartmentRepository.Get(a => a.Id == apartmentId);
        if (apartment == null)
        {
            throw new NotFoundException("Apartment not found");
        }
        int buildingId = apartment.BuildingId;
        var building = _buildingRepository.Get(b => b.Id == buildingId);
        if (building.ManagerId != currentUser.Id)
        {
            throw new UnauthorizedException("Unauthorized to create Task in this apartment");
        }
    }

    private void ValidateUserAuthorizationForUpdate(User currentUser, Task task)
    {
        if (currentUser is Staff && task.StaffId != currentUser.Id)
        {
            throw new UnauthorizedException("Unauthorized to update Task of others");
        }

        var apartment = GetApartmentById(task.ApartmentId);
        int buildingId = apartment.BuildingId;
        var building = _buildingRepository.Get(b => b.Id == buildingId);

        if (currentUser is Manager && building.ManagerId != currentUser.Id)
        {
            throw new UnauthorizedException("Unauthorized to update Task of others building");
        }
    }
    private Apartment GetApartmentById(int apartmentId)
    {
        var apartment = _apartmentRepository.Get(a => a.Id == apartmentId);
        if (apartment == null)
        {
            throw new NotFoundException("Apartment not found");
        }
        return apartment;
    }

    private void UpdateTaskProperties(Task task, Task updatedTask)
    {
        task.StaffId = updatedTask.StaffId ?? task.StaffId;
        task.StartDate = updatedTask.StartDate ?? task.StartDate;
        task.EndDate = updatedTask.EndDate;
        task.Cost = updatedTask.Cost;
    }

    public IEnumerable<Task> GetTasksForCurrentUser(User currentUser)
    {
        var buildingIds = _buildingRepository
            .GetAll<Building>()
            .Where(b => b.ManagerId == currentUser.Id)
            .Select(b => b.Id)
            .ToList();

        var apartmentIds = _apartmentRepository
            .GetAll<Apartment>()
            .Where(a => buildingIds.Contains(a.BuildingId))
            .Select(a => a.Id)
            .ToList();

        return _taskRepository
            .GetAll<Task>()
            .Where(t => apartmentIds.Contains(t.ApartmentId))
            .ToList();
    }

}
