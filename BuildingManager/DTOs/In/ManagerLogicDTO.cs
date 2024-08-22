using Domain;
using IDataAccess;
using IBusinessLogic;

namespace DTOs.In
{
    public class ManagerLogicDTO
    {
        public IGenericRepository<Staff> StaffRepository { get; set; }
        public IGenericRepository<Administrator> AdministratorRepository { get; set; }
        public IGenericRepository<Manager> ManagerRepository { get; set; }
        public IGenericRepository<CompanyAdmin> CompanyAdminRepository { get; set; }
        public ISessionLogic SessionLogic { get; set; }

        public ManagerLogicDTO(
            IGenericRepository<Staff> _staffRepository,
            IGenericRepository<Administrator> _administratorRepository,
            IGenericRepository<Manager> _managerRepository,
            IGenericRepository<CompanyAdmin> _companyAdminRepository,
            ISessionLogic _sessionLogic)
        {
            StaffRepository = _staffRepository;
            AdministratorRepository = _administratorRepository;
            ManagerRepository = _managerRepository;
            CompanyAdminRepository = _companyAdminRepository;
            SessionLogic = _sessionLogic;
        }
    }
}