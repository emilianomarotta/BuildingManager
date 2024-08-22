using Domain;
using Domain.Exceptions;

namespace TestDomain;

[TestClass]
public class TestCategory
{
    Category category;

    [TestInitialize]
    public void Setup()
    {
        category = new Category
        {
            Id = 1,
            Name = "Test"
        };
    }

    [TestMethod]
    public void SetValidName()
    {
        string expectedName = "Test";
        Assert.AreEqual(expectedName, category.Name);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidAttributeException))]
    public void SetInvalidName()
    {
        category.Name = "Tes";
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidAttributeException))]
    public void SetInvalidNameWithNumbers()
    {
        category.Name = "Test123";
    }

    [TestMethod]
    public void EqualCategoriesById()
    {
        Category categoryTest = new Category
        {
            Id = 1
        };
        Assert.AreEqual(category, categoryTest);
    }

    [TestMethod]
    public void NotEqualCategoriesById()
    {
        Category categoryTest = new Category
        {
            Id = 2
        };
        Assert.AreNotEqual(category, categoryTest);
    }

    [TestMethod]
    public void AreEqualCategoriesByName()
    {
        Category categoryTest = new Category
        {
            Name = "Test",
        };
        Assert.AreEqual(category, categoryTest);
    }

    [TestMethod]
    public void AreNotEqualCategoriesByName()
    {
        Category categoryTest = new Category
        {
            Name = "test",
        };
        Assert.AreNotEqual(category, categoryTest);
    }

}