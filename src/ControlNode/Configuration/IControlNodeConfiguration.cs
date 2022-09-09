namespace ControlNode.Configuration
{
    public interface IControlNodeConfiguration
    {
        public DatabaseSettings DbSettings { get; }
        public AuthenticationSettings AuthSettings { get; }
    }
}
