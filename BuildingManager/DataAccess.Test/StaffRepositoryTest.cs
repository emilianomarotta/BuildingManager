using Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Test
{
    [TestClass]
    public class StaffRepositoryTest
    {
        private SqliteConnection _connection;
        private BuildingManagerContext _context;
        private StaffRepository _staffRepository;

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
            _staffRepository = new StaffRepository(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
        }

        [TestMethod]
        public void GetAllStaffs()
        {
            var expectedStaffs = TestData();
            LoadContext(expectedStaffs);

            var retrievedStaffs = _staffRepository.GetAll<Staff>();
            CollectionAssert.AreEqual(expectedStaffs, retrievedStaffs.ToList());
        }

        [TestMethod]
        public void GetAllStaffsByFilter()
        {
            var expectedStaffs = TestData();
            LoadContext(expectedStaffs);

            var retrievedStaffs = _staffRepository.GetAll<Staff>(staff => staff.Id < 10);
            CollectionAssert.AreEqual(expectedStaffs, retrievedStaffs.ToList());
        }

        [TestMethod]
        public void GetStaffById()
        {
            var staffs = TestData();
            LoadContext(staffs);

            var expectedStaff = new Staff { Id = 1, Name = "StaffTest", LastName = "FirstLastname", Email = "test@test.com", Password = "Test.1234" };
            var staffFromDataBase = _staffRepository.Get(staff => staff.Id == expectedStaff.Id);

            Assert.AreEqual(expectedStaff, staffFromDataBase);
        }

        [TestMethod]
        public void InsertStaffToDataBase()
        {
            var newStaff = new Staff { Id = 1, Name = "StaffTest", LastName = "FirstLastname", Email = "test@test.com", Password = "Test.1234"};
            List< Staff> expectedStaffs = new List<Staff> { newStaff };
            _staffRepository.Insert(newStaff);
            var retrievedStaffs = _staffRepository.GetAll<Staff>();

            CollectionAssert.AreEqual(expectedStaffs, retrievedStaffs.ToList());
        }

        [TestMethod]
        public void UpdateStaffNameFromDataBase()
        {
            var staffs = TestData();
            LoadContext(staffs);

            string newStaffName = "UpdatedName";
            var staffToUpdate = _staffRepository.Get(staff => staff.Id == 1);
            staffToUpdate.Name = newStaffName;

            _staffRepository.Update(staffToUpdate);

            var updatedStaff = _staffRepository.Get(staff => staff.Id == 1);

            Assert.AreEqual(newStaffName, updatedStaff.Name);
        }

        [TestMethod]
        public void DeleteStaffFromDataBase()
        {
            var staffToDelete = new Staff { Id = 1, Name = "StaffTest", LastName = "FirstLastname", Email = "test@test.com", Password = "Test.1234" };
            _staffRepository.Insert(staffToDelete);

            _staffRepository.Delete(staffToDelete);
            var staffs = _staffRepository.GetAll<Staff>().ToList();

            CollectionAssert.DoesNotContain(staffs, staffToDelete);
        }

        private void LoadContext(List<Staff> staffs)
        {
            _context.Staffs.AddRange(staffs);
            _context.SaveChanges();
        }

        private List<Staff> TestData()
        {
            return new List<Staff>
        {
            new Staff { Id = 1, Name = "StaffTest", LastName="FirstLastname", Email="test@test.com", Password="Test.1234"},
            new Staff { Id = 2, Name = "SecondStaffTest", LastName="SecondLastname", Email="test2@test.com", Password="Test.1234"},
        };
        }
    }
}