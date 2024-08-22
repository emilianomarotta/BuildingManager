using Domain;

namespace DTOs.In;

public class CompanyAdminCreateModel
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public CompanyAdmin ToEntity()
    {
        return new CompanyAdmin
        {
            Name = this.Name,
            LastName = "ChangeField",
            Email = this.Email,
            Password = this.Password
        };
    }
}