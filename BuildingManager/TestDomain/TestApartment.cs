using Domain;
using Domain.Exceptions;

namespace TestDomain
{
    [TestClass]
    public class TestApartment
    {
        Apartment apartment;
        Building building;
        Owner owner;

        [TestInitialize]
        public void Setup()
        {
            building = new Building
            {
                Id = 1,
                Name = "TestBuilding"
            };

            owner = new Owner
            {
                Id = 1,
                Email = "test@test.com"
            };

            apartment = new Apartment
            {
                Floor = 1,
                Number = 1,
                Bedrooms = 1,
                Bathrooms = 1,
                Balcony = true,
                Id = 1,
                Building = building,
                Owner = owner,

            };
        }

        [TestMethod]
        public void SetValidBathroomAmount()
        {
            int expectedAmount = 1;
            Assert.AreEqual(apartment.Bathrooms, expectedAmount);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidBathroomsAmount()
        {
            apartment.Bathrooms = 0;
        }

        [TestMethod]
        public void SetValidBedroomsAmount()
        {
            int expectedAmount = 1;
            Assert.AreEqual(apartment.Bedrooms, expectedAmount);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidBedroomsAmount()
        {
            apartment.Bedrooms = -1;
        }

        [TestMethod]
        public void SetValidNumber()
        {
            int expectedNumber = 1;
            Assert.AreEqual(apartment.Number, expectedNumber);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidNumber()
        {
            apartment.Number = 0;
        }

        [TestMethod]
        public void AreEqualApartmentsById()
        {
            Apartment apartmentTest = new Apartment
            {
                Id = 1,
            };
            Assert.AreEqual(apartment, apartmentTest);
        }

        [TestMethod]
        public void AreNotEqualApartmentsById()
        {
            Apartment apartmentTest = new Apartment
            {
                Id = 2,
            };
            Assert.AreNotEqual(apartment, apartmentTest);
        }

        [TestMethod]
        public void AreEqualApartmentsBySameProperties()
        {
            Apartment apartmentTest = new Apartment
            {
                Building = building,
                Floor = 1,
                Number = 1
            };
            Assert.AreEqual(apartment, apartmentTest);
        }

        [TestMethod]
        public void AreNotEqualApartmentsByFloor()
        {
            Apartment apartmentTest = new Apartment
            {
                Building = building,
                Floor = 2,
                Number = 1
            };
            Assert.AreNotEqual(apartment, apartmentTest);
        }

        [TestMethod]
        public void AreNotEqualApartmentsByNumber()
        {
            Apartment apartmentTest = new Apartment
            {
                Building = building,
                Floor = 1,
                Number = 2
            };
            Assert.AreNotEqual(apartment, apartmentTest);
        }

        [TestMethod]
        public void AreNotEqualApartmentsByBuilding()
        {
            Building buildingTest = new Building
            {
                Id = 2
            };
            Apartment apartmentTest = new Apartment
            {
                Building = buildingTest,
                Floor = 1,
                Number = 1
            };
            Assert.AreNotEqual(apartment, apartmentTest);
        }
    }
}