using Domain;
using Domain.Exceptions;

namespace TestDomain

{
    [TestClass]
    public class TestBuilding
    {
        Building building;
        private Company company;

        [TestInitialize]
        public void Setup()
        {
            company = new Company
            {
                Id = 1
            };

            building = new Building
            {
                Id = 1,
                Name = "Test",
                Address = "Test, Test 1234",
                Location = "-12.3456, -12.3456",
                Company = company,
                Fees = 1234
            };
        }

        [TestMethod]
        public void SetValidName()
        {
            string expectedName = "Test";
            Assert.AreEqual(building.Name, expectedName);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidName()
        {
            building.Name = "1234";
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetEmptyName()
        {
            building.Name = "";
        }

        [TestMethod]
        public void SetValidAddress()
        {
            string expectedAddress = "Test, Test 1234";
            Assert.AreEqual(building.Address, expectedAddress);
        }

        [TestMethod]
        public void SetValidLocation()
        {
            string expectedLocation = "-12.3456, -12.3456";
            Assert.AreEqual(building.Location, expectedLocation);
        }

        [TestMethod]
        public void SetValidFeesAmount()
        {
            int expectedFees = 1234;
            Assert.AreEqual(building.Fees, expectedFees);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidFeesAmount()
        {
            building.Fees = 0;
        }

        [TestMethod]
        public void SetValidCompany()
        {
            Company expectedCompany = company;
            Assert.AreEqual(expectedCompany, building.Company);
        }

        [TestMethod]
        public void AreEqualBuildingsById()
        {
            Building buildingTest = new Building
            {
                Id = 1,
            };
            Assert.AreEqual(building, buildingTest);
        }

        [TestMethod]
        public void AreNotEqualBuildingsById()
        {
            Building buildingTest = new Building
            {
                Id = 2,
            };
            Assert.AreNotEqual(building, buildingTest);
        }

        [TestMethod]
        public void AreEqualBuildingsByName()
        {
            Building buildingTest = new Building
            {
                Name = "Test",
            };
            Assert.AreEqual(building, buildingTest);
        }

        [TestMethod]
        public void AreNotEqualBuildingsByName()
        {
            Building buildingTest = new Building
            {
                Name = "test",
            };
            Assert.AreNotEqual(building, buildingTest);
        }

        [TestMethod]
        public void AreEqualBuildingsByAddress()
        {
            Building buildingTest = new Building
            {
                Address = "Test, Test 1234",
            };
            Assert.AreEqual(building, buildingTest);
        }

        [TestMethod]
        public void AreNotEqualBuildingsByAddress()
        {
            Building buildingTest = new Building
            {
                Address = "test, test 5678",
            };
            Assert.AreNotEqual(building, buildingTest);
        }

        [TestMethod]
        public void AreEqualBuildingsByLocation()
        {
            Building buildingTest = new Building
            {
                Location = "-12.3456, -12.3456",
            };
            Assert.AreEqual(building, buildingTest);
        }

        [TestMethod]
        public void AreNotEqualBuildingsByLocation()
        {
            Building buildingTest = new Building
            {
                Location = "-34.3456, -34.3456",
            };
            Assert.AreNotEqual(building, buildingTest);
        }
    }
}