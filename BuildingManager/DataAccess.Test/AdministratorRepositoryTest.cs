using Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Test
{
    [TestClass]
    public class AdministratorRepositoryTest
    {
        private SqliteConnection _connection;
        private BuildingManagerContext _context;
        private AdministratorRepository _administratorRepository;

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

            _administratorRepository = new AdministratorRepository(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
        }

        [TestMethod]
        public void GetAllAdministrators()
        {
            var expectedAdministrators = TestData();
            LoadContext(expectedAdministrators);

            var retrievedAdministrators = _administratorRepository.GetAll<Administrator>();
            CollectionAssert.AreEqual(expectedAdministrators, retrievedAdministrators.ToList());
        }

        [TestMethod]
        public void GetAllAdministratorsByFilter()
        {
            var expectedAdministrators = TestData();
            LoadContext(expectedAdministrators);

            var retrievedAdministrators = _administratorRepository.GetAll<Administrator>(administrator => administrator.Id < 10);
            CollectionAssert.AreEqual(expectedAdministrators, retrievedAdministrators.ToList());
        }

        [TestMethod]
        public void GetAdministratorById()
        {
            var administrators = TestData();
            LoadContext(administrators);

            var expectedAdministrator = new Administrator { Id = 1, Name = "AdministratorTest", LastName = "FirstLastname", Email = "test@test.com", Password = "Test.1234" };
            var administratorFromDataBase = _administratorRepository.Get(administrator => administrator.Id == expectedAdministrator.Id);

            Assert.AreEqual(expectedAdministrator, administratorFromDataBase);
        }

        [TestMethod]
        public void InsertAdministratorToDataBase()
        {
            var newAdministrator = new Administrator { Id = 1, Name = "AdministratorTest", LastName = "FirstLastname", Email = "test@test.com", Password = "Test.1234" };
            List <Administrator> expectedAdministrators = new List<Administrator> { newAdministrator };
            _administratorRepository.Insert(newAdministrator);
            var retrievedAdministrators = _administratorRepository.GetAll<Administrator>();
            
            CollectionAssert.AreEqual(expectedAdministrators, retrievedAdministrators.ToList());
        }

        [TestMethod]
        public void UpdateAdministratorNameFromDataBase()
        {
            var administrators = TestData();
            LoadContext(administrators);

            string newAdministratorName = "UpdatedName";
            var administratorToUpdate = _administratorRepository.Get(administrator => administrator.Id == 1);
            administratorToUpdate.Name = newAdministratorName;

            _administratorRepository.Update(administratorToUpdate);

            var updatedAdministrator = _administratorRepository.Get(administrator => administrator.Id == 1);

            Assert.AreEqual(newAdministratorName, updatedAdministrator.Name);
        }

        [TestMethod]
        public void DeleteAdministratorFromDataBase()
        {
            var administratorToDelete = new Administrator { Id = 1, Name = "AdministratorTest", LastName = "FirstLastname", Email = "test@test.com", Password = "Test.1234" };
            _administratorRepository.Insert(administratorToDelete);

            _administratorRepository.Delete(administratorToDelete);
            var administrators = _administratorRepository.GetAll<Administrator>().ToList();

            CollectionAssert.DoesNotContain(administrators, administratorToDelete);
        }

        private void LoadContext(List<Administrator> administrators)
        {
            _context.Administrators.AddRange(administrators);
            _context.SaveChanges();
        }

        private List<Administrator> TestData()
        {
            return new List<Administrator>
        {
            new Administrator { Id = 1, Name = "AdministratorTest", LastName="FirstLastname", Email="test@test.com", Password="Test.1234"},
            new Administrator { Id = 2, Name = "SecondAdministratorTest", LastName="SecondLastname", Email="test2@test.com", Password="Test.1234"},
        };
        }
    }
}