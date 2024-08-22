using System.Xml.Serialization;

namespace DTOs.In
{
    [XmlRoot("building")]
    public class BuildingImportersModel
    {
        [XmlElement("nombre")]
        public string Nombre { get; set; }

        [XmlElement("direccion")]
        public AddressModel Direccion { get; set; }

        [XmlElement("encargado")]
        public string? Encargado { get; set; }

        [XmlElement("gps")]
        public GpsModel Gps { get; set; }

        [XmlElement("gastos_comunes")]
        public int Gastos_Comunes { get; set; }

        [XmlElement("departamentos")]
        public List<ApartmentImporterModel> Departamentos { get; set; }
    }
}
