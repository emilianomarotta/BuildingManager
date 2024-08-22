using DTOs.In;
using IImporter;
using IImporter.Exceptions;
using System.Text.Json;

namespace JSONImporter
{
    public class ImporterJSON : ImporterInterface
    {
        public string GetName()
        {
            return "JSON Importer";
        }

        public List<BuildingImportersModel> ImportBuilding(string datasourcePath)
        {
            try
            {
                var jsonString = File.ReadAllText(datasourcePath);
                var buildings = JsonSerializer.Deserialize<List<BuildingImportersModel>>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (buildings == null)
                {
                    throw new InvalidFormatException("Deserialized list is null");
                }

                return buildings;
            }
            catch (JsonException)
            {
                throw new InvalidFormatException("Invalid JSON format");
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException($"The file at path {datasourcePath} was not found.");
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
