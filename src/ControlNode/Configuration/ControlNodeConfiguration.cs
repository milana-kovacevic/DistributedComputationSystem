namespace ControlNode.Configuration
{
    public class ControlNodeConfiguration : IControlNodeConfiguration
    {
        private readonly IConfiguration config;
        private readonly Lazy<AuthenticationSettings> authSettings;
        private readonly Lazy<DatabaseSettings> dbSettings;

        public ControlNodeConfiguration(IConfiguration configuration)
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

        DatabaseSettings IControlNodeConfiguration.DbSettings => this.dbSettings.Value;

        AuthenticationSettings IControlNodeConfiguration.AuthSettings => this.authSettings.Value;
    }
}
