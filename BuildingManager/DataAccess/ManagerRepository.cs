using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ManagerRepository : GenericRepository<Manager>
    {
        public ManagerRepository(DbContext context)
        {
            Context = context;
        }
    }
}
