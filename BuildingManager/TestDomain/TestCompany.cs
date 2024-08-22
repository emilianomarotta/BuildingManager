using Domain;
using Domain.Exceptions;

namespace TestDomain
{
    [TestClass]
    public class TestCompany
    {
        Company company;

        [TestInitialize]
        public void Setup()
        {
            company = new Company
            {
                Id = 1,
                Name = "Test"
            };
        }

        [TestMethod]
        public void SetValidName()
        {
            string expectedName = "Test";
            Assert.AreEqual(expectedName, company.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidName()
        {
            company.Name = "Tes";
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidNameWithNumbers()
        {
            company.Name = "Test123";
        }

        [TestMethod]
        public void SetValidCompanyAdmin()
        {
            CompanyAdmin companyAdmin = new CompanyAdmin
            {
                Id = 1,
                Name = "company",
                LastName = "Test",
                Email = "test@test.com",
                Password = "Test.1234"
            };
            company.companyAdmin = companyAdmin;
            Assert.AreEqual(companyAdmin, company.companyAdmin);
        }

        [TestMethod]
        public void EqualCompaniesById()
        {
            Company companyTest = new Company
            {
                Id = 1
            };
            Assert.AreEqual(company, companyTest);
        }

        [TestMethod]
        public void NotEqualCompaniesById()
        {
            Company companyTest = new Company
            {
                Id = 2
            };
            Assert.AreNotEqual(company, companyTest);
        }

        [TestMethod]
        public void AreEqualCompaniesByName()
        {
            Company companyTest = new Company
            {
                Name = "Test",
            };
            Assert.AreEqual(company, companyTest);
        }

        [TestMethod]
        public void AreNotEqualCompaniesByName()
        {
            Company companyTest = new Company
            {
                Name = "test",
            };
            Assert.AreNotEqual(company, companyTest);
        }
    }
}