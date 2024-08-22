using Domain.Exceptions;
using System.Text.RegularExpressions;

namespace Domain
{
    public class Building
    {
        private const int MIN_NAME_LENGTH = 4;
        private string _name;
        private string _address;
        private string _location;
        private int _fees;
        public int CompanyId { get; set; }
        private Company _company;
        public int Id { get; set; }
        public int? ManagerId { get; set; }
        public Manager? Manager { get; set; }
        public ICollection<Apartment> Apartments { get; set; } = new List<Apartment>();

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (!ValidateOnlyLetters(value) || !ValidateLength(value))
                {
                    throw new InvalidAttributeException("Name should contain only letters and at least four characters");
                }
                _name = value;
            }
        }

        public string Address
        {
            get
            {
                return _address;
            }

            set
            {
                _address = value;
            }
        }

        public string Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }

        public int Fees
        {
            get { return _fees; }
            set
            {
                if (value < 1)
                {
                    throw new InvalidAttributeException("Fees must be a positive value");
                }
                _fees = value;
            }
        }

        public Company Company
        {
            get { return _company; }
            set
            {
                _company = value;
            }
        }

        private bool ValidateOnlyLetters(string name)
        {
            Regex onlyLettersRegex = new Regex("^[a-zA-Z ]+$");
            return onlyLettersRegex.IsMatch(name);
        }

        private bool ValidateLength(string name)
        {
            return name.Length >= MIN_NAME_LENGTH;
        }

        private bool AreEqualLocations(string location)
        {
            return this.Location == location;
        }

        private bool AreEqualIds(int id)
        {
            return this.Id == id;
        }

        private bool AreEqualAddresses(string address)
        {
            return this.Address == address;
        }

        private bool AreEqualNames(string name)
        {
            return this.Name == name;
        }

        public override bool Equals(object? obj)
        {
            Building building = (Building)obj;
            return AreEqualIds(building.Id) || AreEqualAddresses(building.Address) || AreEqualLocations(building.Location) || AreEqualNames(building.Name);
        }
    }
}