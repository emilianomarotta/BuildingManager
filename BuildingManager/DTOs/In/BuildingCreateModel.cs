using Domain;
using System.Xml.Serialization;

namespace DTOs.In

{
    [XmlRoot("BuildingCreateModel")]
    public class BuildingCreateModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public int CompanyId { get; set; }
        public int Fees { get; set; }
        public int? ManagerId { get; set; }

        public Building ToEntity()
        {
            return new Building
            {
                Name = this.Name,
                Address = this.Address,
                Location = this.Location,
                CompanyId = this.CompanyId,
                Fees = this.Fees,
                ManagerId = this.ManagerId
            };
        }
    }
}
