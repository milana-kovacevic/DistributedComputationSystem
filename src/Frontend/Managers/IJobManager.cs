using Frontend.Models;

namespace Frontend.Managers
{
    public interface IJobManager
    {
        /// <summary>
        /// Initializes jobs.
        /// </summary>
        /// <param name="jobs">Jobs to add to queue.</param>
        public void Initialize(IEnumerable<Job> jobs);

        /// <summary>
        /// Tries to add job to the queue.
        /// </summary>
        bool TryAddJob(Job job);

        /// <summary>
        /// Cancels job.
        /// </summary>
        /// <param name="id">Id of the job to be canceled.</param>
        Task CancelJobAsync(int id);

        /// <summary>
        /// Returns number of jobs.
        /// </summary>
        public int GetNumberOfJobs();
    }
}
