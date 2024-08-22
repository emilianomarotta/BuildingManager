using Domain;

namespace DTOs.In
{
    public class StaffCreateModel
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public Staff ToEntity()
        {
            return new Staff
            {
                Name = this.Name,
                LastName = this.LastName,
                Email = this.Email,
                Password = this.Password,
            };
        }
    }
}
