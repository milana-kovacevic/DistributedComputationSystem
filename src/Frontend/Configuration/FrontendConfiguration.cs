using Microsoft.Extensions.Configuration;

namespace Frontend.Configuration
{
    public class FrontendConfiguration : IFrontendConfiguration
    {
        private readonly IConfiguration config;
        private readonly Lazy<DatabaseSettings> dbSettings;
        private readonly Lazy<AuthenticationSettings> authSettings;

        public FrontendConfiguration(IConfiguration configuration)
        {
            config = configuration;

            dbSettings = new Lazy<DatabaseSettings>(() =>
            {
                var dbSettings = new DatabaseSettings();
                config.GetSection(DatabaseSettings.Database).Bind(dbSettings);
                return dbSettings;
            });

            authSettings = new Lazy<AuthenticationSettings>(() =>
            {
                var authSettings = new AuthenticationSettings();
                config.GetSection(AuthenticationSettings.Auth).Bind(authSettings);
                return authSettings;
            });
        }

        DatabaseSettings IFrontendConfiguration.DbSettings => this.dbSettings.Value;

        AuthenticationSettings IFrontendConfiguration.AuthSettings => this.authSettings.Value;
    }
}
