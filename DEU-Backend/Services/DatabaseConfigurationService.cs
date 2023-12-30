namespace DEU_Backend.Services
{
    public class DatabaseConfigurationService
    {
        private readonly IConfiguration _configuration;

        public DatabaseConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetDatabaseType() => _configuration.GetValue<string>("DatabaseConfig:DatabaseType") ?? "SQLite";

        public string GetConnectionString()
        {
            var dbType = GetDatabaseType();
            return dbType switch
            {
                "PostgreSQL" => _configuration.GetValue<string>("DatabaseConfig:ConnectionStrings:PostgreSQL") ?? throw new Exception("PostgreSQL connection string is not defined"),
                "SQLite" => _configuration.GetValue<string>("DatabaseConfig:ConnectionStrings:SQLite") ?? throw new Exception("SQLite connection string is not defined"),
                _ => throw new Exception("Database type is not defined")
            };
        }
    }
}
