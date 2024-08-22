using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class CategoryRepository : GenericRepository<Category>
    {

        public CategoryRepository(DbContext context)
        {
            Context = context;
        }

    }
}
