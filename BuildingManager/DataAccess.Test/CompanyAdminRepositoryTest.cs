using Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Test
{
    [TestClass]
    public class CompanyAdminRepositoryTest
    {
        private SqliteConnection _connection;
        private BuildingManagerContext _context;
        private CompanyAdminRepository _companyAdminRepository;

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

            _companyAdminRepository = new CompanyAdminRepository(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
        }

        [TestMethod]
        public void GetAllCompanyAdmins()
        {
            var expectedCompanyAdmins = TestData();
            LoadContext(expectedCompanyAdmins);

            var retrievedCompanyAdmins = _companyAdminRepository.GetAll<CompanyAdmin>();
            CollectionAssert.AreEqual(expectedCompanyAdmins, retrievedCompanyAdmins.ToList());
        }

        [TestMethod]
        public void GetAllCompanyAdminsByFilter()
        {
            var expectedCompanyAdmins = TestData();
            LoadContext(expectedCompanyAdmins);

            var retrievedCompanyAdmins = _companyAdminRepository.GetAll<CompanyAdmin>(companyAdmin => companyAdmin.Id < 10);
            CollectionAssert.AreEqual(expectedCompanyAdmins, retrievedCompanyAdmins.ToList());
        }

        [TestMethod]
        public void GetCompanyAdminById()
        {
            var companyAdmins = TestData();
            LoadContext(companyAdmins);

            var expectedCompanyAdmin = new CompanyAdmin { Id = 1, Name = "CompanyAdminTest", LastName = "FirstLastname", Email = "test@test.com", Password = "Test.1234" };
            var companyAdminFromDataBase = _companyAdminRepository.Get(companyAdmin => companyAdmin.Id == expectedCompanyAdmin.Id);

            Assert.AreEqual(expectedCompanyAdmin, companyAdminFromDataBase);
        }

        [TestMethod]
        public void InsertCompanyAdminToDataBase()
        {
            var newCompanyAdmin = new CompanyAdmin { Id = 1, Name = "CompanyAdminTest", LastName = "FirstLastname", Email = "test@test.com", Password = "Test.1234" };
            List<CompanyAdmin> expectedCompanyAdmins = new List<CompanyAdmin> { newCompanyAdmin };
            _companyAdminRepository.Insert(newCompanyAdmin);
            var retrievedCompanyAdmins = _companyAdminRepository.GetAll<CompanyAdmin>();

            CollectionAssert.AreEqual(expectedCompanyAdmins, retrievedCompanyAdmins.ToList());
        }

        [TestMethod]
        public void UpdateCompanyAdminNameFromDataBase()
        {
            var companyAdmins = TestData();
            LoadContext(companyAdmins);

            string newCompanyAdminName = "UpdatedName";
            var companyAdminToUpdate = _companyAdminRepository.Get(companyAdmin => companyAdmin.Id == 1);
            companyAdminToUpdate.Name = newCompanyAdminName;

            _companyAdminRepository.Update(companyAdminToUpdate);

            var updatedCompanyAdmin = _companyAdminRepository.Get(companyAdmin => companyAdmin.Id == 1);

            Assert.AreEqual(newCompanyAdminName, updatedCompanyAdmin.Name);
        }

        [TestMethod]
        public void DeleteCompanyAdminFromDataBase()
        {
            var companyAdminToDelete = new CompanyAdmin { Id = 1, Name = "CompanyAdminTest", LastName = "FirstLastname", Email = "test@test.com", Password = "Test.1234" };
            _companyAdminRepository.Insert(companyAdminToDelete);

            _companyAdminRepository.Delete(companyAdminToDelete);
            var companyAdmins = _companyAdminRepository.GetAll<CompanyAdmin>().ToList();

            CollectionAssert.DoesNotContain(companyAdmins, companyAdminToDelete);
        }

        private void LoadContext(List<CompanyAdmin> companyAdmins)
        {
            _context.CompanyAdmins.AddRange(companyAdmins);
            _context.SaveChanges();
        }

        private List<CompanyAdmin> TestData()
        {
            return new List<CompanyAdmin>
        {
            new CompanyAdmin { Id = 1, Name = "CompanyAdminTest", LastName="FirstLastname", Email="test@test.com", Password="Test.1234"},
            new CompanyAdmin { Id = 2, Name = "SecondCompanyAdminTest", LastName="SecondLastname", Email="test2@test.com", Password="Test.1234"},
        };
        }
    }
}