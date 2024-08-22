using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class CompanyAdminRepository : GenericRepository<CompanyAdmin>
    {
        public CompanyAdminRepository(DbContext context)
        {
            Context = context;
        }
    }
}
