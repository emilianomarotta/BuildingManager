using DTOs.In;
using IImporter;
using IImporter.Exceptions;
using System.Xml.Serialization;

namespace XMLImporter
{
    public class ImporterXML : ImporterInterface
    {
        public string GetName()
        {
            return "XML Importer";
        }

        public List<BuildingImportersModel> ImportBuilding(string datasourcePath)
        {
            try
            {
                FileInfo xmlFile = new FileInfo(datasourcePath);
                using (FileStream stream = new FileStream(xmlFile.FullName, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(BuildingCreateModelList));
                    BuildingCreateModelList buildingList = (BuildingCreateModelList)serializer.Deserialize(stream);
                    return buildingList.Buildings;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidFormatException("Invalid XML format");
            }
        }
    }
}