using Domain;
using Domain.Exceptions;
using Task = Domain.Task;

namespace TestDomain;

[TestClass]
public class TestTask
{
    Task task;
    Category category;
    Staff staff;
    Apartment apartment;


    [TestInitialize]
    public void Setup()
    {
        category = new Category
        {
            Id = 1
        };

        staff = new Staff
        {
            Id = 1
        };

        apartment = new Apartment()
        {
            Id = 1
        };

        task = new Task
        {
            Id = 1,
            Category = category,
            Staff = staff,
            Apartment = apartment,
            Description = "Testing test",
            Cost = 1,
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(1)
        };
    }

    [TestMethod]
    public void SetValidCategory()
    {
        Category expectedCategory = category;
        Assert.AreEqual(expectedCategory, task.Category);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidAttributeException))]
    public void SetNullCategory()
    {
        task.Category = null;
    }

    [TestMethod]
    public void SetValidApartment()
    {
        Apartment expectedApartment = apartment;
        Assert.AreEqual(expectedApartment, task.Apartment);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidAttributeException))]
    public void SetNullApartment()
    {
        task.Apartment = null;
    }

    [TestMethod]
    public void SetValidStartDate()
    {
        DateTime expectedDate = DateTime.Today;
        Assert.AreEqual(expectedDate, task.StartDate);
    }

    [TestMethod]
    public void SetValidEndDate()
    {
        DateTime expectedDate = DateTime.Today.AddDays(1);
        Assert.AreEqual(expectedDate, task.EndDate);
    }

    [TestMethod]
    public void SetValidCost()
    {
        double expectedCost = 1;
        Assert.AreEqual(expectedCost, task.Cost);
    }

    [TestMethod]
    public void SetValidDescription()
    {
        string expectedDescription = "Testing test";
        Assert.AreEqual(expectedDescription, task.Description);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidAttributeException))]
    public void SetInvalidDescription()
    {
        task.Description = "";
    }

    [TestMethod]
    public void EqualTasksById()
    {
        Task testTask = new Task
        {
            Id = 1
        };
        Assert.AreEqual(testTask, task);
    }

    [TestMethod]
    public void NotEqualTasksById()
    {
        Task testTask = new Task
        {
            Id = 2
        };
        Assert.AreNotEqual(testTask, task);
    }
}