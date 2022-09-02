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
        private readonly JobQueue _jobQueue;

        public JobManager(JobQueue jobQueue)
        {
            this._jobQueue = jobQueue;
        }

        /// </inheritdoc>
        public void Initialize(IEnumerable<Job> jobs)
        {
            foreach (var job in jobs)
            {
                _jobQueue.TryEnqueueJob(job);
            }
        }

        /// </inheritdoc>
        public bool TryAddJob(Job job)
        {
            return _jobQueue.TryEnqueueJob(job);
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
            return _jobQueue.GetNumberOfJobs();
        }
    }
}
