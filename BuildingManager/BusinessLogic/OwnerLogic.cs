using IBusinessLogic.Exceptions;
using Domain;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class OwnerLogic : IBusinessLogic<Owner>
{
    private readonly IGenericRepository<Owner> _repository;

    public OwnerLogic(IGenericRepository<Owner> repository)
    {
        _repository = repository;
    }
    
    public List<Owner> GetAll()
    {
        return _repository.GetAll<Owner>().ToList();
    }

    public Owner GetById(int id)
    {
        return _repository.Get(owner => owner.Id == id);
    }

    public Owner Create(Owner owner)
    {
        if (EmailExists(owner.Email))
        {
            throw new AlreadyExistsException("Owner already exists");
        }
        _repository.Insert(owner);
        return owner;
    }

    public Owner Update(int id, Owner updatedOwner)
    {
        var owner = _repository.Get(owner => owner.Id == id);
        if (owner == null)
        {
            throw new NotFoundException("Owner not found");
        }
        if (owner.Email != updatedOwner.Email)
        {
            if (EmailExists(updatedOwner.Email))
            {
                throw new AlreadyExistsException("Email already being used");
            }
            owner.Email = updatedOwner.Email;
        }
        owner.Name = updatedOwner.Name;
        owner.LastName = updatedOwner.LastName;
        _repository.Update(owner);
        return owner;
    }

    public bool Delete(int id)
    {
        var owner = _repository.Get(owner => owner.Id == id);
        if (owner == null)
        {
            return false;
        }
        _repository.Delete(owner);
        return true;
    }

    private bool EmailExists(string email)
    {
        List<Owner> existingOwners = _repository.GetAll<Owner>().ToList();
        var owner = existingOwners.FirstOrDefault(owner => owner.Email == email);
        return owner != null;
    }
}