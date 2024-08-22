using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class BuildingRepository : GenericRepository<Building>
    {
        public BuildingRepository(DbContext context)
        {
            Context = context;
        }
    }
}
