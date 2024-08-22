using Domain;
using DTOs.In;
using IBusinessLogic;
using IBusinessLogic.Exceptions;
using IDataAccess;

namespace BusinessLogic;

public class StaffLogic : IBusinessLogic<Staff>
{
    private readonly IGenericRepository<Staff> _staffRepository;
    private readonly IGenericRepository<Administrator> _administratorRepository;
    private readonly IGenericRepository<Manager> _managerRepository;
    private readonly IGenericRepository<CompanyAdmin> _companyAdminRepository;
    private readonly ISessionLogic _sessionLogic;

    public StaffLogic(StaffLogicDTO dto)
    {
        _staffRepository = dto.StaffRepository;
        _administratorRepository = dto.AdministratorRepository;
        _managerRepository = dto.ManagerRepository;
        _companyAdminRepository = dto.CompanyAdminRepository;
        _sessionLogic = dto.SessionLogic;
    }

    public List<Staff> GetAll()
    {
        return _staffRepository.GetAll<Staff>().ToList();
    }

    public Staff GetById(int id)
    {
        var currentUser = _sessionLogic.GetCurrentUser();
        var staff = _staffRepository.Get(staff => staff.Id == id);
        if (staff == null)
        {
            throw new NotFoundException("Staff not found");
        }
        return staff;
    }

    public Staff Create(Staff staff)
    {
        if (EmailExists(staff.Email))
        {
            throw new AlreadyExistsException("Staff already exists");
        }
        _staffRepository.Insert(staff);
        return staff;
    }

    public Staff Update(int id, Staff updatedStaff)
    {
        var staff = _staffRepository.Get(staff => staff.Id == id);
        if (staff == null)
        {
            throw new NotFoundException("Staff not found");
        }
        if (staff.Email != updatedStaff.Email)
        {
            if (EmailExists(updatedStaff.Email))
            {
                throw new AlreadyExistsException("Email already being used");
            }
            staff.Email = updatedStaff.Email;
        }
        staff.Name = updatedStaff.Name;
        staff.LastName = updatedStaff.LastName;
        staff.Password = updatedStaff.Password;
        _staffRepository.Update(staff);
        return staff;
    }

    public bool Delete(int id)
    {
        var staff = _staffRepository.Get(staff => staff.Id == id);
        if (staff == null)
        {
            return false;
        }
        _staffRepository.Delete(staff);
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