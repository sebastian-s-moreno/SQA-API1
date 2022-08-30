using Forte.Location.DataAccess.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Forte.Location.DataAccess.Schema
{
    public class LocationDbContext : DbContext
    {
        public LocationDbContext(DbContextOptions<LocationDbContext> options)
            : base(options)
        {
            if (Database.IsSqlite())
            {
                Database.EnsureCreated();
            }
        }

        public DbSet<LocationEntity> Locations { get; set; }

    }
}
