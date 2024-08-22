using Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Test
{
    [TestClass]
    public class UserRepositoryTest
    {
        private SqliteConnection _connection;
        private BuildingManagerContext _context;
        private UserRepository _userRepository;

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

            _userRepository = new UserRepository(_context);
            _userRepository = new UserRepository(_context);
        }

        [TestMethod]
        public void FinBytoken()
        {
            var sessions = TestData();
            var admin = TestDataAdministrator();
            LoadContext(sessions, admin);

            var expectedUser = new Administrator { Id = 1, Name = "admin", LastName = "lastname", Email = "admin@admin.com", Password = "Test.1234" };

            User? userFromDataBase = _userRepository.FindByToken(sessions.First().Token);

            Assert.AreEqual(expectedUser.Id, userFromDataBase.Id);
        }
        [TestMethod]
        public void FinBytokenNotFound()
        {
            var sessions = new List<Session>();
            var admin = TestDataAdministrator();
            LoadContext(sessions, admin);
            User? userFromDataBase = _userRepository.FindByToken(new Guid());

            Assert.IsNull(userFromDataBase);
        }

        [TestMethod]
        public void FindByEmailAndPassword()
        {
            var sessions = TestData();
            var admin = TestDataAdministrator();
            LoadContext(sessions, admin);

            var expectedUser = new Administrator { Id = 1, Name = "admin", LastName = "lastname", Email = "admin@admin.com", Password = "Test.1234" };
            string email = "admin@admin.com";
            string password = "Test.1234";
            User? userFromDataBase = _userRepository.FindByEmailAndPassword(email, password);

            Assert.AreEqual(expectedUser.Id, userFromDataBase.Id);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
        }
        private void LoadContext(List<Session> session, List<Administrator> adminsitrator)
        {
            _context.Administrators.AddRange(adminsitrator);
            _context.Sessions.AddRange(session);
            _context.SaveChanges();
        }

        private List<Session> TestData()
        {
            return new List<Session>
        {
            new Session { Id = 1, Token = new Guid(), UserId=1, Role = "Administrator"},
            new Session { Id = 2, Token = new Guid(), UserId=2, Role = "Administrator"},
        };
        }
        private List<Administrator> TestDataAdministrator()
        {
            return new List<Administrator>
        {
            new Administrator { Id = 1, Name = "admin", LastName="lastname", Email="admin@admin.com", Password="Test.1234"},
            new Administrator { Id = 2, Name = "admin", LastName="lastname", Email="admin2@admin.com", Password="Test.1234"},
        };
        }
    }

}
