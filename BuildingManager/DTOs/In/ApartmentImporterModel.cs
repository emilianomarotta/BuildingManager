using System.Xml.Serialization;

namespace DTOs.In
{
    [XmlRoot("apartment")]
    public class ApartmentImporterModel
    {
        [XmlElement("piso")]
        public int Piso { get; set; }

        [XmlElement("numero_puerta")]
        public int NumeroPuerta { get; set; }

        [XmlElement("habitaciones")]
        public int Habitaciones { get; set; }

        [XmlElement("conTerraza")]
        public bool ConTerraza { get; set; }

        [XmlElement("baños")]
        public int Baños { get; set; }

        [XmlElement("propietarioEmail")]
        public string PropietarioEmail { get; set; }
    }
}
