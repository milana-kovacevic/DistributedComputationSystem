using Frontend.Configuration;
using Frontend.Providers;

namespace IntegrationTests
{
    /// <summary>
    /// Connection string provider for test database running on local machine.
    /// </summary>
    internal class LocalSqlDbConnectionStringProvider : IConnectionStringProvider
    {
        private readonly DatabaseSettings _dbSettings;

        public LocalSqlDbConnectionStringProvider()
        {
            // UNDONE
            // TODO use test database and test db connectionProvider
            _dbSettings = new DatabaseSettings()
            {
                ServerName = "distr-computation-server.database.windows.net",
                DbName = "distr-computation-db",
                Port = 1433,
                SqlAdminUsername = "defaultAdmin",
                SqlAdminPassword = ""
            };
        }

        public string GetConnectionString()
        {
            return $"Server=tcp:{_dbSettings.ServerName},{_dbSettings.Port};Initial Catalog={_dbSettings.DbName};Persist Security Info=False;User ID={_dbSettings.SqlAdminUsername};Password={_dbSettings.SqlAdminPassword};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        }
    }
}