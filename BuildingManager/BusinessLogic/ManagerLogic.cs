using Domain;
using DTOs.In;
using IBusinessLogic;
using IBusinessLogic.Exceptions;
using IDataAccess;

namespace BusinessLogic;

public class ManagerLogic : IBusinessLogic<Manager>
{
    private readonly IGenericRepository<Manager> _managerRepository;
    private readonly IGenericRepository<Administrator> _administratorRepository;
    private readonly IGenericRepository<Staff> _staffRepository;
    private readonly IGenericRepository<CompanyAdmin> _companyAdminRepository;
    private readonly ISessionLogic _sessionLogic;
    public ManagerLogic(ManagerLogicDTO dto)
    {
        _staffRepository = dto.StaffRepository;
        _administratorRepository = dto.AdministratorRepository;
        _managerRepository = dto.ManagerRepository;
        _companyAdminRepository = dto.CompanyAdminRepository;
        _sessionLogic = dto.SessionLogic;
    }

    public List<Manager> GetAll()
    {
        return _managerRepository.GetAll<Manager>().ToList();
    }

    public Manager GetById(int id)
    {
        var currentUser = _sessionLogic.GetCurrentUser();
        if(currentUser is Manager && currentUser.Id != id)
        {
            throw new UnauthorizedException("Unauthorized to view other managers");
        }
        return _managerRepository.Get(manager => manager.Id == id);
    }

    public Manager Create(Manager manager)
    {
        if (EmailExists(manager.Email))
        {
            throw new AlreadyExistsException("Manager already exists");
        }
        _managerRepository.Insert(manager);
        return manager;
    }

    public Manager Update(int id, Manager updatedManager)
    {
        var currentUser = _sessionLogic.GetCurrentUser();
        var manager = _managerRepository.Get(manager => manager.Id == id);
        if (manager == null)
        {
            throw new NotFoundException("Manager not found");
        }
        if (currentUser.Id != id)
        {
            throw new UnauthorizedException("Unauthorized to edit other managers");
        }
        if (manager.Email != updatedManager.Email)
        {
            if (EmailExists(updatedManager.Email))
            {
                throw new AlreadyExistsException("Email already being used");
            }
            manager.Email = updatedManager.Email;
        }
        manager.Name = updatedManager.Name;
        manager.LastName = updatedManager.LastName;
        manager.Password = updatedManager.Password;
        _managerRepository.Update(manager);
        return manager;
    }

    public bool Delete(int id)
    {
        var manager = _managerRepository.Get(manager => manager.Id == id);
        if (manager == null)
        {
            return false;
        }
        _managerRepository.Delete(manager);
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