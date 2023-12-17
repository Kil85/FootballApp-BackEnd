using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FootballResultsApi.Entities
{
    public class FootballResultsDbContext : DbContext
    {
        public FootballResultsDbContext(DbContextOptions<FootballResultsDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Fixture> Fixtures { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<MetaData> MetaDatas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Fixture>()
                .HasOne(f => f.HomeTeam)
                .WithMany(t => t.Fixtures)
                .HasForeignKey(f => f.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Fixture>()
                .HasOne(f => f.AwayTeam)
                .WithMany()
                .HasForeignKey(f => f.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Fixture>()
                .HasOne(f => f.League)
                .WithMany(l => l.Fixtures)
                .HasForeignKey(f => f.LeagueId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<League>().Property(e => e.Id).ValueGeneratedNever();

            modelBuilder.Entity<Team>().Property(e => e.Id).ValueGeneratedNever();
            modelBuilder.Entity<Fixture>().Property(e => e.Id).ValueGeneratedNever();

            var converter = new ValueConverter<DateOnly, DateTime>(
                dateOnly => dateOnly.ToDateTime(new TimeOnly(0)),
                dateTime => DateOnly.FromDateTime(dateTime)
            );

            modelBuilder.Entity<MetaData>().Property(e => e.FixtureDate).HasConversion(converter);

            base.OnModelCreating(modelBuilder);
        }
    }
}
