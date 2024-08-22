using Domain.Exceptions;

namespace Domain
{
    public class Apartment
    {
        private int _number;
        private int _bedrooms;
        private int _bathrooms;
        public int Floor { get; set; }
        public bool Balcony { get; set; }
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public Building Building { get; set; }
        public int OwnerId { get; set; }
        public Owner Owner { get; set; }

        public int Number
        {
            get { return _number; }
            set
            {
                if (value < 1)
                {
                    throw new InvalidAttributeException("Apartment number must be 1 or higher");
                }
                _number = value;
            }
        }

        public int Bedrooms
        {
            get { return _bedrooms; }
            set
            {
                if (value < 0)
                {
                    throw new InvalidAttributeException("Apartment bedrooms amount must be a positive value");
                }
                _bedrooms = value;
            }
        }

        public int Bathrooms
        {
            get { return _bathrooms; }
            set
            {
                if (value < 1)
                {
                    throw new InvalidAttributeException("Apartment bathrooms amount must be 1 or higher");
                }
                _bathrooms = value;
            }
        }

        private bool AreEqualFloors(int floor)
        {
            return this.Floor == floor;
        }

        private bool AreEqualNumbers(int number)
        {
            return this.Number == number;
        }

        private bool AreEqualBuildings(Building building)
        {
            return this.Building == building;
        }

        private bool AreEqualApartments(Apartment apartment)
        {
            return AreEqualBuildings(apartment.Building) && AreEqualFloors(apartment.Floor) && AreEqualNumbers(apartment.Number);
        }

        public override bool Equals(object? obj)
        {
            Apartment apartment = (Apartment)obj;
            return this.Id == apartment.Id || AreEqualApartments(apartment);
        }
    }
}
