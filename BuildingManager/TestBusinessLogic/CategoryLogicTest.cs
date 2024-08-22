using Domain;
using BusinessLogic;
using IBusinessLogic.Exceptions;
using IDataAccess;
using Moq;

namespace TestBusinessLogic;

[TestClass]
public class CategoryLogicTest
{
    private Mock<IGenericRepository<Category>> _categoryRepository;
    private CategoryLogic _categoryLogic;
    private Category category;


    [TestInitialize]
    public void Setup()
    {
        _categoryRepository = new Mock<IGenericRepository<Category>>(MockBehavior.Strict);
        _categoryLogic = new CategoryLogic(_categoryRepository.Object);
        category = new Category()
        {
            Id = 1,
            Name = "Category",
        };

    }

    [TestMethod]
    public void CreateCategoryTest()
    {
        List<Category> categories = new List<Category> { };
        _categoryRepository.Setup(c => c.GetAll<Category>()).Returns(categories);
        _categoryRepository.Setup(c => c.Insert(category));
        var insertedCategory = _categoryLogic.Create(category);

        _categoryRepository.VerifyAll();
        Assert.AreEqual(category, insertedCategory);
    }

    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void CreateAlreadyExistingCategory()
    {
        List<Category> categories = new List<Category> { category };
        _categoryRepository.Setup(c => c.GetAll<Category>()).Returns(categories);
        var otherCategory = new Category()
        {
            Id = 2,
            Name = "Category",
        };
        var insertedCategory = _categoryLogic.Create(otherCategory);

        _categoryRepository.VerifyAll();
    }

    [TestMethod]
    public void GetAllCompanies()
    {
        List<Category> categories = new List<Category> { category };
        _categoryRepository.Setup(c => c.GetAll<Category>()).Returns(categories);
        List<Category> allCompanies = _categoryLogic.GetAll();

        _categoryRepository.VerifyAll();
        CollectionAssert.AreEqual(categories, allCompanies);
    }

    [TestMethod]
    public void GetCategoryById()
    {
        int categoryId = 1;
        _categoryRepository.Setup(c => c.Get(category => category.Id == categoryId, null)).Returns(category);
        var categoryById = _categoryLogic.GetById(categoryId);

        _categoryRepository.VerifyAll();
        Assert.AreEqual(category, categoryById);
    }

    [TestMethod]
    public void GetCategoryByIdNotFound()
    {
        int categoryId = 1;
        _categoryRepository.Setup(c => c.Get(category => category.Id == categoryId, null)).Returns((Category)null);
        var categoryById = _categoryLogic.GetById(categoryId);

        _categoryRepository.VerifyAll();
        Assert.IsNull(categoryById);
    }

    [TestMethod]
    public void UpdateCategory()
    {
        int categoryId = 1;
        var updatedCategory = new Category
        {
            Id = 1,
            Name = "Updated Category",
        };
        List<Category> categories = new List<Category> {  };
        _categoryRepository.Setup(c => c.GetAll<Category>()).Returns(categories);
        _categoryRepository.Setup(c => c.Get(category => category.Id == categoryId, null)).Returns(category);
        _categoryRepository.Setup(c => c.Update(category));
        var categoryById = _categoryLogic.Update(categoryId, updatedCategory);

        _categoryRepository.VerifyAll();
        Assert.AreEqual(updatedCategory, categoryById);
    }
    [TestMethod]
    [ExpectedException(typeof(AlreadyExistsException))]
    public void UpdateAlreadyExistingCategory()
    {
        int categoryId = 1;
        var updatedCategory = new Category()
        {
            Id = 1,
            Name = "Category",
        };
        List<Category> categories = new List<Category> { category };
        _categoryRepository.Setup(c => c.GetAll<Category>()).Returns(categories);
        _categoryRepository.Setup(c => c.Update(category));
        var categoryById = _categoryLogic.Update(categoryId, updatedCategory);

        _categoryRepository.VerifyAll();
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public void UpdateNotFoundCategory()
    {
        int categoryId = 1;
        var updatedCategory = new Category()
        {
            Id = 2,
            Name = "Updated Category",
        };
        List<Category> categories = new List<Category> { category };
        _categoryRepository.Setup(c => c.GetAll<Category>()).Returns(categories);
        _categoryRepository.Setup(c => c.Get(category => category.Id == categoryId, null)).Returns((Category)null);
        _categoryRepository.Setup(c => c.Update(updatedCategory));
        var categoryById = _categoryLogic.Update(categoryId, updatedCategory);

        _categoryRepository.VerifyAll();
    }

    [TestMethod]
    public void DeleteCategory()
    {
        int categoryId = 1;
        List<Category> categories = new List<Category> { category };
        _categoryRepository.Setup(c => c.Get(category => category.Id == categoryId, null)).Returns(category);
        _categoryRepository.Setup(c => c.Delete(category));
        var categoryById = _categoryLogic.Delete(categoryId);

        _categoryRepository.VerifyAll();
        Assert.IsTrue(categoryById);
    }

    [TestMethod]
    public void DeleteNotFoundCategory()
    {
        int categoryId = 1;
        List<Category> categories = new List<Category> { };
        _categoryRepository.Setup(c => c.Get(category => category.Id == categoryId, null)).Returns((Category)null);
        var categoryById = _categoryLogic.Delete(categoryId);

        _categoryRepository.VerifyAll();
        Assert.IsFalse(categoryById);
    }
}

