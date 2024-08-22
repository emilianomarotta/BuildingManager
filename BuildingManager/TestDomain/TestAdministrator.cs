using Domain;
using Domain.Exceptions;

namespace TestDomain
{
    [TestClass]
    public class TestAdministrator
    {
        User administrator;

        [TestInitialize]
        public void Setup()
        {
            administrator = new Administrator
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
            Assert.AreEqual(expectedName, administrator.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidName()
        {
            administrator.Name = "A";
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidNameWithNumbers()
        {
            administrator.Name = "Test123";
        }

        [TestMethod]
        public void SetValidLastName()
        {
            string expectedLastName = "Test";
            Assert.AreEqual(expectedLastName, administrator.LastName);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidLastName()
        {
            administrator.LastName = "A";
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidLastNameWithNumbers()
        {
            administrator.LastName = "Test123";
        }

        [TestMethod]
        public void SetValidEmail()
        {
            string expectedEmail = "test@test.com";
            Assert.AreEqual(expectedEmail, administrator.Email);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidEmail()
        {
            administrator.Email = "test";
        }

        [TestMethod]
        public void SetValidPassword()
        {
            Assert.AreEqual("Test1234.", administrator.Password);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidPassword()
        {
            administrator.Password = "123";
        }

        [TestMethod]
        public void EqualsAdministratorById()
        {
            User administrator2 = new Administrator
            {
                Id = 1
            };
            Assert.AreEqual(administrator, administrator2);
        }

        [TestMethod]
        public void NotEqualsAdministratorById()
        {
            User administrator2 = new Administrator
            {
                Id = 2
            };
            Assert.AreNotEqual(administrator, administrator2);
        }

        [TestMethod]
        public void EqualsAdministratorByEmail()
        {
            User administrator2 = new Administrator
            {
                Email = "test@test.com"
            };
            Assert.AreEqual(administrator, administrator2);
        }

        [TestMethod]
        public void NotEqualsAdministratorByEmail()
        {
            User administrator2 = new Administrator
            {
                Email = "test2@test.com"
            };
            Assert.AreNotEqual(administrator, administrator2);
        }
    }
}