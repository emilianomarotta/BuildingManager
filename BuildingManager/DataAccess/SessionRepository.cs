using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class SessionRepository : GenericRepository<Session>
    {
        public SessionRepository(DbContext context)
        {
            Context = context;
        }
    }
}

