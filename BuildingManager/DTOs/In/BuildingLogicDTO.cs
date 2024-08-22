using Domain;
using IBusinessLogic;
using IDataAccess;

namespace DTOs.In
{
    public class BuildingLogicDTO
    {
        public IGenericRepository<Building> BuildingRepository { get; set; }
        public IGenericRepository<Manager> ManagerRepository { get; set; }
        public IGenericRepository<Company> CompanyRepository { get; set; }
        public ISessionLogic SessionLogic { get; set; }

        public BuildingLogicDTO(
            IGenericRepository<Building> buildingRepository,
            IGenericRepository<Manager> managerRepository,
            IGenericRepository<Company> companyRepository,
            ISessionLogic sessionLogic)
        {
            BuildingRepository = buildingRepository;
            ManagerRepository = managerRepository;
            CompanyRepository = companyRepository;
            SessionLogic = sessionLogic;
        }
    }
}