using Moq;
using Domain;
using BusinessLogic;
using IBusinessLogic.Exceptions;
using IDataAccess;
using Task = Domain.Task;
using IBusinessLogic;
using DTOs.In;

namespace TestBusinessLogic;

[TestClass]
public class TaskLogicTest
{
    private Mock<IGenericRepository<Task>> _taskRepository;
    private Mock<IGenericRepository<Category>> _categoryRepository;
    private Mock<IGenericRepository<Staff>> _staffRepository;
    private Mock<IGenericRepository<Apartment>> _apartmentRepository;
    private Mock<IGenericRepository<Building>> _buildingRepository;
    private Mock<ISessionLogic> _sessionLogic;
    private TaskLogic _taskLogic;
    private Task task;
    private Category category;
    private Staff staff;
    private Apartment apartment;
    private Building building;
    private Manager manager;
    private Owner owner;

    [TestInitialize]
    public void Setup()
    {
        _taskRepository = new Mock<IGenericRepository<Task>>(MockBehavior.Strict);
        _categoryRepository = new Mock<IGenericRepository<Category>>(MockBehavior.Strict);
        _staffRepository = new Mock<IGenericRepository<Staff>>(MockBehavior.Loose);
        _apartmentRepository = new Mock<IGenericRepository<Apartment>>(MockBehavior.Strict);
        _buildingRepository = new Mock<IGenericRepository<Building>>(MockBehavior.Strict);
        _sessionLogic = new Mock<ISessionLogic>(MockBehavior.Strict);
        var dto = new TaskLogicDTO(
            _taskRepository.Object,
            _categoryRepository.Object,
            _staffRepository.Object,
            _apartmentRepository.Object,
            _buildingRepository.Object,
            _sessionLogic.Object);

        _taskLogic = new TaskLogic(dto);

        category = new Category
        {
            Id = 1,
            Name = "Category"
        };
        manager = new Manager
        {
            Id = 1
        };
        building = new Building
        {
            Id = 1,
            ManagerId = 1
        };

        owner = new Owner
        {
            Id = 1,
        };

        apartment = new Apartment
        {
            Id = 1,
            Number = 1,
            Building = building,
            BuildingId = building.Id,
            Owner = owner,
            OwnerId = owner.Id,
            Floor = 1,
            Bedrooms = 1,
            Bathrooms = 1,
            Balcony = true,
        };

        staff = new Staff
        {
            Id = 1,
            Name = "Staff",
            LastName = "Staff",
            Email = "staff@staff.com",
            Password = "Staff1234."
        };

        task = new Task
        {
            Id = 1,
            Category = category,
            CategoryId = category.Id,
            Description = "Description",
            Apartment = apartment,
            ApartmentId = apartment.Id,
            Cost = 100,
        };
    }

    [TestMethod]
    public void GetAllTasks()
    {
        var expectedTasks = new List<Task> { task };
        var buildings = new List<Building> { building };
        var apartments = new List<Apartment> { apartment };
        _buildingRepository.Setup(b => b.GetAll<Building>()).Returns(buildings);
        _apartmentRepository.Setup(b => b.GetAll<Apartment>()).Returns(apartments);
        _taskRepository.Setup(t => t.GetAll<Task>()).Returns(expectedTasks);
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        var result = _taskLogic.GetAll();

        _taskRepository.VerifyAll();
        CollectionAssert.AreEqual(expectedTasks, result);
    }

    [TestMethod]
    public void GetAllTasksByFilter()
    {
        int taskId = task.Id;
        var expectedTasks = new List<Task> { task };
        var buildings = new List<Building> { building };
        var apartments = new List<Apartment> { apartment };
        _buildingRepository.Setup(b => b.GetAll<Building>()).Returns(buildings);
        _apartmentRepository.Setup(b => b.GetAll<Apartment>()).Returns(apartments);
        _taskRepository.Setup(t => t.GetAll<Task>()).Returns(expectedTasks);
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        Task returnedTask = _taskLogic.GetById(taskId);

        _taskRepository.VerifyAll();
        Assert.AreEqual(task, returnedTask);
    }

    [TestMethod]
    public void CreateTask()
    {
        int taskId = task.Id;
        int categoryId = category.Id;
        int apartmentId = apartment.Id;
        int buildingId = building.Id;
        _taskRepository.Setup(t => t.Insert(task));
        _taskRepository.Setup(t => t.Get(t => t.Id == taskId, null)).Returns((Task)null);
        _categoryRepository.Setup(c => c.Get(c => c.Id == categoryId, null)).Returns(category);
        _apartmentRepository.Setup(a => a.Get(a => a.Id == apartmentId, null)).Returns(apartment);
        _buildingRepository.Setup(b => b.Get(b => b.Id == buildingId, null)).Returns(building);
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);

        Task newTask = _taskLogic.Create(task);

