using Domain;
using DTOs.In;
using IBusinessLogic.Exceptions;
using IBusinessLogic;
using Domain.DataTypes;
using IDataAccess;

namespace BusinessLogic;

public class InvitationLogic : IBusinessLogic<Invitation>
{
    private readonly IGenericRepository<Invitation> _invitationRepository;
    private readonly IGenericRepository<Administrator> _administratorRepository;
    private readonly IGenericRepository<Manager> _managerRepository;
    private readonly IGenericRepository<Staff> _staffRepository;
    private readonly IGenericRepository<CompanyAdmin> _companyAdminRepository;

    public InvitationLogic(InvitationLogicDTO dto)
    {
        _staffRepository = dto.StaffRepository;
        _administratorRepository = dto.AdministratorRepository;
        _managerRepository = dto.ManagerRepository;
        _companyAdminRepository = dto.CompanyAdminRepository;
        _invitationRepository = dto.InvitationRepository;
    }

    public List<Invitation> GetAll()
    {
        return _invitationRepository.GetAll<Invitation>().ToList();
    }

    public Invitation GetById(int id)
    {
        return _invitationRepository.Get(invitation => invitation.Id == id);
    }

    public Invitation Create(Invitation invitation)
    {
        if (EmailExists(invitation.Email))
        {
            throw new AlreadyExistsException("Email already being used");
        }
        if (InvitationToEmailExists(invitation.Email) && InvitationToEmailPending(invitation.Email))
        {
            throw new AlreadyExistsException("Invitation already exists");
        }
        ValidateInvitation(invitation);
        _invitationRepository.Insert(invitation);
        return invitation;
    }

    public Invitation Update(int id, Invitation updatedInvitation)
    {
        Invitation invitation = _invitationRepository.Get(invitation => invitation.Id == id);
        if (invitation == null || invitation.Status != Status.Pending)
        {
            throw new NotFoundException("Invitation not found, expired, rejected or already accepted");
        }
        if (invitation.Expiration < DateTime.Today)
        {
            UpdateInvitationStatus(invitation, Status.Expired);
            throw new AlreadyExistsException("Invitation has expired");
        }
        if (invitation.Expiration >= DateTime.Today)
        {
            if (invitation.Email != updatedInvitation.Email)
            {
                throw new InconsistentDataException("Email provided does not match to invitation Email");
            }
            Status status = updatedInvitation.Status;
            UpdateInvitationStatus(invitation, status);
        }
        return invitation;
    }

    public bool Delete(int id)
    {
        var invitation = _invitationRepository.Get(invitation => invitation.Id == id);
        if (invitation == null)
        {
            return false;
        }
        if (invitation.Status == Status.Accepted)
        {
            throw new AlreadyExistsException("Accepted invitations cannot be deleted");
        }
        _invitationRepository.Delete(invitation);
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


    private bool InvitationToEmailExists(string email)
    {
        return _invitationRepository.GetAll<Invitation>().Any(invitation => invitation.Email == email);
    }

    private bool InvitationToEmailPending(string email)
    {
        Invitation invitation = _invitationRepository.GetAll<Invitation>().FirstOrDefault(invitation => invitation.Email == email);
        return invitation.Status == Status.Pending;
    }

    private void UpdateInvitationStatus(Invitation invitation, Status status)
    {
        invitation.Status = status;
        _invitationRepository.Update(invitation);
    }

    private void ValidateInvitation(Invitation invitation)
    {
        if (invitation.Expiration < DateTime.Today)
        {
            throw new InconsistentDataException("Expiration date must be greater than today");
        }

    }
}