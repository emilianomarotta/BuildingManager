using Domain;
using Domain.DataTypes;
using Domain.Exceptions;

namespace TestDomain
{
    [TestClass]
    public class TestInvitation
    {
        Invitation invitation;

        [TestInitialize]
        public void Setup()
        {
            invitation = new Invitation
            {
                Id = 1,
                Name = "Test",
                Email = "test@test.com",
                Expiration = DateTime.Today.AddDays(1),

            };
        }

        [TestMethod]
        public void SetValidName()
        {
            string expectedName = "Test";
            Assert.AreEqual(expectedName, invitation.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidName()
        {
            invitation.Name = "A";
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidNameWithNumbers()
        {
            invitation.Name = "Test123";
        }

        [TestMethod]
        public void SetValidEmail()
        {
            string expectedEmail = "test@test.com";
            Assert.AreEqual(expectedEmail, invitation.Email);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAttributeException))]
        public void SetInvalidEmail()
        {
            invitation.Email = "test";
        }

        [TestMethod]
        public void SetValidExpiration()
        {
            DateTime expectedExpiration = DateTime.Today.AddDays(1);
            Assert.AreEqual(expectedExpiration, invitation.Expiration);
        }

        [TestMethod]
        public void SetValidStatus()
        {
            Status expectedStatus = Status.Pending;
            Assert.AreEqual(expectedStatus, invitation.Status);
        }
        [TestMethod]
        public void SetValidRole()
        {
            invitation.Role = Role.Manager;
            Role expectedRole = Role.Manager;
            Assert.AreEqual(expectedRole, invitation.Role);
        }

        [TestMethod]
        public void EqualsInvitationById()
        {
            Invitation invitation2 = new Invitation
            {
                Id = 1
            };
            Assert.AreEqual(invitation, invitation2);
        }

        [TestMethod]
        public void NotEqualsInvitationById()
        {
            Invitation invitation2 = new Invitation
            {
                Id = 2
            };
            Assert.AreNotEqual(invitation, invitation2);
        }
    }
}
