using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class OwnerRepository : GenericRepository<Owner>
    {
        public OwnerRepository(DbContext context)
        {
            Context = context;
        }
    }
}
