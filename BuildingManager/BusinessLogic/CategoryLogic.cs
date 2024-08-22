using IBusinessLogic.Exceptions;
using Domain;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class CategoryLogic : IBusinessLogic<Category>
{
    private readonly IGenericRepository<Category> _repository;

    public CategoryLogic(IGenericRepository<Category> repository)
    {
        _repository = repository;
    }
    
    public List<Category> GetAll()
    {
        return _repository.GetAll<Category>().ToList();
    }

    public Category GetById(int id)
    {
        return _repository.Get(category => category.Id == id);
    }

    public Category Create(Category category)
    {
        if (NameExists(category.Name))
        {
            throw new AlreadyExistsException("Category already exists");
        }
        _repository.Insert(category);
        return category;
    }

    public Category Update(int id, Category updatedCategory)
    {
        if (NameExists(updatedCategory.Name))
        {
            throw new AlreadyExistsException("Name already being used");
        }
        var category = _repository.Get(category => category.Id == id);
        if (category == null)
        {
            throw new NotFoundException("Category not found");
        }
        category.Name = updatedCategory.Name;
        _repository.Update(category);
        return category;
    }

    public bool Delete(int id)
    {
        var category = _repository.Get(category => category.Id == id);
        if (category == null)
        {
            return false;
        }
        _repository.Delete(category);
        return true;
    }

    private bool NameExists(string name)
    {
        List<Category> existingCategories = _repository.GetAll<Category>().ToList();
        var category = existingCategories.FirstOrDefault(category => category.Name == name);
        return category != null;
    }
}