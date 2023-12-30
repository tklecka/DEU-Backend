using DEU_Backend.Services;
using Microsoft.EntityFrameworkCore;

namespace DEU_Backend
{
    public class DeuDbContext : DbContext
    {
        private readonly DatabaseConfigurationService _dbConfigService;

        public DeuDbContext(DbContextOptions<DeuDbContext> options, DatabaseConfigurationService dbConfigService)
            : base(options)
        {
            _dbConfigService = dbConfigService;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbType = _dbConfigService.GetDatabaseType();
            var connectionString = _dbConfigService.GetConnectionString();

            if (dbType == "PostgreSQL")
            {
                optionsBuilder.UseNpgsql(connectionString);
            }
            else if (dbType == "SQLite")
            {
                optionsBuilder.UseSqlite(connectionString);
            }
        }

    }
}
