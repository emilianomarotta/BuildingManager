using Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataAccess.Test
{
    [TestClass]
    public class ManagerRepositoryTest
    {
        private SqliteConnection _connection;
        private BuildingManagerContext _context;
        private ManagerRepository _managerRepository;

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

            _managerRepository = new ManagerRepository(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
        }

        [TestMethod]
        public void GetAllManagers()
        {
            var expectedManagers = TestData();
            LoadContext(expectedManagers);

            var retrievedManagers = _managerRepository.GetAll<Manager>();
            CollectionAssert.AreEqual(expectedManagers, retrievedManagers.ToList());
        }

        [TestMethod]
        public void GetAllManagersByFilter()
        {
            var expectedManagers = TestData();
            LoadContext(expectedManagers);

            var retrievedManagers = _managerRepository.GetAll<Manager>(manager => manager.Id < 10);
            CollectionAssert.AreEqual(expectedManagers, retrievedManagers.ToList());
        }

        [TestMethod]
        public void GetManagerById()
        {
            var managers = TestData();
            LoadContext(managers);

            var expectedManager = new Manager { Id = 1, Name = "ManagerTest", LastName = "FirstLastname", Email = "test@test.com", Password = "Test.1234" };
            var managerFromDataBase = _managerRepository.Get(manager => manager.Id == expectedManager.Id);

            Assert.AreEqual(expectedManager, managerFromDataBase);
        }

        [TestMethod]
        public void InsertManagerToDataBase()
        {
            var newManager = new Manager { Id = 1, Name = "ManagerTest", LastName = "FirstLastname", Email = "test@test.com", Password = "Test.1234" };
            List <Manager> expectedManagers = new List<Manager> { newManager };
            _managerRepository.Insert(newManager);
            var retrievedManagers = _managerRepository.GetAll<Manager>();
            
            CollectionAssert.AreEqual(expectedManagers, retrievedManagers.ToList());
        }

        [TestMethod]
        public void UpdateManagerNameFromDataBase()
        {
            var managers = TestData();
            LoadContext(managers);

            string newManagerName = "UpdatedName";
            var managerToUpdate = _managerRepository.Get(manager => manager.Id == 1);
            managerToUpdate.Name = newManagerName;

            _managerRepository.Update(managerToUpdate);

            var updatedManager = _managerRepository.Get(manager => manager.Id == 1);

            Assert.AreEqual(newManagerName, updatedManager.Name);
        }

        [TestMethod]
        public void DeleteManagerFromDataBase()
        {
            var managerToDelete = new Manager { Id = 1, Name = "ManagerTest", LastName = "FirstLastname", Email = "test@test.com", Password = "Test.1234" };
            _managerRepository.Insert(managerToDelete);

            _managerRepository.Delete(managerToDelete);
            var managers = _managerRepository.GetAll<Manager>().ToList();

            CollectionAssert.DoesNotContain(managers, managerToDelete);
        }

        private void LoadContext(List<Manager> managers)
        {
            _context.Managers.AddRange(managers);
            _context.SaveChanges();
        }

        private List<Manager> TestData()
        {
            return new List<Manager>
        {
            new Manager { Id = 1, Name = "ManagerTest", LastName="FirstLastname", Email="test@test.com", Password="Test.1234"},
            new Manager { Id = 2, Name = "SecondManagerTest", LastName="SecondLastname", Email="test2@test.com", Password="Test.1234"},
        };
        }
    }
}