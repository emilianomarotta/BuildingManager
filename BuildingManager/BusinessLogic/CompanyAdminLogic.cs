using Domain;
using DTOs.In;
using IBusinessLogic;
using IBusinessLogic.Exceptions;
using IDataAccess;

namespace BusinessLogic;

public class CompanyAdminLogic : IBusinessLogic<CompanyAdmin>
{
    private readonly IGenericRepository<CompanyAdmin> _companyAdminRepository;
    private readonly IGenericRepository<Administrator> _administratorRepository;
    private readonly IGenericRepository<Manager> _managerRepository;
    private readonly IGenericRepository<Staff> _staffRepository;
    private readonly ISessionLogic _sessionLogic;

    public CompanyAdminLogic(CompanyAdminLogicDTO dto)
    {
        _staffRepository = dto.StaffRepository;
        _administratorRepository = dto.AdministratorRepository;
        _managerRepository = dto.ManagerRepository;
        _companyAdminRepository = dto.CompanyAdminRepository;
        _sessionLogic = dto.SessionLogic;
    }

    public List<CompanyAdmin> GetAll()
    {
        return _companyAdminRepository.GetAll<CompanyAdmin>().ToList();
    }

    public CompanyAdmin GetById(int id)
    {
        return _companyAdminRepository.Get(companyAdmin => companyAdmin.Id == id);
    }

    public CompanyAdmin Create(CompanyAdmin companyAdmin)
    {
        if (EmailExists(companyAdmin.Email))
        {
            throw new AlreadyExistsException("Email already being used");
        }
        _companyAdminRepository.Insert(companyAdmin);
        return companyAdmin;
    }

    public CompanyAdmin Update(int id, CompanyAdmin updatedCompanyAdmin)
    {
        var currentUser = _sessionLogic.GetCurrentUser();
        var companyAdmin = _companyAdminRepository.Get(companyAdmin => companyAdmin.Id == id);
        if (companyAdmin == null)
        {
            throw new NotFoundException("Company Administrator not found");
        }
        if (currentUser.Id != id)
        {
            throw new UnauthorizedException("Unauthorized to edit other users");
        }
        if (companyAdmin.Email != updatedCompanyAdmin.Email)
        {
            if (EmailExists(updatedCompanyAdmin.Email))
            {
                throw new AlreadyExistsException("Email already being used");
            }
            companyAdmin.Email = updatedCompanyAdmin.Email;
        }
        companyAdmin.Name = updatedCompanyAdmin.Name;
        companyAdmin.LastName = updatedCompanyAdmin.LastName;
        companyAdmin.Password = updatedCompanyAdmin.Password;
        _companyAdminRepository.Update(companyAdmin);
        return companyAdmin;
    }

    public bool Delete(int id)
    {
        var companyAdmin = _companyAdminRepository.Get(companyAdmin => companyAdmin.Id == id);
        if (companyAdmin == null)
        {
            return false;
        }
        _companyAdminRepository.Delete(companyAdmin);
        return true;
    }

    private bool EmailExists(string email)
    {
        if (EmailExistStaff(email) || EmailExistManager(email) || EmailExistCompanyAdmin(email) || EmailExistAdministrator(email))
        {
            return true;
        }
        return false;
    }

    private bool EmailExistStaff(string email)
    {
        List<Staff> existingStaffs = _staffRepository.GetAll<Staff>().ToList();
        var staff = existingStaffs.FirstOrDefault(staff => staff.Email == email);
        if (staff != null)
        {
            return true;
        }
        return false;
    }

    private bool EmailExistManager(string email)
    {
        List<Manager> existingManagers = _managerRepository.GetAll<Manager>().ToList();
        var manager = existingManagers.FirstOrDefault(manager => manager.Email == email);
        if (manager != null)
        {
            return true;
        }
        return false;
    }

    private bool EmailExistCompanyAdmin(string email)
    {
        List<CompanyAdmin> existingCompanyAdmins = _companyAdminRepository.GetAll<CompanyAdmin>().ToList();
        var companyAdmin = existingCompanyAdmins.FirstOrDefault(companyAdmin => companyAdmin.Email == email);
        if (companyAdmin != null)
        {
            return true;
        }
        return false;
    }
    private bool EmailExistAdministrator(string email)
    {
        List<Administrator> existingAdministrators = _administratorRepository.GetAll<Administrator>().ToList();
        var administrator = existingAdministrators.FirstOrDefault(administrator => administrator.Email == email);
        if (administrator != null)
        {
            return true;
        }
        return false;
    }
}