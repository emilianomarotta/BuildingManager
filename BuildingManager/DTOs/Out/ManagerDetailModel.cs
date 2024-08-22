using Domain;

namespace DTOs.Out;

public class ManagerDetailModel
{
    public ManagerDetailModel(Manager manager)
    {
        Id = manager.Id;
        Email = manager.Email;
        Name = manager.Name;
        LastName = manager.LastName;
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    public override bool Equals(object? obj)
    {
        var otherManagerDetail = obj as ManagerDetailModel;
        return Id == otherManagerDetail.Id;
    }
    
}