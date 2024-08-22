using System.Xml.Serialization;

namespace DTOs.In
{
    [XmlRoot("buildings")]
    public class BuildingCreateModelList
    {
        [XmlElement("building")]
        public List<BuildingImportersModel> Buildings { get; set; }
    }
}
