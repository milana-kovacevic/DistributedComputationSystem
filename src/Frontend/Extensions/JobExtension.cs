using Frontend.Models;

namespace Frontend.Extensions
{
    /// <summary>
    /// Extension methods for Job object.
    /// </summary>
    public static class JobExtension
    {
        public static bool IsActive(this Job job)
        {
            return job.JobResult.State == JobState.Pending
                || job.JobResult.State == JobState.Queued
                || job.JobResult.State == JobState.InProgress;
        }
    }
}
