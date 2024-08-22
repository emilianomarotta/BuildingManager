using IBusinessLogic.Exceptions;
using Domain;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class CompanyLogic : IBusinessLogic<Company>
{
    private readonly IGenericRepository<Company> _repository;
    private readonly ISessionLogic _sessionLogic;
    public CompanyLogic(IGenericRepository<Company> repository, ISessionLogic sessionLogic)
    {
        _repository = repository;
        _sessionLogic = sessionLogic;
    }

    public List<Company> GetAll()
    {
        var currentUser = _sessionLogic.GetCurrentUser();
        int currentUserId = currentUser.Id;
        return _repository.GetAll<Company>().Where(c => c.CompanyAdminId == currentUserId).ToList();
    }

    public Company GetById(int id)
    {
        var currentUser = _sessionLogic.GetCurrentUser();
        int currentUserId = currentUser.Id;
        return _repository.Get(company => company.Id == id && company.CompanyAdminId == currentUserId);
    }

    public Company Create(Company company)
    {
        if (NameExists(company.Name))
        {
            throw new AlreadyExistsException("Company already exists");
        }
        var currentUser = _sessionLogic.GetCurrentUser();
        if (AdminAlreadyOnCompany(currentUser.Id))
        {
            throw new AlreadyExistsException("Cannot create other Company");
        }
        company.CompanyAdminId = currentUser.Id;
        company.companyAdmin = (CompanyAdmin)currentUser;
        _repository.Insert(company);
        return company;
    }

    public Company Update(int id, Company updatedCompany)
    {
        if (NameExists(updatedCompany.Name))
        {
            throw new AlreadyExistsException("Name already being used");
        }
        var company = _repository.Get(company => company.Id == id);
        if (company == null)
        {
            throw new NotFoundException("Company not found");
        }
        if (!UserAuthorizedToEdit(company))
        {
            throw new UnauthorizedException("Unauthorized to update other companies");
        }
        company.Name = updatedCompany.Name;
        _repository.Update(company);
        return company;
    }

    public bool Delete(int id)
    {
        var company = _repository.Get(company => company.Id == id);
        if (company == null)
        {
            return false;
        }
        if (!UserAuthorizedToEdit(company))
        {
            throw new UnauthorizedException("Unauthorized to delete other companies");
        }
        _repository.Delete(company);
        return true;
    }

    private bool NameExists(string name)
    {
        List<Company> existingCompanys = _repository.GetAll<Company>().ToList();
        var company = existingCompanys.FirstOrDefault(company => company.Name == name);
        return company != null;
    }

    private bool AdminAlreadyOnCompany(int companyAdminId)
    {
        List<Company> existingCompanys = _repository.GetAll<Company>().ToList();
        return existingCompanys.Any(c => c.CompanyAdminId == companyAdminId);
    }

    private bool UserAuthorizedToEdit(Company company)
    {
        var currentUser = _sessionLogic.GetCurrentUser();
        return currentUser.Id == company.CompanyAdminId;
    }
}