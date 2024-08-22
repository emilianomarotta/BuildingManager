using Domain;
using IBusinessLogic;
using IBusinessLogic.Exceptions;
using Moq;
using DTOs.Out;
using DTOs.In;
using Microsoft.AspNetCore.Mvc;
using WebAPI;

namespace TestWebAPI;

[TestClass]
public class CategortyControllerTest
{
    private Mock<IBusinessLogic<Category>> _categoryLogic;
    private CategoryController _categoryController;
    private Category category;
    
    [TestInitialize]
    public void Setup()
    {
        _categoryLogic = new Mock<IBusinessLogic<Category>>();
        _categoryController = new CategoryController(_categoryLogic.Object);
        category = new Category
        {
            Id = 1,
            Name = "Category"
        };
    }
    
    [TestMethod]
    public void GetAllCategories()
    {
        List<Category> categories = new List<Category> { category };
        _categoryLogic.Setup(x => x.GetAll()).Returns(categories);
        var expectedCategories = categories.Select(c => new CategoryDetailModel(c)).ToList();
        
        var result = _categoryController.Index();
        var okResult = result as OkObjectResult;
        var actualCategories = okResult.Value as List<CategoryDetailModel>;
        
        _categoryLogic.VerifyAll();
        CollectionAssert.AreEqual(expectedCategories, actualCategories);
    }
    
    [TestMethod]
    public void GetOkTest()
    {
        List<Category> categories = new List<Category> { category };
        _categoryLogic.Setup(o => o.GetById(It.IsAny<int>())).Returns(categories.First());
        var expectedCategoryModel = new CategoryDetailModel(categories.First());
        
        var result = _categoryController.Show(categories.First().Id);
        var okResult = result as OkObjectResult;
        var actualCategoryModel = okResult.Value as CategoryDetailModel;
        
        _categoryLogic.VerifyAll();
        Assert.AreEqual(expectedCategoryModel, actualCategoryModel);
    }
    
    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public void GetNotFoundTest()
    {
        _categoryLogic.Setup(o => o.GetById(It.IsAny<int>())).Throws(new NotFoundException("Category not found"));
        var result = _categoryController.Show(1);

        _categoryLogic.VerifyAll();
    }
    
    [TestMethod]
    public void CreateCategory()
    {
        CategoryCreateModel categoryCreateModel = new CategoryCreateModel
        {
            Name = "Category"
        };
        _categoryLogic.Setup(o => o.Create(It.IsAny<Category>())).Returns(categoryCreateModel.ToEntity);
        var expectedCategoryModel = new CategoryDetailModel(categoryCreateModel.ToEntity());
        
        var result = _categoryController.Create(categoryCreateModel);
        var okResult = result as CreatedAtActionResult;
        var actualCategoryModel = okResult.Value as CategoryDetailModel;
        
        _categoryLogic.VerifyAll();
        Assert.AreEqual(expectedCategoryModel, actualCategoryModel);
    }
    
    [TestMethod]
    public void UpdateCategory()
    {
        int categoryId = 1;
        CategoryCreateModel categoryCreateModel = new CategoryCreateModel
        {
            Name = "TestCategory"
        };
        
        _categoryLogic.Setup(o => o.Update(It.IsAny<int>(), It.IsAny<Category>())).Returns(categoryCreateModel.ToEntity());
        
        var result = _categoryController.Update(categoryId, categoryCreateModel);
        var okResult = result as OkObjectResult;
        var actualCategoryModel = okResult.Value as CategoryDetailModel;
        var expectedCategoryModel = new CategoryDetailModel(categoryCreateModel.ToEntity());
        
        _categoryLogic.VerifyAll();
        Assert.AreEqual(expectedCategoryModel, actualCategoryModel);
    }
    
    [TestMethod]
    public void UpdateNotFoundCategory()
    {
        int categoryId = 1;
        CategoryCreateModel categoryCreateModel = new CategoryCreateModel
        {
            Name = "TestCategory"
        };
        
        _categoryLogic.Setup(o => o.Update(It.IsAny<int>(), It.IsAny<Category>())).Returns((Category)null);
        
        var result = _categoryController.Update(categoryId, categoryCreateModel);
        var notFoundResult = result as NotFoundObjectResult;
        
        _categoryLogic.VerifyAll();
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }
    
    [TestMethod]
    public void DeleteCategory()
    {
        int categoryId = 1;
        _categoryLogic.Setup(o => o.Delete(It.IsAny<int>())).Returns(true);
        
        var result = _categoryController.Delete(categoryId);
        var okResult = result as NoContentResult;
        
        _categoryLogic.VerifyAll();
        Assert.AreEqual(204, okResult.StatusCode);
    }
    
    [TestMethod]
    public void DeleteNotFoundCategory()
    {
        int categoryId = 1;
        _categoryLogic.Setup(o => o.Delete(It.IsAny<int>())).Returns(false);
        
        var result = _categoryController.Delete(categoryId);
        var notFoundResult = result as NotFoundObjectResult;
        
        _categoryLogic.VerifyAll();
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }
}