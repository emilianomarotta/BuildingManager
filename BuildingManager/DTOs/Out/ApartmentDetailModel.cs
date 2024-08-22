using Domain;

namespace DTOs.Out;

public class ApartmentDetailModel
{
    public ApartmentDetailModel(Apartment apartment)
    {
        Id = apartment.Id;
        Floor = apartment.Floor;
        Number = apartment.Number;
        BuildingId = apartment.BuildingId;
        OwnerId = apartment.OwnerId;
        Bedrooms = apartment.Bedrooms;
        Bathrooms = apartment.Bathrooms;
        Balcony = apartment.Balcony;
    }
    public int Id { get; set; }
    public int Floor { get; set; }
    public int Number { get; set; }
    public int BuildingId { get; set; }
    public int OwnerId { get; set; }
    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public bool Balcony { get; set; }

    public override bool Equals(object? obj)
    {
        var otherApartmentDetail = obj as ApartmentDetailModel;
        return Id == otherApartmentDetail.Id;
    }
}