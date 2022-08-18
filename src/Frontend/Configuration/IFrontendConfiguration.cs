namespace Frontend.Configuration
{
    public interface IFrontendConfiguration
    {
        public DatabaseSettings DbSettings { get; }
        public AuthenticationSettings AuthSettings { get; }
    }
}
