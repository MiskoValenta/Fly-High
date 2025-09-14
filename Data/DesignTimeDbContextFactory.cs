using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FlyHigh.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<VolleyballDbContext>
    {
        public VolleyballDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<VolleyballDbContext>();
            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new VolleyballDbContext(builder.Options);
        }
    }
}