using Frontend.Configuration;

namespace Frontend.Providers
{
    public class AzureSqlDbConnectionStringProvider : IConnectionStringProvider
    {
        private readonly DatabaseSettings _dbSettings;

        public AzureSqlDbConnectionStringProvider(IConfiguration config)
        {
            _dbSettings = new DatabaseSettings();
            config.GetSection(DatabaseSettings.Database).Bind(_dbSettings);
        }

        public AzureSqlDbConnectionStringProvider(DatabaseSettings dbSettings)
        {
            _dbSettings = dbSettings;
        }

        public string GetConnectionString()
        {
            return $"Server=tcp:{_dbSettings.ServerName},{_dbSettings.Port};Initial Catalog={_dbSettings.DbName};Persist Security Info=False;User ID={_dbSettings.SqlAdminUsername};Password={_dbSettings.SqlAdminPassword};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        }
    }
}
