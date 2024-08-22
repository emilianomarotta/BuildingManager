using Microsoft.Extensions.DependencyInjection;
using BusinessLogic;
using Domain;
using IBusinessLogic;
using IDataAccess;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace ServicesFactory;

public class ServicesFactory
{
    public ServicesFactory() { }

    public static void RegisterServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IBusinessLogic<Administrator>, AdministratorLogic>();

        // Si manejo estado, uso addScoped
     //   serviceCollection.AddScoped<ISessionLogic, SessionLogic>();
    }
    public static void RegisterDataAccess(IServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContext<DbContext, BuildingManagerContext>();
        serviceCollection.AddTransient<IGenericRepository<Administrator>, AdministratorRepository>();
    }
}