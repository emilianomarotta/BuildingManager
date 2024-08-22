using System.Xml.Serialization;

namespace DTOs.In
{
    [XmlRoot("departamentos")]
    public class ApartmentImporterModelList
    {
        [XmlElement("apartment")]
        public List<ApartmentImporterModel> Apartments { get; set; }
    }
}
