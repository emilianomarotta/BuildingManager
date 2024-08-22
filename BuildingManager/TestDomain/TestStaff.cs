using Domain;
using Domain.Exceptions;

namespace TestDomain
{
    [TestClass]
    public class TestStaff
    {
        Staff staff;
        Building building;

        [TestInitialize]
        public void Setup()
        {

            staff = new Staff
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
            Assert.AreEqual(expectedName, staff.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidName()
        {
            staff.Name = "A";
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidNameWithNumbers()
        {
            staff.Name = "Test123";
        }

        [TestMethod]
        public void SetValidLastName()
        {
            string expectedLastName = "Test";
            Assert.AreEqual(expectedLastName, staff.LastName);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidLastName()
        {
            staff.LastName = "A";
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidLastNameWithNumbers()
        {
            staff.LastName = "Test123";
        }

        [TestMethod]
        public void SetValidEmail()
        {
            string expectedEmail = "test@test.com";
            Assert.AreEqual(expectedEmail, staff.Email);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidEmail()
        {
            staff.Email = "test";
        }

        [TestMethod]
        public void SetValidPassword()
        {
            Assert.AreEqual("Test1234.", staff.Password);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidPassword()
        {
            staff.Password = "123";
        }

        [TestMethod]
        public void EqualsStaffById()
        {
            User staff2 = new Staff
            {
                Id = 1
            };
            Assert.AreEqual(staff, staff2);
        }

        [TestMethod]
        public void NotEqualsStaffById()
        {
            User staff2 = new Staff
            {
                Id = 2
            };
            Assert.AreNotEqual(staff, staff2);
        }

        [TestMethod]
        public void EqualsStaffByEmail()
        {
            User staff2 = new Staff
            {
                Email = "test@test.com"
            };
            Assert.AreEqual(staff, staff2);
        }

        [TestMethod]
        public void NotEqualsStaffByEmail()
        {
            User staff2 = new Staff
            {
                Email = "test2@test.com"
            };
            Assert.AreNotEqual(staff, staff2);
        }
    }
}