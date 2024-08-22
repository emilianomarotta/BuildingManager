using Microsoft.EntityFrameworkCore;
using Task = Domain.Task;

namespace DataAccess
{
    public class TaskRepository : GenericRepository<Task>
    {
        public TaskRepository(DbContext context)
        {
            Context = context;
        }
    }
}

