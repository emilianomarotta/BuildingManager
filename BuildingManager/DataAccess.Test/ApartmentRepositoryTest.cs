using Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Test
{
    [TestClass]
    public class ApartmentRepositoryTest
    {
        private SqliteConnection _connection;
        private BuildingManagerContext _context;
        private ApartmentRepository _apartmentRepository;
        private Manager manager;
        private Company company;
        private CompanyAdmin companyAdmin;
        private Building firstBuilding;
        private Building secondBuilding;
        private Owner owner;

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

            _apartmentRepository = new ApartmentRepository(_context);

            manager = new Manager
            {
                Id = 1,
                Name = "Manager",
                LastName = "Manager",
                Email = "manager@manager.com",
                Password = "Manager1234."
            };
            companyAdmin = new CompanyAdmin
            {
                Id = 2,
                Name = "CAdmin",
                LastName = "CAdmin",
                Email = "company@admin.com",
                Password = "Test.1234."
            };

            company = new Company
            {
                Id = 1,
                Name = "Company",
                companyAdmin = companyAdmin,
                CompanyAdminId = companyAdmin.Id
            };

            firstBuilding = new Building
            {
                Id = 1,
                Name = "FirstBuildingTest",
                Manager = manager,
                Address = "Address, Address 1234",
                Location = "-12.3456, -12.3456",
                Company = company,
                Fees = 1
            };

            secondBuilding = new Building
            {
                Id = 2,
                Name = "SecondBuildingTest",
                Manager = manager,
                Address = "Address, Address 5678",
                Location = "-21.3456, -21.3456",
                Company = company,
                Fees = 1
            };

            owner = new Owner
            {
                Id = 1,
                Name = "Owner",
                LastName = "Owner",
                Email = "owner@owner.com"
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
        }

        [TestMethod]
        public void GetAllApartments()
        {
            var expectedApartments = TestData();
            LoadContext(expectedApartments);

            var retrievedApartments = _apartmentRepository.GetAll<Apartment>();
            CollectionAssert.AreEqual(expectedApartments, retrievedApartments.ToList());
        }

        [TestMethod]
        public void GetAllApartmentsByFilter()
        {
            var apartments = TestData();
            LoadContext(apartments);
            var expectedApartments = apartments;
            var retrievedApartments = _apartmentRepository.GetAll<Apartment>(apartment => apartment.Id < 10);
            CollectionAssert.AreEqual(expectedApartments, retrievedApartments.ToList());
        }

        [TestMethod]
        public void GetApartmentById()
        {
            var apartments = TestData();
            LoadContext(apartments);
            int apartmentdId = 1;
            var expectedApartment = new Apartment { Id = 1, Floor = 1, Number = 1, Building = firstBuilding, Owner = owner, Bedrooms = 1, Bathrooms = 1, Balcony = true };
            var apartmentFromDataBase = _apartmentRepository.Get(a => a.Id == apartmentdId);

            Assert.AreEqual(expectedApartment, apartmentFromDataBase);
        }

        [TestMethod]
        public void InsertApartmentToDataBase()
        {
            var newApartment = new Apartment { Id = 1, Floor = 1, Number = 1, Building = firstBuilding, Owner = owner, Bedrooms = 1, Bathrooms = 1, Balcony = true };
            List<Apartment> expectedApartments = new List<Apartment> { newApartment };
            _apartmentRepository.Insert(newApartment);
            var retrievedApartments = _apartmentRepository.GetAll<Apartment>();

            CollectionAssert.AreEqual(expectedApartments, retrievedApartments.ToList());
        }

        [TestMethod]
        public void UpdateApartmentNumberFromDataBase()
        {
            var apartments = TestData();
            LoadContext(apartments);
            int apartmentdId = 1;
            var apartmentToUpdate = _apartmentRepository.Get(a => a.Id == apartmentdId);
            apartmentToUpdate.Number = 3;

            _apartmentRepository.Update(apartmentToUpdate);

            var updatedApartment = _apartmentRepository.Get(a => a.Id == apartmentdId);
            int expectedNumber = 3;
            Assert.AreEqual(expectedNumber, updatedApartment.Number);
        }

        [TestMethod]
        public void DeleteApartmentFromDataBase()
        {
            var apartmentToDelete = new Apartment { Id = 1, Floor = 1, Number = 1, Building = firstBuilding, Owner = owner, Bedrooms = 1, Bathrooms = 1, Balcony = true };
            _apartmentRepository.Insert(apartmentToDelete);
            _apartmentRepository.Delete(apartmentToDelete);
            var apartmentsAfterDelete = _apartmentRepository.GetAll<Apartment>().ToList();

            CollectionAssert.DoesNotContain(apartmentsAfterDelete, apartmentToDelete);
        }

        private void LoadContext(List<Apartment> apartments)
        {
            _context.Apartments.AddRange(apartments);
            _context.SaveChanges();
        }

        private List<Apartment> TestData()
        {
            return new List<Apartment>
        {
            new Apartment { Id = 1, Floor = 1, Number = 1, Building = firstBuilding, Owner = owner, OwnerId = owner.Id,  Bedrooms = 1, Bathrooms = 1, Balcony = true },
            new Apartment { Id = 2, Floor = 1, Number = 2, Building = secondBuilding, Owner = owner, Bedrooms = 1, OwnerId = owner.Id, Bathrooms = 1, Balcony = true }
        };
        }
    }
}
