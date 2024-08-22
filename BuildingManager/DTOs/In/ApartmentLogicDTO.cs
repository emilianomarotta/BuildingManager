using Domain;
using IBusinessLogic;
using IDataAccess;

namespace DTOs.In
{
    public class ApartmentLogicDTO
    {
        public IGenericRepository<Apartment> ApartmentRepository { get; set; }
        public IGenericRepository<Building> BuildingRepository { get; set; }
        public IGenericRepository<Owner> OwnerRepository { get; set; }
        public ISessionLogic SessionLogic { get; set; }

        public ApartmentLogicDTO(
            IGenericRepository<Apartment> apartmentRepository,
            IGenericRepository<Building> buildingRepository,
            IGenericRepository<Owner> ownerRepository,
            ISessionLogic sessionLogic)
        {
            ApartmentRepository = apartmentRepository;
            BuildingRepository = buildingRepository;
            OwnerRepository = ownerRepository;
            SessionLogic = sessionLogic;
        }
    }
}