using Frontend.Configuration;

namespace Frontend.Providers
{
    public class AzureSqlDbConnectionStringProvider : IConnectionStringProvider
    {
        private readonly DatabaseSettings dbSettings;

        public AzureSqlDbConnectionStringProvider(IConfiguration config)
        {
            dbSettings = new DatabaseSettings();
            config.GetSection(DatabaseSettings.Database).Bind(dbSettings);
        }

        public string GetConnectionString()
        {
            return $"Server=tcp:{dbSettings.ServerName},{dbSettings.Port};Initial Catalog={dbSettings.DbName};Persist Security Info=False;User ID={dbSettings.SqlAdminUsername};Password={dbSettings.SqlAdminPassword};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        }
    }
}
