using Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Test
{
    [TestClass]
    public class OwnerRepositoryTest
    {
        private SqliteConnection _connection;
        private BuildingManagerContext _context;
        private OwnerRepository _ownerRepository;

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

            _ownerRepository = new OwnerRepository(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
        }

        [TestMethod]
        public void GetAllOwners()
        {
            var expectedOwners = TestData();
            LoadContext(expectedOwners);

            var retrievedOwners = _ownerRepository.GetAll<Owner>();
            CollectionAssert.AreEqual(expectedOwners, retrievedOwners.ToList());
        }

        [TestMethod]
        public void GetAllOwnersByFilter()
        {
            var expectedOwners = TestData();
            LoadContext(expectedOwners);

            var retrievedOwners = _ownerRepository.GetAll<Owner>(owner => owner.Id < 10);
            CollectionAssert.AreEqual(expectedOwners, retrievedOwners.ToList());
        }

        [TestMethod]
        public void GetOwnerById()
        {
            var owners = TestData();
            LoadContext(owners);

            var expectedOwner = new Owner { Id = 1, Name = "OwnerTest", LastName = "FirstLastname", Email = "test@test.com" };
            var ownerFromDataBase = _ownerRepository.Get(owner => owner.Id == expectedOwner.Id);

            Assert.AreEqual(expectedOwner, ownerFromDataBase);
        }

        [TestMethod]
        public void InsertOwnerToDataBase()
        {
            var newOwner = new Owner { Id = 1, Name = "OwnerTest", LastName = "FirstLastname", Email = "test@test.com" };
            List <Owner> expectedOwners = new List<Owner> { newOwner };
            _ownerRepository.Insert(newOwner);
            var retrievedOwners = _ownerRepository.GetAll<Owner>();
            
            CollectionAssert.AreEqual(expectedOwners, retrievedOwners.ToList());
        }

        [TestMethod]
        public void UpdateOwnerNameFromDataBase()
        {
            var owners = TestData();
            LoadContext(owners);

            string newOwnerName = "UpdatedName";
            var ownerToUpdate = _ownerRepository.Get(owner => owner.Id == 1);
            ownerToUpdate.Name = newOwnerName;

            _ownerRepository.Update(ownerToUpdate);

            var updatedOwner = _ownerRepository.Get(owner => owner.Id == 1);

            Assert.AreEqual(newOwnerName, updatedOwner.Name);
        }

        [TestMethod]
        public void DeleteOwnerFromDataBase()
        {
            var ownerToDelete = new Owner { Id = 1, Name = "OwnerTest", LastName = "FirstLastname", Email = "test@test.com" };
            _ownerRepository.Insert(ownerToDelete);

            _ownerRepository.Delete(ownerToDelete);
            var owners = _ownerRepository.GetAll<Owner>().ToList();

            CollectionAssert.DoesNotContain(owners, ownerToDelete);
        }

        private void LoadContext(List<Owner> owners)
        {
            _context.Owners.AddRange(owners);
            _context.SaveChanges();
        }

        private List<Owner> TestData()
        {
            return new List<Owner>
        {
            new Owner { Id = 1, Name = "OwnerTest", LastName="FirstLastname", Email="test@test.com"},
            new Owner { Id = 2, Name = "SecondOwnerTest", LastName="SecondLastname", Email="test2@test.com"},
        };
        }
    }
}