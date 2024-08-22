using Domain;

namespace DTOs.In;

public class ManagerCreateModel
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public Manager ToEntity()
    {
        return new Manager
        {
            Name = this.Name,
            LastName = "ChangeField",
            Email = this.Email,
            Password = this.Password
        };
    }
}