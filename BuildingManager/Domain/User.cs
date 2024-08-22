using Domain.Exceptions;
using System.Text.RegularExpressions;

namespace Domain
{
    public abstract class User

    {
        private const int MIN_NAME_LENGTH = 4;
        public int Id { get; set; }
        private string _name;
        private string _lastName;
        private string _email;
        private string _password;
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
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                if (!ValidateOnlyLetters(value) || !ValidateLength(value))
                {
                    throw new InvalidAttributeException("Last name should contain only letters and at least four characters");
                }
                _lastName = value;
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
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (!ValidatePassword(value))
                {
                    throw new InvalidAttributeException(("Password must be at least 8 characters long, have one capital letter, one symbol and one number"));
                }
                _password = value;
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
        private bool ValidatePassword(string password)
        {
            Regex validPassword = new Regex(@"^(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$");
            return validPassword.IsMatch(password);
        }

        private bool AreEqualIds(int id)
        {
            return this.Id == id;
        }
        private bool AreEqualEmails(string email)
        {
            return this.Email == email;
        }
        public override bool Equals(object? obj)
        {
            User user = (User)obj;
            return AreEqualIds(user.Id) || AreEqualEmails(user.Email);
        }
    }
}
