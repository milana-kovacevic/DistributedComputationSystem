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
            return job.State == JobState.Pending || job.State == JobState.InProgress;
        }
    }
}
