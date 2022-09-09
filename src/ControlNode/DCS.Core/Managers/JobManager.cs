using ControlNode.DCS.Core.Engine;
using ControlNode.Frontend.Data;
using ControlNode.Frontend.Models;

namespace ControlNode.DCS.Core.Managers
{
    /// <summary>
    /// Class that manages jobs submitted for execution.
    /// Also manages job cancellation.
    /// </summary>
    public class JobManager : IJobManager
    {
        private readonly JobQueue _jobQueue;
        private DbEntityManager _dbEntityManager;

        public JobManager(JobQueue jobQueue, DbEntityManager dbEntityManager)
        {
            _jobQueue = jobQueue;
            _dbEntityManager = dbEntityManager;
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
            if (_jobQueue.TryEnqueueJob(job))
            {
                _dbEntityManager.UpdateJobState(job.JobId, newState: JobState.Queued);

                return true;
            }

            return false;
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
