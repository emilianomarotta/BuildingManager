using IBusinessLogic;
using IImporterLogic;
using Domain;
using Microsoft.AspNetCore.Mvc;
using DTOs.Out;
using DTOs.In;
using WebApi.Filters;

namespace WebAPI
{
    [ApiController]
    [Route("api/importers")]
    [AuthenticationFilter("CompanyAdmin")]
    public class ImporterController : Controller
    {
        private readonly ImporterLogicInterface _importerLogic;
        private readonly IBusinessLogic<Building> _buildingLogic;
        private readonly IBusinessLogic<Manager> _managerLogic;

        public ImporterController(ImporterLogicInterface importerLogic, IBusinessLogic<Building> buildingLogic, IBusinessLogic<Manager> managerLogic)
        {
            _importerLogic = importerLogic;
            _buildingLogic = buildingLogic;
            _managerLogic = managerLogic;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var availableImporters = _importerLogic.GetAllImporters();
            return Ok(availableImporters.Select(i => i.GetName()).ToList());
        }

        [HttpGet("import")]
        public IActionResult ImportBuildings(string importerName, string datasourcePath)
        {
            var availableImporters = _importerLogic.GetAllImporters();
            var importer = availableImporters.FirstOrDefault(i => i.GetName() == importerName);
            if (importer == null)
            {
                return NotFound($"Importer '{importerName}' not found.");
            }

            var buildings = importer.ImportBuilding(datasourcePath);
            if (!HasValidBuildings(buildings))
            {
                return NoContent();
            }
            var errors = new List<string>();
            var createdBuildings = new List<BuildingDetailModel>();

            foreach (var buildingImporterModel in buildings)
            {
                try
                {
                    var buildingCreateModel = new BuildingCreateModel
                    {
                        Name = buildingImporterModel.Nombre,
                        Address = $"Ciudad, {buildingImporterModel.Direccion.Calle_Principal} {buildingImporterModel.Direccion.Numero_Puerta.ToString()}",
                        Location = $"{buildingImporterModel.Gps.Latitud.ToString()}, {buildingImporterModel.Gps.Longitud.ToString()}",
                        CompanyId = 0,
                        Fees = buildingImporterModel.Gastos_Comunes,
                        ManagerId = GetManagerIdByEmail(buildingImporterModel.Encargado)
                    };
                    Console.WriteLine(buildingCreateModel.Name);
                    Console.WriteLine(buildingCreateModel.Address);
                    Console.WriteLine(buildingCreateModel.Location);
                    Console.WriteLine(buildingCreateModel.Fees);

                    var correctBuilding = _buildingLogic.Create(buildingCreateModel.ToEntity());
                    createdBuildings.Add(new BuildingDetailModel(correctBuilding));
                }
                catch (Exception ex)
                {
                    errors.Add($"Error for building {buildingImporterModel.Nombre}: {ex.Message}");
                }
            }
            var result = CreateImportResult(createdBuildings, errors);
            if (errors.Count > 0)
            {
                Console.Write(buildings.ToList());
                return BadRequest(result);
            }

            return Ok(result);
        }
        
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Importers");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var filePath = Path.Combine(uploadPath, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { FilePath = filePath });
        }


        private bool HasValidBuildings(List<BuildingImportersModel>? buildings)
        {
            return buildings != null && buildings.Count > 0;
        }

        private object CreateImportResult(List<BuildingDetailModel> createdBuildings, List<string> errors)
        {
            return new
            {
                CreatedBuildings = createdBuildings,
                Errors = errors
            };
        }

        private int? GetManagerIdByEmail(string email)
        {
            int? id = null;
            if (string.IsNullOrEmpty(email))
            {
                return id;
            }
            var managers = _managerLogic.GetAll();
            var manager = managers.FirstOrDefault(manager => manager.Email == email);
            if (manager == null)
            {
                return 0;
            }
            return manager.Id;
        }
    }
}

