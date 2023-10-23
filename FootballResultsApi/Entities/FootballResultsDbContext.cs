using Microsoft.EntityFrameworkCore;

namespace FootballResultsApi.Entities
{
    public class FootballResultsDbContext : DbContext
    {
        public FootballResultsDbContext(DbContextOptions<FootballResultsDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
