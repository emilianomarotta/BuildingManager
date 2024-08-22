using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class CompanyRepository : GenericRepository<Company>
    {
        public CompanyRepository(DbContext context)
        {
            Context = context;
        }
    }
}
