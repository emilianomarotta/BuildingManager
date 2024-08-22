using DTOs.In;
namespace IImporter
{
    public interface ImporterInterface
    {
        string GetName();
        List<BuildingImportersModel> ImportBuilding(string datasourcePath);
    }
}
