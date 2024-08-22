using Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Domain.DataTypes;

namespace DataAccess.Test
{
    [TestClass]
    public class InvitationRepositoryTest
    {
        private SqliteConnection _connection;
        private BuildingManagerContext _context;
        private InvitationRepository _invitationRepository;

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

            _invitationRepository = new InvitationRepository(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
        }

        [TestMethod]
        public void GetAllInvitations()
        {
            var expectedInvitations = TestData();
            LoadContext(expectedInvitations);

            var retrievedInvitations = _invitationRepository.GetAll<Invitation>();
            CollectionAssert.AreEqual(expectedInvitations, retrievedInvitations.ToList());
        }

        [TestMethod]
        public void GetInvitationById()
        {
            var invitations = TestData();
            LoadContext(invitations);

            var expectedInvitation = new Invitation { Id = 1, Name = "FirstInvitationTest", Email = "test@test.com", Expiration = DateTime.Today.AddDays(5) };
            var invitationFromDatabase = _invitationRepository.Get(invitation => invitation.Id == expectedInvitation.Id);

            Assert.AreEqual(expectedInvitation, invitationFromDatabase);
        }

        [TestMethod]
        public void GetAllInvitationsByFilter()
        {
            var expectedInvitations = TestData();
            LoadContext(expectedInvitations);

            var retrievedInvitations = _invitationRepository.GetAll<Invitation>(invitation => invitation.Id < 10);
            CollectionAssert.AreEqual(expectedInvitations, retrievedInvitations.ToList());
        }

        [TestMethod]
        public void InsertInvitationToDataBase()
        {
            var newInvitation = new Invitation { Id = 1, Name = "FirstInvitationTest", Email = "test@test.com", Expiration = DateTime.Today.AddDays(5) };
            List<Invitation> expectedInvitations = new List<Invitation> { newInvitation };
            _invitationRepository.Insert(newInvitation);
            var retrievedInvitations = _invitationRepository.GetAll<Invitation>();

            CollectionAssert.AreEqual(expectedInvitations, retrievedInvitations.ToList());
        }

        [TestMethod]
        public void UpdateInvitationStatusFromDataBase()
        {
            var invitations = TestData();
            LoadContext(invitations);
            var invitationToUpdate = _invitationRepository.Get(invitation => invitation.Id == 1);
            invitationToUpdate.Status = Status.Accepted;
            _invitationRepository.Update(invitationToUpdate);

            var updatedInvitation = _invitationRepository.Get(invitation => invitation.Id == 1);

            Assert.AreEqual(invitationToUpdate.Status, updatedInvitation.Status);
        }

        [TestMethod]
        public void DeleteInvitationFromDataBase()
        {
            var invitationToDelete = new Invitation { Id = 1, Name = "FirstInvitationTest", Email = "test@test.com", Expiration = DateTime.Today.AddDays(5) };
            _invitationRepository.Insert(invitationToDelete);

            _invitationRepository.Delete(invitationToDelete);
            var invitations = _invitationRepository.GetAll<Invitation>().ToList();

            CollectionAssert.DoesNotContain(invitations, invitationToDelete);
        }

        private void LoadContext(List<Invitation> invitations)
        {
            _context.Invitations.AddRange(invitations);
            _context.SaveChanges();
        }

        private List<Invitation> TestData()
        {
            return new List<Invitation>
        {
            new Invitation { Id = 1, Name = "FirstInvitationTest", Email="test@test.com", Expiration=DateTime.Today.AddDays(5) },
            new Invitation { Id = 2, Name = "SecondInvitationTest", Email="test2@test.com",Expiration=DateTime.Today.AddDays(6) }
        };
        }
    }
}
