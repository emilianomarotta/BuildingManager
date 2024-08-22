using Domain;
using Domain.Exceptions;

namespace TestDomain
{
    [TestClass]
    public class TestCompanyAdmin
    {
        CompanyAdmin companyAdmin;

        [TestInitialize]
        public void Setup()
        {
            companyAdmin = new CompanyAdmin
            {
                Id = 1,
                Name = "Test",
                LastName = "Test",
                Email = "test@test.com",
                Password = "Test1234."
            };
        }

        [TestMethod]
        public void SetValidName()
        {
            string expectedName = "Test";
            Assert.AreEqual(expectedName, companyAdmin.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidName()
        {
            companyAdmin.Name = "A";
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidNameWithNumbers()
        {
            companyAdmin.Name = "Test123";
        }

        [TestMethod]
        public void SetValidLastName()
        {
            string expectedLastName = "Test";
            Assert.AreEqual(expectedLastName, companyAdmin.LastName);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidLastName()
        {
            companyAdmin.LastName = "A";
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidLastNameWithNumbers()
        {
            companyAdmin.LastName = "Test123";
        }

        [TestMethod]
        public void SetValidEmail()
        {
            string expectedEmail = "test@test.com";
            Assert.AreEqual(expectedEmail, companyAdmin.Email);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidEmail()
        {
            companyAdmin.Email = "test";
        }

        [TestMethod]
        public void SetValidPassword()
        {
            Assert.AreEqual("Test1234.", companyAdmin.Password);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidPassword()
        {
            companyAdmin.Password = "123";
        }

        [TestMethod]
        public void EqualsCompanyAdminById()
        {
            User companyAdmin2 = new CompanyAdmin
            {
                Id = 1
            };
            Assert.AreEqual(companyAdmin, companyAdmin2);
        }

        [TestMethod]
        public void NotEqualsCompanyAdminById()
        {
            User companyAdmin2 = new CompanyAdmin
            {
                Id = 2
            };
            Assert.AreNotEqual(companyAdmin, companyAdmin2);
        }

        [TestMethod]
        public void EqualsCompanyAdminByEmail()
        {
            User companyAdmin2 = new CompanyAdmin
            {
                Email = "test@test.com"
            };
            Assert.AreEqual(companyAdmin, companyAdmin2);
        }

        [TestMethod]
        public void NotEqualsCompanyAdminByEmail()
        {
            User companyAdmin2 = new CompanyAdmin
            {
                Email = "test2@test.com"
            };
            Assert.AreNotEqual(companyAdmin, companyAdmin2);
        }
    }
}

