using IBusinessLogic.Exceptions;
using Domain;
using IBusinessLogic;
using IDataAccess;
using DTOs.In;

namespace BusinessLogic
{
    public class ApartmentLogic : IBusinessLogic<Apartment>
    {
        private readonly IGenericRepository<Apartment> _apartmentRepository;
        private readonly IGenericRepository<Building> _buildingRepository;
        private readonly IGenericRepository<Owner> _ownerRepository;
        private readonly ISessionLogic _sessionLogic;

        public ApartmentLogic(ApartmentLogicDTO dto)
        {
            _apartmentRepository = dto.ApartmentRepository;
            _buildingRepository = dto.BuildingRepository;
            _ownerRepository = dto.OwnerRepository;
            _sessionLogic = dto.SessionLogic;
        }

        public List<Apartment> GetAll()
        {
            var currentUser = _sessionLogic.GetCurrentUser();
            return this.GetApartmentsForCurrentUser(currentUser).ToList();
        }

        public Apartment GetById(int id)
        {
            var currentUser = _sessionLogic.GetCurrentUser();
            var apartment = _apartmentRepository.Get(apartment => apartment.Id == id);
            if (apartment == null)
            {
                throw new NotFoundException("Apartment not found");
            }
            int buildingId = apartment.BuildingId;
            Building building = _buildingRepository.Get(b => b.Id == buildingId);
            if (building.ManagerId != currentUser.Id)
            {
                throw new UnauthorizedException("Unauthorized to view this apartment");
            }
            return apartment;
        }

        public Apartment Create(Apartment apartment)
        {
            var currentUser = _sessionLogic.GetCurrentUser();
            if (!BuildingExists(apartment.BuildingId))
            {
                throw new NotFoundException("Building not found");
            }
            int buildingId = apartment.BuildingId;
            Building building = _buildingRepository.Get(b => b.Id == buildingId);
            if (building.ManagerId != currentUser.Id)
            {
                throw new UnauthorizedException("Unauthorized to create apartments in this building");
            }
            Owner owner = OwnerExists(apartment.OwnerId);
            if (owner == null)
            {
                throw new NotFoundException("Owner not found");
            }
            if (ApartmentExists(apartment))
            {
                throw new AlreadyExistsException("Apartment already exists");
            }
            _apartmentRepository.Insert(apartment);
            return apartment;
        }

        public Apartment Update(int id, Apartment updatedApartment)
        {
            var currentUser = _sessionLogic.GetCurrentUser();
            Apartment apartment = _apartmentRepository.Get(apartment => apartment.Id == id);
            if (apartment == null)
            {
                throw new NotFoundException("Apartment not found");
            }
            int buildingId = apartment.BuildingId;
            Building building = _buildingRepository.Get(b => b.Id == buildingId);
            if (building.ManagerId != currentUser.Id)
            {
                throw new UnauthorizedException("Unauthorized to update apartments in this building");
            }
            Owner newOwner = OwnerExists(updatedApartment.OwnerId);
            if (newOwner == null)
            {
                throw new NotFoundException("Owner not found");
            }
            updatedApartment.Owner = newOwner;
            if (newOwner.Id != apartment.OwnerId)
            {
                apartment.Owner = updatedApartment.Owner;
                apartment.OwnerId = updatedApartment.OwnerId;
                _apartmentRepository.Update(apartment);

            }
            return apartment;
        }

        public bool Delete(int id)
        {
            var currentUser = _sessionLogic.GetCurrentUser();
            Apartment apartment = _apartmentRepository.Get(a => a.Id == id);
            if (apartment == null)
            {
                return false;
            }
            int buildingId = apartment.BuildingId;
            Building building = _buildingRepository.Get(b => b.Id == buildingId);
            if (building.ManagerId != currentUser.Id)
            {
                throw new UnauthorizedException("Unauthorized to delete apartments in this building");
            }
            _apartmentRepository.Delete(apartment);
            return true;
        }

        private bool ApartmentExists(Apartment apartment)
        {
            int apartmentId = apartment.Id;
            Apartment apartmentToValidate = _apartmentRepository.Get(a => a.Id == apartmentId);
            return apartmentToValidate != null;
        }

        private Owner OwnerExists(int ownerId)
        {
            Owner ownerToValidate = _ownerRepository.Get(o => o.Id == ownerId);
            return ownerToValidate;
        }

        private bool BuildingExists(int buildingId)
        {
            Building buildingToValidate = _buildingRepository.Get(b => b.Id == buildingId);
            return buildingToValidate != null;
        }

        private IEnumerable<Apartment> GetApartmentsForCurrentUser(User currentUser)
        {
            var buildingIds = _buildingRepository
                .GetAll<Building>()
                .Where(b => b.ManagerId == currentUser.Id)
                .Select(b => b.Id)
                .ToList();

            return _apartmentRepository
                .GetAll<Apartment>()
                .Where(a => buildingIds.Contains(a.BuildingId))
                .ToList();
        }
    }
}


