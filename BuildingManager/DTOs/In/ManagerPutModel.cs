using Domain;

namespace DTOs.In;

public class ManagerPutModel
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }

    public Manager ToEntity()
    {
        return new Manager
        {
            Name = this.Name,
            LastName = this.LastName,
            Email = this.Email,
            Password = this.Password
        };
    }
}