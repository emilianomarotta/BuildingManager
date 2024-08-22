using Domain;
using Domain.Exceptions;

namespace TestDomain
{
    [TestClass]
    public class TestManager
    {
        User manager;

        [TestInitialize]
        public void Setup()
        {
            manager = new Manager
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
            Assert.AreEqual(expectedName, manager.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidName()
        {
            manager.Name = "A";
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidNameWithNumbers()
        {
            manager.Name = "Test123";
        }

        [TestMethod]
        public void SetValidLastName()
        {
            string expectedLastName = "Test";
            Assert.AreEqual(expectedLastName, manager.LastName);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidLastName()
        {
            manager.LastName = "A";
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidLastNameWithNumbers()
        {
            manager.LastName = "Test123";
        }

        [TestMethod]
        public void SetValidEmail()
        {
            string expectedEmail = "test@test.com";
            Assert.AreEqual(expectedEmail, manager.Email);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidEmail()
        {
            manager.Email = "test";
        }

        [TestMethod]
        public void SetValidPassword()
        {
            Assert.AreEqual("Test1234.", manager.Password);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidPassword()
        {
            manager.Password = "123";
        }

        [TestMethod]
        public void EqualsManagerById()
        {
            User manager2 = new Manager
            {
                Id = 1
            };
            Assert.AreEqual(manager, manager2);
        }

        [TestMethod]
        public void NotEqualsManagerById()
        {
            User manager2 = new Manager
            {
                Id = 2
            };
            Assert.AreNotEqual(manager, manager2);
        }

        [TestMethod]
        public void EqualsManagerByEmail()
        {
            User manager2 = new Manager
            {
                Email = "test@test.com"
            };
            Assert.AreEqual(manager, manager2);
        }

        [TestMethod]
        public void NotEqualsManagerByEmail()
        {
            User manager2 = new Manager
            {
                Email = "test2@test.com"
            };
            Assert.AreNotEqual(manager, manager2);
        }
    }
}