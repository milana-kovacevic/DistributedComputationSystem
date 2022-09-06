using Frontend.Models;

namespace Frontend.Extensions
{
    /// <summary>
    /// Extension methods for Job and its related objects.
    /// </summary>
    public static class JobExtension
    {
        public static bool IsActive(this JobResult jobResult)
        {
            return jobResult.State == JobState.Pending
                || jobResult.State == JobState.Queued
                || jobResult.State == JobState.InProgress;
        }
    }
}
