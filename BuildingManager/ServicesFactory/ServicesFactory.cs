using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DataAccess;
using Domain;
using IBusinessLogic;
using IImporterLogic;
using IDataAccess;
using BusinessLogic;
using Task = Domain.Task;
using DTOs.In;

namespace ServiceFactory;

public class ServicesFactory
{
    public ServicesFactory() { }

    public static void RegisterServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IBusinessLogic<Administrator>, AdministratorLogic>();
        serviceCollection.AddTransient<IBusinessLogic<Staff>, StaffLogic>();
        serviceCollection.AddTransient<IBusinessLogic<Manager>, ManagerLogic>();
        serviceCollection.AddTransient<IBusinessLogic<Invitation>, InvitationLogic>();
        serviceCollection.AddTransient<IBusinessLogic<Company>, CompanyLogic>();
        serviceCollection.AddTransient<IBusinessLogic<Category>, CategoryLogic>();
        serviceCollection.AddTransient<IBusinessLogic<Owner>, OwnerLogic>();
        serviceCollection.AddTransient<IBusinessLogic<Building>, BuildingLogic>();
        serviceCollection.AddTransient<IBusinessLogic<Apartment>, ApartmentLogic>();
        serviceCollection.AddTransient<IBusinessLogic<Task>, TaskLogic>();
        serviceCollection.AddScoped<ISessionLogic, SessionLogic>();
        serviceCollection.AddTransient<UserLogic, UserLogic>();
        serviceCollection.AddTransient<ImporterLogicInterface, ImporterLogic>();
        serviceCollection.AddTransient<IBusinessLogic<CompanyAdmin>, CompanyAdminLogic>();

        serviceCollection.AddTransient<TaskLogicDTO>();
        serviceCollection.AddTransient<StaffLogicDTO>();
        serviceCollection.AddTransient<ManagerLogicDTO>();
        serviceCollection.AddTransient<AdministratorLogicDTO>();
        serviceCollection.AddTransient<InvitationLogicDTO>();
        serviceCollection.AddTransient<CompanyAdminLogicDTO>();
        serviceCollection.AddTransient<ApartmentLogicDTO>();
        serviceCollection.AddTransient<BuildingLogicDTO>();
    }
    public static void RegisterDataAccess(IServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContext<DbContext, BuildingManagerContext>();
        serviceCollection.AddTransient<IGenericRepository<Administrator>, AdministratorRepository>();
        serviceCollection.AddTransient<IGenericRepository<Staff>, StaffRepository>();
        serviceCollection.AddTransient<IGenericRepository<Manager>, ManagerRepository>();
        serviceCollection.AddTransient<IGenericRepository<Invitation>, InvitationRepository>();
        serviceCollection.AddTransient<IGenericRepository<Company>, CompanyRepository>();
        serviceCollection.AddTransient<IGenericRepository<Category>, CategoryRepository>();
        serviceCollection.AddTransient<IGenericRepository<Owner>, OwnerRepository>();
        serviceCollection.AddTransient<IGenericRepository<Building>, BuildingRepository>();
        serviceCollection.AddTransient<IGenericRepository<Apartment>, ApartmentRepository>();
        serviceCollection.AddTransient<IGenericRepository<Task>, TaskRepository>();
        serviceCollection.AddScoped<IGenericRepository<Session>, SessionRepository>();
        serviceCollection.AddTransient<IUserRepository<User>, UserRepository>();
        serviceCollection.AddTransient<IGenericRepository<CompanyAdmin>, CompanyAdminRepository>();


    }
}

