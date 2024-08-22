using Domain.Exceptions;

namespace Domain;

public class Task
{
    public int Id { get; set; }
    public int? StaffId { get; set; }
    public Staff? Staff { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.Today;
    public int CategoryId { get; set; }
    private Category _category;
    public int ApartmentId { get; set; }
    public Apartment _apartment;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public double Cost { get; set; } = 0;
    private string _description;

    public Category Category
    {
        get { return _category; }
        set
        {
            if (value == null)
            {
                throw new InvalidAttributeException("Category id required");
            }

            _category = value;
        }
    }

    public Apartment Apartment
    {
        get { return _apartment; }
        set
        {
            if (value == null)
            {
                throw new InvalidAttributeException("Apartment id required");
            }

            _apartment = value;
        }
    }

    public string Description
    {
        get { return _description; }
        set
        {
            if (value == "")
            {
                throw new InvalidAttributeException("Description cannot be empty");
            }

            _description = value;
        }
    }

    public override bool Equals(object? obj)
    {
        Task task = (Task)obj;
        return this.Id == task.Id;
    }
}