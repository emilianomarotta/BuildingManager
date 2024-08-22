using Domain;

namespace DTOs.In
{
    public class AdministratorCreateModel
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public Administrator ToEntity()
        {
            return new Administrator
            {
                Name = this.Name,
                LastName = this.LastName,
                Email = this.Email,
                Password = this.Password
            };
        }
    }
}
