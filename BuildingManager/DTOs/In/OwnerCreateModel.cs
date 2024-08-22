using Domain;
namespace DTOs.In
{
    public class OwnerCreateModel
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public Owner ToEntity()
        {
            return new Owner
            {
                Name = this.Name,
                LastName = this.LastName,
                Email = this.Email,
            };
        }
    }
}
