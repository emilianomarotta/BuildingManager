using Domain;

namespace DTOs.In;

public class ApartmentPutModel
{
    public int OwnerId { get; set; }

    public Apartment ToEntity()
    {
        return new Apartment()
        {
            OwnerId = this.OwnerId,
        };
    }
}