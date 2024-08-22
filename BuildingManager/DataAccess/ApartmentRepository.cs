using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class ApartmentRepository : GenericRepository<Apartment>
{
    public ApartmentRepository(DbContext context)
    {
        Context = context;
    }
}