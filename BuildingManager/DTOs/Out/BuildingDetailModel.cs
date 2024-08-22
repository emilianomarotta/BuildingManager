using Domain;
namespace DTOs.Out;

public class BuildingDetailModel
{
   
    public BuildingDetailModel(Building building)
    {
        Id = building.Id;
        Name = building.Name;
        ManagerId = building.ManagerId;
        Manager = building.Manager;
        Address = building.Address;
        Location = building.Location;
        Fees = building.Fees;
        CompanyId = building.CompanyId;
        Company = building.Company;
        Apartments = building.Apartments;
    }
    
    public ICollection<Apartment> Apartments { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public int? ManagerId { get; set; }
    public Manager? Manager { get; set; }
    public string Address { get; set; }
    public string Location { get; set; }
    public int Fees { get; set; }
    public int CompanyId { get; set; }
    public Company Company { get; set; }
    
    public override bool Equals(object? obj)
    {
        var otherBuildingDetail = obj as BuildingDetailModel;
        return Name == otherBuildingDetail.Name;
    }
}