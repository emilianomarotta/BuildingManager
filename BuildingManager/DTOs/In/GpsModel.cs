using System.Xml.Serialization;

namespace DTOs.In
{
    [XmlRoot("gps")]
    public class GpsModel
    {
        [XmlElement("latitud")]
        public double Latitud { get; set; }

        [XmlElement("longitud")]
        public double Longitud { get; set; }
    }
}
