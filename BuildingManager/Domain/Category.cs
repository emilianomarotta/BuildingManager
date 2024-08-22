using Domain.Exceptions;
using System.Text.RegularExpressions;

namespace Domain;

public class Category
{
    private const int MIN_NAME_LENGTH = 4;
    public int Id { get; set; }
    private string _name;

    public string Name
    {
        get { return _name; }
        set
        {
            if (!ValidateLength(value) || !ValidateOnlyLetters(value))
            {
                throw new InvalidAttributeException("Name should contain only letters and at least four characters");
            }
            _name = value;
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

    private bool AreEqualIds(int id)
    {
        return this.Id == id;
    }

    private bool AreEqualNames(string name)
    {
        return this.Name == name;
    }

    public override bool Equals(object? obj)
    {
        Category category = (Category)obj;
        return AreEqualIds(category.Id) || AreEqualNames(category.Name);
    }
}