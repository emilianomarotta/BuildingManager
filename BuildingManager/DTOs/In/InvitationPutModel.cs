using Domain;
using Domain.DataTypes;

namespace DTOs.In;

public class InvitationPutModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    public Status Status { get; set; }

    public Invitation ToEntity()
    {
        return new Invitation
        {
            Email = this.Email,
            Status = this.Status
        };
    }
}