namespace Frontend.Exceptions
{
    public class NetworkException : Exception
    {
        private const string MessageFormat = "Unable to locate address for service '{0}'.";

        public NetworkException(string message)
            : base(string.Format(MessageFormat, message))
        {
        }
    }
}