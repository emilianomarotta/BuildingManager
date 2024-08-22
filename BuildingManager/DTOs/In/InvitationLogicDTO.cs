using Domain;
using IDataAccess;

namespace DTOs.In
{
    public class InvitationLogicDTO
    {
        public IGenericRepository<Staff> StaffRepository { get; set; }
        public IGenericRepository<Administrator> AdministratorRepository { get; set; }
        public IGenericRepository<Manager> ManagerRepository { get; set; }
        public IGenericRepository<CompanyAdmin> CompanyAdminRepository { get; set; }
        public IGenericRepository<Invitation> InvitationRepository { get; set; }

        public InvitationLogicDTO(
            IGenericRepository<Staff> _staffRepository,
            IGenericRepository<Administrator> _administratorRepository,
            IGenericRepository<Manager> _managerRepository,
            IGenericRepository<CompanyAdmin> _companyAdminRepository,
           IGenericRepository<Invitation> _invitationRepository)
        {
            StaffRepository = _staffRepository;
            AdministratorRepository = _administratorRepository;
            ManagerRepository = _managerRepository;
            CompanyAdminRepository = _companyAdminRepository;
            InvitationRepository = _invitationRepository;
        }
    }
}