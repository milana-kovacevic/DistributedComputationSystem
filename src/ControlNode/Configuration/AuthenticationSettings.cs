namespace ControlNode.Configuration
{
    public class AuthenticationSettings
    {
        public const string Auth = "Authentication";

        public bool AuthEnabled { get; set; } = false;
        public string TenantId { get; set; } = String.Empty;
        public string ClientId { get; set; } = String.Empty;
        public string AuthServer { get; set; } = String.Empty;
        public string Issuer { get; set; } = String.Empty;
        public string Audience { get; set; } = String.Empty;
    }
}
