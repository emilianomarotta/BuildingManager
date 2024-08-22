using Domain;
using Domain.DataTypes;

namespace DTOs.In
{
    public class InvitationCreateModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime Expiration { get; set; }
        public Role Role { get; set; }

        public Invitation ToEntity()
        {
            return new Invitation
            {
                Name = this.Name,
                Email = this.Email,
                Role = this.Role,
                Expiration = this.Expiration
            };
        }
    }
}
