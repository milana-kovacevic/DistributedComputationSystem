namespace ControlNode.DCS.Core.Exceptions
{
    public class JobExecutorException : Exception
    {
        private const string MessageFormat = "Exception happened during execution on ComputeNode. '{0}'.";

        public JobExecutorException(string message)
            : base(string.Format(MessageFormat, message))
        {
        }
    }
}
