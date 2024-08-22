using Domain;
using IBusinessLogic;
using IBusinessLogic.Exceptions;
using IDataAccess;
using DTOs.In;

namespace BusinessLogic;

public class BuildingLogic : IBusinessLogic<Building>
{
    private readonly IGenericRepository<Building> _buildingRepository;
    private readonly IGenericRepository<Company> _companyRepository;
    private readonly IGenericRepository<Manager> _managerRepository;
    private readonly ISessionLogic _sessionLogic;

    public BuildingLogic(BuildingLogicDTO dto)
    {
        _buildingRepository = dto.BuildingRepository;
        _managerRepository = dto.ManagerRepository;
        _companyRepository = dto.CompanyRepository;
        _sessionLogic = dto.SessionLogic;
    }

    public List<Building> GetAll()
    {
        var currentUser = _sessionLogic.GetCurrentUser();
        int companyId = 0;
        if (currentUser is CompanyAdmin)
        {
            companyId = GetCompanyIdCurrentUser(currentUser);
        }
        int? userId = currentUser.Id;
        var buildings = _buildingRepository.GetAll<Building>().Where(building => building.CompanyId == companyId || building.ManagerId == userId).ToList();
        LoadManagersBuildings(buildings);
        LoadCompaniesBuildings(buildings);
        return buildings;
    }

    public Building GetById(int id)
    {
        var currentUser = _sessionLogic.GetCurrentUser();
        int companyId = 0;
        if (currentUser is CompanyAdmin)
        {
            companyId = GetCompanyIdCurrentUser(currentUser);
        }
        int? userId = currentUser.Id;
        var building = _buildingRepository.Get(building => (building.Id == id && (building.CompanyId == companyId || building.ManagerId == userId)));
        if (building != null)
        {
            LoadManagerBuilding(building);
            LoadCompanyBuilding(building);
        }
        return building;
    }

    public Building Create(Building building)
    {
        var currentUser = _sessionLogic.GetCurrentUser();
        if (BuildingNameExists(building))
        {
            throw new AlreadyExistsException("Building already exists");
        }
        if (building.ManagerId != null)
        {
            int? managerId = building.ManagerId;
            var manager = _managerRepository.Get(manager => manager.Id == managerId);
            if (manager == null)
            {
                throw new NotFoundException("Manager not found");
            }
            building.Manager = manager;
        }
        int adminCompanyId = GetCompanyIdCurrentUser(currentUser);
        int companyId = adminCompanyId;
        var company = _companyRepository.Get(company => company.Id == companyId);
        if (company == null)
        {
            throw new NotFoundException("Company not found");
        }
        if (company.Id != adminCompanyId)
        {
            throw new UnauthorizedException("Unauthorized to create buildings for other companies");
        }
        building.Company = company;
        _buildingRepository.Insert(building);
        return building;
    }

    public Building Update(int id, Building updatedBuilding)
    {
        var currentUser = _sessionLogic.GetCurrentUser();
        var building = _buildingRepository.Get(building => building.Id == id);
        if (building == null)
        {
            throw new NotFoundException("Building not found");
        }
        int adminCompanyId = GetCompanyIdCurrentUser(currentUser);
        if (building.CompanyId != adminCompanyId)
        {
            throw new UnauthorizedException("Unauthorized to update other companies buildings");
        }
        if (updatedBuilding.ManagerId != null)
        {
            int? managerId = updatedBuilding.ManagerId;
            var manager = _managerRepository.Get(manager => manager.Id == managerId);
            if (manager == null)
            {
                throw new NotFoundException("Manager not found");
            }
            building.Manager = manager;
        }
        building.Fees = updatedBuilding.Fees;
        _buildingRepository.Update(building);
        return building;
    }

    public bool Delete(int id)
    {
        var building = _buildingRepository.Get(building => building.Id == id);
        if (building == null)
        {
            return false;
        }
        var currentUser = _sessionLogic.GetCurrentUser();
        int adminCompanyId = GetCompanyIdCurrentUser(currentUser);
        if (building.CompanyId != adminCompanyId)
        {
            throw new UnauthorizedException("Unauthorized to delete other companies buildings");
        }
        _buildingRepository.Delete(building);
        return true;
    }

    private void LoadManagersBuildings(List<Building> buildings)
    {
        foreach (var building in buildings)
        {
            LoadManagerBuilding(building);
        }
    }
    private void LoadManagerBuilding(Building building)
    {
        if (building.ManagerId.HasValue)
        {
            int? managerId = building.ManagerId;
            building.Manager = _managerRepository.Get(manager => manager.Id == managerId);
        }
    }
    private void LoadCompaniesBuildings(List<Building> buildings)
    {
        foreach (var building in buildings)
        {
            LoadCompanyBuilding(building);
        }
    }
    private void LoadCompanyBuilding(Building building)
    {
        int companyId = building.CompanyId;
        building.Company = _companyRepository.Get(company => company.Id == companyId);

    }


    private bool BuildingNameExists(Building building)
    {
        List<Building> existingBuildings = _buildingRepository.GetAll<Building>().ToList();
        var existBuilding = existingBuildings.FirstOrDefault(b => b.Equals(building));
        if (existBuilding != null)
        {
            return true;
        }
        return false;
    }

    private int GetCompanyIdCurrentUser(User currentUser)
    {
        currentUser = _sessionLogic.GetCurrentUser();
        int currentUserId = currentUser.Id;
        var company = _companyRepository.Get(company => company.CompanyAdminId == currentUserId);
        var companyId = company.Id;
        return companyId;
    }

}