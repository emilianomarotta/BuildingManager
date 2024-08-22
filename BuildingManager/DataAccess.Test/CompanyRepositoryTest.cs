using Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Test
{
    [TestClass]

    public class CompanyRepositoryTest
    {
        private SqliteConnection _connection;
        private BuildingManagerContext _context;
        private CompanyRepository _companyRepository;
        private CompanyAdmin companyAdmin;
        private CompanyAdmin otherCompanyAdmin;

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

            _companyRepository = new CompanyRepository(_context);
            companyAdmin = new CompanyAdmin
            {
                Id = 2,
                Name = "CAdmin",
                LastName = "CAdmin",
                Email = "company@admin.com",
                Password = "Test.1234."
            };

            otherCompanyAdmin = new CompanyAdmin
            {
                Id = 3,
                Name = "CAdmin",
                LastName = "CAdmin",
                Email = "otherAdmin@admin.com",
                Password = "Test.1234."
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
        }

        [TestMethod]
        public void GetAllCompanies()
        {
            var expectedCompanies = TestData();
            LoadContext(expectedCompanies);

            var retrievedCompanies = _companyRepository.GetAll<Company>();
            CollectionAssert.AreEqual(expectedCompanies, retrievedCompanies.ToList());
        }

        [TestMethod]
        public void GetAllCompaniesByFilter()
        {
            var expectedCompanies = TestData();
            LoadContext(expectedCompanies);

            var retrievedCompanies = _companyRepository.GetAll<Company>(company => company.Id < 10);
            CollectionAssert.AreEqual(expectedCompanies, retrievedCompanies.ToList());
        }

        [TestMethod]
        public void GetCompanyById()
        {
            var companies = TestData();
            LoadContext(companies);

            var expectedCompany = new Company { Id = 1, Name = "FirstCompanyTest" };
            var companyFromDataBase = _companyRepository.Get(company => company.Id == expectedCompany.Id);

            Assert.AreEqual(expectedCompany, companyFromDataBase);
        }

        [TestMethod]
        public void InsertCompanyToDataBase()
        {
            var newCompany = new Company { Id = 1, Name = "CompanyTest", companyAdmin = companyAdmin, CompanyAdminId = companyAdmin.Id };
            List<Company> expectedCompanies = new List<Company> { newCompany };
            _companyRepository.Insert(newCompany);
            var retrievedCompanies = _companyRepository.GetAll<Company>();

            CollectionAssert.AreEqual(expectedCompanies, retrievedCompanies.ToList());
        }

        [TestMethod]
        public void UpdateCompanyNameFromDataBase()
        {
            var companies = TestData();
            LoadContext(companies);

            string newCompanyName = "UpdatedName";
            var companyToUpdate = _companyRepository.Get(company => company.Id == 1);
            companyToUpdate.Name = newCompanyName;

            _companyRepository.Update(companyToUpdate);

            var updatedCompany = _companyRepository.Get(company => company.Id == 1);

            Assert.AreEqual(newCompanyName, updatedCompany.Name);
        }

        [TestMethod]
        public void DeleteCompanyFromDataBase()
        {
            var companyToDelete = new Company { Id = 1, Name = "CompanyTest", companyAdmin = companyAdmin, CompanyAdminId = companyAdmin.Id };
            _companyRepository.Insert(companyToDelete);

            _companyRepository.Delete(companyToDelete);
            var companies = _companyRepository.GetAll<Company>().ToList();

            CollectionAssert.DoesNotContain(companies, companyToDelete);
        }

        private void LoadContext(List<Company> companies)
        {
            _context.Companies.AddRange(companies);
            _context.SaveChanges();
        }

        private List<Company> TestData()
        {
            return new List<Company>
        {
            new Company { Id = 1, Name = "FirstCompanyTest", companyAdmin = companyAdmin, CompanyAdminId = companyAdmin.Id },
            new Company { Id = 2, Name = "SecondCompanyTest", companyAdmin = otherCompanyAdmin, CompanyAdminId = otherCompanyAdmin.Id }
        };
        }
    }
}
