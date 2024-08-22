using Domain;

namespace DTOs.Out
{
    public class AdministratorDetailModel
    {
        public AdministratorDetailModel(Administrator administrator)
        {
            Id = administrator.Id;
            Email = administrator.Email;
            Name = administrator.Name;
            LastName = administrator.LastName;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public override bool Equals(object? obj)
        {
            var otherAdministratorDetail = obj as AdministratorDetailModel;
            return Id == otherAdministratorDetail.Id;
        }
    }
}
