using Domain;
namespace DTOs.In
{
    public class BuildingPutModel
    {
        public int Fees { get; set; }
        public int? ManagerId { get; set; }

        public Building ToEntity()
        {
            return new Building
            {
                Fees = this.Fees,
                ManagerId = this.ManagerId
            };
        }
    }
}
