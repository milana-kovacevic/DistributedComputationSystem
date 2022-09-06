using Frontend.Data;
using Frontend.Exceptions;
using Frontend.Models;
using System.Collections.Concurrent;

namespace Frontend.Engine
{
    public class JobExecutionMonitor
    {
        private readonly ILogger<JobExecutionMonitor> _logger;
        private DbEntityManager _dbEntityManager;

        // Dictionary for tracking running atomic jobs.
        public static ConcurrentDictionary<int, ConcurrentDictionary<int, AtomicJobResult>> _inProgressTasks { get; } = new();


        public JobExecutionMonitor(
            ILogger<JobExecutionMonitor> logger,
            DbEntityManager dbEntityManager)
        {
            _logger = logger;
            _dbEntityManager = dbEntityManager;
        }

        // Updates the internal state which tracks in progress atomic jobs.
        internal void NotifyAtomicJobCompletion(int jobId, int atomicJobId, AtomicJobState atomicJobState)
        {
            // If atomic job failed, mark parent job as failed as well.
            if (atomicJobState == AtomicJobState.Failed)
            {
                _dbEntityManager.UpdateJobState(jobId, JobState.Failed, error: ExceptionMessages.ParentJobFailed);
            }

            if (_inProgressTasks.TryGetValue(jobId, out var jobDictionary))
            {
                _logger.LogInformation($"Removing atomic job from dictionary: {jobId} : {atomicJobId}");
                jobDictionary.Remove(atomicJobId, out _);

                // If that was the last job in the dictionary, update the state of the parent job.
                // Job is succeeded if all atomic jobs passed.
                // If some atomic job passed, the value of the state of the job should be already set to Failed.
                //
                // TODO !!! track the number of pending atomic jobs separately - this is not correct
                if (jobDictionary.Count == 0)
                {
                    JobState endResult = _dbEntityManager.UpdateJobStateToSuccessIfNotFailed(jobId, JobState.Succeeded);

                    _logger.LogInformation($"Parent job {jobId} completed. End result: {endResult}");
                }
            }
        }

        internal void AddJob(int jobId)
        {
            _inProgressTasks.TryAdd(jobId, new ConcurrentDictionary<int, AtomicJobResult>());
        }

        internal void AddAtomicJob(int jobId, int atomicJobId, AtomicJobResult atomicJobResult)
        {
            if (_inProgressTasks.TryGetValue(jobId, out var jobDictionary))
            {
                jobDictionary.TryAdd(atomicJobId, atomicJobResult);

                _logger.LogInformation($"Added job to dictionary: {jobId} : {atomicJobId}");
            }
        }

        public IEnumerable<int> GetMonitoredJobs()
        {
            return _inProgressTasks.Keys;
        }
    }
}
