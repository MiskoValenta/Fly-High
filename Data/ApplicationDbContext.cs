using FlyHigh.Models;
using Microsoft.EntityFrameworkCore;

namespace FlyHigh.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Anket> Ankets { get; set; }

        // Každý "DbSet<>" je jedna tabulka v databázi
    }
}
