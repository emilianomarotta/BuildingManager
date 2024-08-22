using Domain;
using Domain.Exceptions;

namespace TestDomain
{
    [TestClass]
    public class TestOwner
    {
        Owner owner;

        [TestInitialize]
        public void Setup()
        {
            owner = new Owner
            {
                Id = 1,
                Name = "Test",
                LastName = "Test",
                Email = "test@test.com"
            };
        }

        [TestMethod]
        public void SetValidName()
        {
            string expectedName = "Test";
            Assert.AreEqual(expectedName, owner.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidName()
        {
            owner.Name = "Tes";
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidNameWithNumbers()
        {
            owner.Name = "Test123";
        }

        [TestMethod]
        public void SetValidLastName()
        {
            string expectedLastName = "Test";
            Assert.AreEqual(expectedLastName, owner.LastName);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidLastName()
        {
            owner.LastName = "A";
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidLastNameWithNumbers()
        {
            owner.LastName = "Test123";
        }

        [TestMethod]
        public void SetValidEmail()
        {
            string expectedEmail = "test@test.com";
            Assert.AreEqual(expectedEmail, owner.Email);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidEmail()
        {
            owner.Email = "test";
        }


        [TestMethod]
        public void EqualsOwnerById()
        {
            Owner ownerTest = new Owner
            {
                Id = 1
            };
            Assert.AreEqual(owner, ownerTest);
        }

        [TestMethod]
        public void NotEqualsOwnerById()
        {
            Owner ownerTest = new Owner
            {
                Id = 2
            };
            Assert.AreNotEqual(owner, ownerTest);
        }

        [TestMethod]
        public void EqualsOwnerByEmail()
        {
            Owner ownerTest = new Owner
            {
                Email = "test@test.com"
            };
            Assert.AreEqual(owner, ownerTest);
        }

        [TestMethod]
        public void NotEqualsOwnerByEmail()
        {
            Owner ownerTest = new Owner
            {
                Email = "test2@test.com"
            };
            Assert.AreNotEqual(owner, ownerTest);
        }
    }
}