using Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Task = Domain.Task;

namespace DataAccess.Test
{
    [TestClass]
    public class TaskRepositoryTest
    {
        private SqliteConnection _connection;
        private BuildingManagerContext _context;
        private TaskRepository _taskRepository;
        private Building building;
        private Manager manager;
        private CompanyAdmin companyAdmin;
        private Company company;
        private Category category;
        private Apartment apartment;
        private Owner owner;
        private Staff staff;

        [TestInitialize]
        public void Setup()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            var contextOptions = new DbContextOptionsBuilder<BuildingManagerContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new BuildingManagerContext(contextOptions);
            _context.Database.EnsureCreated();
            _taskRepository = new TaskRepository(_context);
            manager = new Manager
            {
                Id = 1,
                Name = "Manager",
                LastName = "Manager",
                Email = "manager@manager.com",
                Password = "Manager1234."
            };

            companyAdmin = new CompanyAdmin
            {
                Id = 3,
                Name = "CAdmin",
                LastName = "CAdmin",
                Email = "company@admin.com",
                Password = "Test.1234."
            };

            company = new Company
            {
                Id = 1,
                Name = "Company",
                companyAdmin = companyAdmin,
                CompanyAdminId = companyAdmin.Id
            };

            building = new Building
            {
                Id = 1,
                Name = "FirstBuildingTest",
                Manager = manager,
                Address = "Address, Address 1234",
                Location = "-12.3456, -12.3456",
                Company = company,
                Fees = 1
            };

            category = new Category
            {
                Id = 1,
                Name = "CategoryTest"
            };

            owner = new Owner
            {
                Id = 1,
                Name = "OwnerTest",
                LastName = "OwnerTest",
                Email = "test@test.com"
            };

            apartment = new Apartment
            {
                Id = 1,
                Building = building,
                Floor = 1,
                Number = 1,
                Owner = owner,
                Balcony = true,
                Bathrooms = 1,
                Bedrooms = 1,
            };

            staff = new Staff
            {
                Id = 2,
                Name = "StaffTest",
                LastName = "StaffTest",
                Email = "testStaff@test.com",
                Password = "Test.1234",
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
        }

        [TestMethod]
        public void GetAllTasks()
        {
            var expectedTasks = TestData();
            LoadContext(expectedTasks);

            var retrievedTasks = _taskRepository.GetAll<Task>();
            CollectionAssert.AreEqual(expectedTasks, retrievedTasks.ToList());
        }

        [TestMethod]
        public void GetAllTasksByFilter()
        {
            var expectedTasks = TestData();
            LoadContext(expectedTasks);

            var retrievedTasks = _taskRepository.GetAll<Task>(task => task.Id < 10);
            CollectionAssert.AreEqual(expectedTasks, retrievedTasks.ToList());
        }

        [TestMethod]
        public void GetTaskById()
        {
            var tasks = TestData();
            LoadContext(tasks);

            var expectedTask = new Task { Id = 1, Category = category, Description = "Test task", Apartment = apartment, Staff = staff };
            var taskFromDataBase = _taskRepository.Get(task => task.Id == expectedTask.Id);

            Assert.AreEqual(expectedTask, taskFromDataBase);
        }

        [TestMethod]
        public void InsertTaskToDataBase()
        {
            var newTask = new Task { Id = 1, Category = category, Description = "Test Task", Apartment = apartment, Staff = staff };
            List<Task> expectedTasks = new List<Task> { newTask };
            _taskRepository.Insert(newTask);
            var retrievedTasks = _taskRepository.GetAll<Task>();

            CollectionAssert.AreEqual(expectedTasks, retrievedTasks.ToList());
        }

        [TestMethod]
        public void UpdateTaskStartDateFromDataBase()
        {
            var tasks = TestData();
            LoadContext(tasks);

            var taskToUpdate = _taskRepository.Get(task => task.Id == 1);
            taskToUpdate.StartDate = DateTime.Today.AddDays(2);

            _taskRepository.Update(taskToUpdate);

            var updatedTask = _taskRepository.Get(task => task.Id == 1);

            Assert.AreEqual(DateTime.Today.AddDays(2), updatedTask.StartDate);
        }

        [TestMethod]
        public void DeleteTaskFromDataBase()
        {
            var taskToDelete = new Task { Id = 1, Category = category, Description = "Test task", Apartment = apartment, Staff = staff };
            _taskRepository.Insert(taskToDelete);

            _taskRepository.Delete(taskToDelete);
            var tasks = _taskRepository.GetAll<Task>().ToList();

            CollectionAssert.DoesNotContain(tasks, taskToDelete);
        }

        private void LoadContext(List<Task> tasks)
        {
            _context.Tasks.AddRange(tasks);
            _context.SaveChanges();
        }

        private List<Task> TestData()
        {
            return new List<Task>
        {
            new Task { Id = 1, Category= category, Description= "Test task", Apartment = apartment, Staff= staff  },
            new Task { Id = 2, Category= category, Description= "Test task", Apartment = apartment, Staff= staff  },

        };
        }
    }
}