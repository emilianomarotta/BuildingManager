using System.Xml.Serialization;

namespace DTOs.In
{
    [XmlRoot("direccion")]
    public class AddressModel
    {
        [XmlElement("calle_principal")]
        public string Calle_Principal { get; set; }

        [XmlElement("numero_puerta")]
        public int Numero_Puerta { get; set; }

        [XmlElement("calle_secundaria")]
        public string Calle_Secundaria { get; set; }
    }
}
