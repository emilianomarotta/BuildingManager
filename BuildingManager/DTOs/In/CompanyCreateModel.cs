using Domain;

namespace DTOs.In
{
    public class CompanyCreateModel
    {
        public string Name { get; set; }

        public Company ToEntity()
        {
            return new Company
            {
                Name = this.Name
            };
        }
    }
}
