namespace ControlNode.DCS.Core.Exceptions
{
    public static class DCSCoreExceptionMessages
    {
        public static readonly string UnhandledException = "Unhandled exception occurred. More details: '{0}'";
        public static readonly string ParentJobFailed = "Parent job marked as failed as one of the atomic jobs failed.";
        public static readonly string UnhandledExceptionRetry = "Retrying on exception. Retry num: {0}. More details: '{1}'";
    }
}
