namespace ComputeNode.Exceptions
{
    public static class ExceptionMessages
    {
        public static readonly string InvalidInputData = "Unable to calculate sum of digits for input: '{0}'";
        public static readonly string UnhandledException = "Unhandled exception occurred for job {0}:{1}. More details: '{2}'";
        public static readonly string NonexistentSpecificExecutor = "Specific job executor not defined for Atomic job type: '{0}'";
    }
}
