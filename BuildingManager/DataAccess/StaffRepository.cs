using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class StaffRepository : GenericRepository<Staff>
    {
        public StaffRepository(DbContext context)
        {
            Context = context;
        }
    }
}
