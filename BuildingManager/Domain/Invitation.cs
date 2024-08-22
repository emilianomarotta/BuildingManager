using Domain.DataTypes;
using Domain.Exceptions;
using System.Text.RegularExpressions;

namespace Domain
{
    public class Invitation
    {
        public Invitation()
        {
            Status = Status.Pending;
        }

        private const int MIN_NAME_LENGTH = 4;
        public int Id { get; set; }
        private string _name { get; set; }
        private string _email { get; set; }
        private DateTime _expiration { get; set; }
        public Status Status { get; set; }
        public Role Role { get; set; }

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
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                if (!ValidateEmail(value))
                {
                    throw new InvalidAttributeException("Wrong email format: \"example@example.com\"");
                }
                _email = value;
            }
        }
        public DateTime Expiration
        {
            get
            {
                return _expiration;
            }
            set
            {
                _expiration = value;
            }
        }

        private bool ValidateOnlyLetters(string name)
        {
            Regex onlyLetterRegex = new Regex("^[a-zA-Z ]+$");
            return onlyLetterRegex.IsMatch(name);
        }

        private bool ValidateLength(string name)
        {
            return name.Length >= MIN_NAME_LENGTH;
        }

        private bool ValidateEmail(string email)
        {
            Regex validEmail = new Regex(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+.[A-Za-z]{2,}$");
            return validEmail.IsMatch(email);
        }

        public override bool Equals(object? obj)
        {
            Invitation invitation = (Invitation)obj;
            return this.Id == invitation.Id;
        }
    }
}
