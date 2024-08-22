using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class AdministratorRepository : GenericRepository<Administrator>
    {
        public AdministratorRepository(DbContext context)
        {
            Context = context;
        }
    }
}
