using Frontend.Engine;
using Frontend.Models;

namespace Frontend.Managers
{
    /// <summary>
    /// Class that manages jobs submitted for execution.
    /// Also manages job cancellation.
    /// </summary>
    public class JobManager : IJobManager
    {
        private static readonly JobQueue jobQueue = new JobQueue();

        /// <summary>
        /// Initializes queue with unfinished jobs from the database.
        /// </summary>
        /// <param name="jobs">Jobs from the db.</param>
        public static void Initialize(IEnumerable<Job> jobs)
        {
            foreach (var job in jobs)
            {
                jobQueue.TryEnqueueJob(job);
            }
        }

        public bool TryAddJob(Job job)
        {
            return jobQueue.TryEnqueueJob(job);
        }

        /// <summary>
        /// Cancels job.
        /// </summary>
        /// <param name="id">Id of the job to be canceled.</param>
        public async Task CancelJobAsync(int id)
        {
            // TODO
            Task.Delay(100);
        }
    }
}