        _taskRepository.VerifyAll();
        _categoryRepository.VerifyAll();
        _apartmentRepository.VerifyAll();
        Assert.AreEqual(task, newTask);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public void CreateTaskCategoryNotFound()
    {
        int taskId = task.Id;
        int categoryId = category.Id;
        int apartmentId = apartment.Id;

        _taskRepository.Setup(t => t.Get(t => t.Id == taskId, null)).Returns((Task)null);
        _categoryRepository.Setup(c => c.Get(c => c.Id == categoryId, null)).Returns((Category)null);
        _apartmentRepository.Setup(a => a.Get(a => a.Id == apartmentId, null)).Returns(apartment);
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);

        _taskLogic.Create(task);
    }


    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public void CreateTaskApartmentNotFound()
    {
        int taskId = task.Id;
        int categoryId = category.Id;
        int apartmentId = apartment.Id;

        _taskRepository.Setup(t => t.Get(t => t.Id == taskId, null)).Returns((Task)null);
        _categoryRepository.Setup(c => c.Get(c => c.Id == categoryId, null)).Returns(category);
        _apartmentRepository.Setup(a => a.Get(a => a.Id == apartmentId, null)).Returns((Apartment)null);
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);

        _taskLogic.Create(task);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void CreateTaskAlreadyExists()
    {
        int taskId = task.Id;
        int categoryId = category.Id;
        int apartmentId = apartment.Id;

        _taskRepository.Setup(t => t.Get(t => t.Id == taskId, null)).Returns(task);
        _categoryRepository.Setup(c => c.Get(c => c.Id == categoryId, null)).Returns(category);
        _apartmentRepository.Setup(a => a.Get(a => a.Id == apartmentId, null)).Returns(apartment);
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);

        _taskLogic.Create(task);
    }

    [TestMethod]
    public void UpdateTask()
    {
        int taskId = task.Id;
        int staffId = staff.Id;
        int apartmentId = apartment.Id;
        int buildingId = building.Id;
        Task updatedTask = new Task
        {
            Id = taskId,
            Category = category,
            CategoryId = category.Id,
            Description = "Updated description",
            Staff = staff,
            StaffId = staff.Id,
            Apartment = apartment,
            ApartmentId = apartment.Id,
            StartDate = DateTime.Today,
            Cost = 200,
        };
        _apartmentRepository.Setup(a => a.Get(a => a.Id == apartmentId, null)).Returns(apartment);
        _buildingRepository.Setup(b => b.Get(b => b.Id == buildingId, null)).Returns(building);
        _taskRepository.Setup(t => t.Get(t => t.Id == taskId, null)).Returns(task);
        _staffRepository.Setup(s => s.Get(s => s.Id == staffId, null)).Returns(staff);
        _apartmentRepository.Setup(a => a.Get(a => a.Id == apartmentId, null)).Returns(apartment);
        _taskRepository.Setup(t => t.Update(task));
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);

        Task result = _taskLogic.Update(taskId, updatedTask);

        _taskRepository.VerifyAll();
        _staffRepository.VerifyAll();
        Assert.AreEqual(updatedTask, result);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public void UpdateTaskNotFound()
    {
        int taskId = task.Id;

        Task updatedTask = new Task
        {
            Id = taskId,
            Category = category,
            CategoryId = category.Id,
            Description = "Updated description",
            Staff = staff,
            StaffId = staff.Id,
            Apartment = apartment,
            ApartmentId = apartment.Id,
            Cost = 200,
        };

        _taskRepository.Setup(t => t.Get(t => t.Id == taskId, null)).Returns((Task)null);
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        _taskLogic.Update(taskId, updatedTask);
        _taskRepository.VerifyAll();
        _staffRepository.VerifyAll();
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public void UpdateTaskStaffNotFound()
    {
        int taskId = task.Id;
        int staffId = staff.Id;

        Task updatedTask = new Task
        {
            Id = taskId,
            Category = category,
            CategoryId = category.Id,
            Description = "Updated description",
            Staff = staff,
            StaffId = staff.Id,
            Apartment = apartment,
            ApartmentId = apartment.Id,
            Cost = 200,
        };
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        _taskRepository.Setup(t => t.Get(t => t.Id == taskId, null)).Returns(task);
        _staffRepository.Setup(s => s.Get(s => s.Id == staffId, null)).Returns((Staff)null);

        _taskLogic.Update(taskId, updatedTask);
    }

    [TestMethod]

    public void DeleteTask()
    {
        int taskId = task.Id;
        int apartmentId = apartment.Id;
        int buildingId = building.Id;
        _taskRepository.Setup(t => t.Get(t => t.Id == taskId, null)).Returns(task);
        _taskRepository.Setup(t => t.Delete(task));
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        _apartmentRepository.Setup(a => a.Get(a => a.Id == apartmentId, null)).Returns(apartment);
        _buildingRepository.Setup(b => b.Get(b => b.Id == buildingId, null)).Returns(building);
        bool result = _taskLogic.Delete(taskId);

        _taskRepository.VerifyAll();
        Assert.IsTrue(result);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public void DeleteTaskNotFound()
    {
        int taskId = task.Id;

        _taskRepository.Setup(t => t.Get(t => t.Id == taskId, null)).Returns((Task)null);
        _sessionLogic.Setup(s => s.GetCurrentUser(null)).Returns((User)manager);
        bool result = _taskLogic.Delete(taskId);

        _taskRepository.VerifyAll();
    }
}

