using Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Test
{
    [TestClass]
    public class BuildingRepositoryTest
    {
        private SqliteConnection _connection;
        private BuildingManagerContext _context;
        private BuildingRepository _buildingRepository;
        private Manager manager;
        private CompanyAdmin companyAdmin;
        private Company company;

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

            _buildingRepository = new BuildingRepository(_context);

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
                Id = 2,
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
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
        }

        [TestMethod]
        public void GetAllBuildings()
        {
            var expectedBuildings = TestData();
            LoadContext(expectedBuildings);

            var retrievedBuildings = _buildingRepository.GetAll<Building>();
            CollectionAssert.AreEqual(expectedBuildings, retrievedBuildings.ToList());
        }

        [TestMethod]
        public void GetAllBuildingsByFilter()
        {
            var expectedBuildings = TestData();
            LoadContext(expectedBuildings);

            var retrievedBuildings = _buildingRepository.GetAll<Building>(building => building.Id < 10);
            CollectionAssert.AreEqual(expectedBuildings, retrievedBuildings.ToList());
        }

        [TestMethod]
        public void GetBuildingById()
        {
            var buildings = TestData();
            LoadContext(buildings);

            var expectedBuilding = new Building { Id = 1, Name = "FirstBuildingTest", Manager = manager, Address = "Address, Address 1234", Location = "-12.3456, -12.3456", Company = company, Fees = 1 };
            var buildingFromDataBase = _buildingRepository.Get(building => building.Id == expectedBuilding.Id);

            Assert.AreEqual(expectedBuilding, buildingFromDataBase);
        }

        [TestMethod]
        public void InsertBuildingToDataBase()
        {
            var newBuilding = new Building { Id = 1, Name = "FirstBuildingTest", Manager = manager, Address = "Address, Address 1234", Location = "-12.3456, -12.3456", Company = company, Fees = 1 };
            List<Building> expectedBuildings = new List<Building> { newBuilding };
            _buildingRepository.Insert(newBuilding);
            var retrievedBuildings = _buildingRepository.GetAll<Building>();

            CollectionAssert.AreEqual(expectedBuildings, retrievedBuildings.ToList());
        }

        [TestMethod]
        public void UpdateBuildingNameFromDataBase()
        {
            var buildings = TestData();
            LoadContext(buildings);

            string newBuildingName = "UpdatedName";
            var buildingToUpdate = _buildingRepository.Get(building => building.Id == 1);
            buildingToUpdate.Name = newBuildingName;

            _buildingRepository.Update(buildingToUpdate);

            var updatedBuilding = _buildingRepository.Get(building => building.Id == 1);

            Assert.AreEqual(newBuildingName, updatedBuilding.Name);
        }

        [TestMethod]
        public void DeleteBuildingFromDataBase()
        {
            var buildingToDelete = new Building { Id = 1, Name = "FirstBuildingTest", Manager = manager, Address = "Address, Address 1234", Location = "-12.3456, -12.3456", Company = company, Fees = 1 };
            _buildingRepository.Insert(buildingToDelete);

            _buildingRepository.Delete(buildingToDelete);
            var buildings = _buildingRepository.GetAll<Building>().ToList();

            CollectionAssert.DoesNotContain(buildings, buildingToDelete);
        }

        private void LoadContext(List<Building> buildings)
        {
            _context.Buildings.AddRange(buildings);
            _context.SaveChanges();
        }

        private List<Building> TestData()
        {
            return new List<Building>
        {
            new Building { Id = 1, Name = "FirstBuildingTest", Manager = manager, Address = "Address, Address 1234", Location = "-12.3456, -12.3456", Company = company, Fees = 1 },
            new Building { Id = 2, Name = "SecondBuildingTest", Manager = manager, Address = "Address, Address 5678", Location = "-23.4567, -23.4567", Company = company, Fees = 1  }
        };
        }
    }
}
