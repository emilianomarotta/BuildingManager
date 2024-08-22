using Domain;

namespace DTOs.Out
{
    public class SessionDetailModel
    {
        public SessionDetailModel(Session session)
        {
            Id = session.Id;
            UserId = session.UserId;
            Token = session.Token;
            Role = session.Role;
            Email = session.User.Email;
        }
        public int Id { get; set; }
        public int UserId { get; set; }
        public Guid Token { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }

        public override bool Equals(object? obj)
        {
            var otherSessionDetailModel = obj as SessionDetailModel;
            return Id == otherSessionDetailModel.Id;
        }
    }
}
