using Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Test
{
    [TestClass]
    public class SessionRepositoryTest
    {
        private SqliteConnection _connection;
        private BuildingManagerContext _context;
        private SessionRepository _sessionRepository;
        private AdministratorRepository administratorRepository;

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

            _sessionRepository = new SessionRepository(_context);
            administratorRepository = new AdministratorRepository(_context);
        }

        [TestMethod]
        public void GetAllSessions()
        {
            var expectedSessions = TestData();
            var admin = TestDataAdministrator();
            LoadContext(expectedSessions, admin);

            var retrievedSessions = _sessionRepository.GetAll<Session>();
            CollectionAssert.AreEqual(expectedSessions, retrievedSessions.ToList());
        }

        [TestMethod]
        public void GetAllSessionsByFilter()
        {
            var expectedSessions = TestData();
            var admin = TestDataAdministrator();
            LoadContext(expectedSessions, admin);

            var retrievedSessions = _sessionRepository.GetAll<Session>(session => session.Id < 10);
            CollectionAssert.AreEqual(expectedSessions, retrievedSessions.ToList());
        }

        [TestMethod]
        public void GetSessionById()
        {
            var expectedSessions = TestData();
            var admin = TestDataAdministrator();
            LoadContext(expectedSessions, admin);

            var expectedSession = new Session { Id = 1, Token = new Guid(), UserId = 1, Role = "Administrator" };
            var sessionFromDataBase = _sessionRepository.Get(session => session.Id == expectedSession.Id);

            Assert.AreEqual(expectedSession.Id, sessionFromDataBase.Id);
        }

        [TestMethod]
        public void InsertSessionToDataBase()
        {
            var admin = TestDataAdministrator();
            _context.Administrators.AddRange(admin);
            var newSession = new Session { Id = 1, Token = new Guid(), UserId = 1, Role = "Administrator" };
            List<Session> expectedSessions = new List<Session> { newSession };
            _sessionRepository.Insert(newSession);
            var retrievedSessions = _sessionRepository.GetAll<Session>();

            CollectionAssert.AreEqual(expectedSessions, retrievedSessions.ToList());
        }


        [TestMethod]
        public void DeleteSessionFromDataBase()
        {
            var admin = TestDataAdministrator();
            _context.Administrators.AddRange(admin);
            var sessionToDelete = new Session { Id = 1, Token = new Guid(), UserId = 1, Role = "Administrator" };
            _sessionRepository.Insert(sessionToDelete);

            _sessionRepository.Delete(sessionToDelete);
            var sessions = _sessionRepository.GetAll<Session>().ToList();

            CollectionAssert.DoesNotContain(sessions, sessionToDelete);
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
