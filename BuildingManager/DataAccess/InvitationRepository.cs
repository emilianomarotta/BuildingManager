using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class InvitationRepository : GenericRepository<Invitation>
    {

        public InvitationRepository(DbContext context)
        {
            Context = context;
        }

    }
}
