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
        private readonly JobQueue jobQueue = new();

        /// </inheritdoc>
        public void Initialize(IEnumerable<Job> jobs)
        {
            foreach (var job in jobs)
            {
                jobQueue.TryEnqueueJob(job);
            }
        }

        /// </inheritdoc>
        public bool TryAddJob(Job job)
        {
            return jobQueue.TryEnqueueJob(job);
        }

        /// </inheritdoc>
        public async Task CancelJobAsync(int id)
        {
            // TODO
            await Task.Delay(100);
        }

        /// </inheritdoc>
        public int GetNumberOfJobs()
        {
            return jobQueue.GetNumberOfJobs();
        }
    }
}
