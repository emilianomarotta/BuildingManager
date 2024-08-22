using Domain;
using DTOs.In;
using IBusinessLogic;
using IBusinessLogic.Exceptions;
using IDataAccess;

namespace BusinessLogic;

public class AdministratorLogic : IBusinessLogic<Administrator>
{
    private readonly IGenericRepository<Administrator> _administratorRepository;
    private readonly IGenericRepository<Manager> _managerRepository;
    private readonly IGenericRepository<Staff> _staffRepository;
    private readonly IGenericRepository<CompanyAdmin> _companyAdminRepository;
    private readonly ISessionLogic _sessionLogic;

    public AdministratorLogic(AdministratorLogicDTO dto)
    {
        _staffRepository = dto.StaffRepository;
        _administratorRepository = dto.AdministratorRepository;
        _managerRepository = dto.ManagerRepository;
        _companyAdminRepository = dto.CompanyAdminRepository;
        _sessionLogic = dto.SessionLogic;
    }

    public List<Administrator> GetAll()
    {
        return _administratorRepository.GetAll<Administrator>().ToList();
    }

    public Administrator GetById(int id)
    {
        return _administratorRepository.Get(administrator => administrator.Id == id);
    }

    public Administrator Create(Administrator administrator)
    {
        if (EmailExists(administrator.Email))
        {
            throw new AlreadyExistsException("Administrator already exists");
        }
        _administratorRepository.Insert(administrator);
        return administrator;
    }

    public Administrator Update(int id, Administrator updatedAdministrator)
    {
        var currentUser = _sessionLogic.GetCurrentUser();
        var administrator = _administratorRepository.Get(administrator => administrator.Id == id);
        if (administrator == null)
        {
            throw new NotFoundException("Administrator not found");
        }
        if (currentUser.Id != id)
        {
            throw new UnauthorizedException("Unauthorized to edit other adminstrators");
        }
        if (administrator.Email != updatedAdministrator.Email)
        {
            if (EmailExists(updatedAdministrator.Email))
            {
                throw new AlreadyExistsException("Email already being used");
            }
            administrator.Email = updatedAdministrator.Email;
        }
        administrator.Name = updatedAdministrator.Name;
        administrator.LastName = updatedAdministrator.LastName;
        administrator.Password = updatedAdministrator.Password;
        _administratorRepository.Update(administrator);
        return administrator;
    }

    public bool Delete(int id)
    {
        var administrator = _administratorRepository.Get(administrator => administrator.Id == id);
        if (administrator == null)
        {
            return false;
        }
        _administratorRepository.Delete(administrator);
        return true;
    }

    private bool EmailExists(string email)
    {
        if (EmailExistStaff(email) || EmailExistManager(email) || EmailExistAdministrator(email) || EmailExistCompanyAdmin(email))
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
}