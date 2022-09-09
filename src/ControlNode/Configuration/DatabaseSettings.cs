namespace ControlNode.Configuration
{
    public class DatabaseSettings
    {
        public const string Database = "Database";

        public string ServerName { get; set; } = String.Empty;
        public string DbName { get; set; } = String.Empty;
        public string SqlAdminUsername { get; set; } = String.Empty;
        public string SqlAdminPassword { get; set; } = String.Empty;
        public int Port { get; set; } = -1;
    }
}
