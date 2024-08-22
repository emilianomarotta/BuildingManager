using Domain;

namespace DTOs.In;

public class ApartmentCreateModel
{
    public int Floor { get; set; }
    public int Number { get; set; }
    public int BuildingId { get; set; }
    public int OwnerId { get; set; }
    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public bool Balcony { get; set; }

    public Apartment ToEntity()
    {
        return new Apartment()
        {
            Floor = this.Floor,
            Number = this.Number,
            BuildingId = this.BuildingId,
            OwnerId = this.OwnerId,
            Bedrooms = this.Bedrooms,
            Bathrooms = this.Bathrooms,
            Balcony = this.Balcony
        };
    }
}