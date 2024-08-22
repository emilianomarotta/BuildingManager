using Domain;
using Domain.DataTypes;
using System;

namespace DTOs.Out
{
    public class InvitationDetailModel
    {
        public InvitationDetailModel(Invitation invitation)
        {
            Id = invitation.Id;
            Name = invitation.Name;
            Email = invitation.Email;
            Expiration = invitation.Expiration;
            Role = invitation.Role;
            Status = invitation.Status;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public DateTime Expiration { get; set; }
        public Status Status { get; set; }

        public override bool Equals(object? obj)
        {
            var otherInvitationDetail = obj as InvitationDetailModel;
            return Id == otherInvitationDetail.Id;
        }
    }
}
