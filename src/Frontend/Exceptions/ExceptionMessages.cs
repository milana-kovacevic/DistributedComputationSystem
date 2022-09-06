namespace Frontend.Exceptions
{
    public static class ExceptionMessages
    {
        public static readonly string SystemTooBusy = "System is too busy to accept new job requests. Try again later.";
        public static readonly string InputDataNotProvided = "InputData is not provided.";
        public static readonly string UnhandledException = "Unhandled exception occurred. More details: '{0}'";
        public static readonly string ParentJobFailed = "Parent job marked as failed as one of the atomic jobs failed.";
    }
}
