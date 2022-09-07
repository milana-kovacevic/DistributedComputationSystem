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

        // Dictionary for tracking running jobs.
        public static ConcurrentDictionary<int, JobDetails> _inProgressTasks { get; } = new();

        public JobExecutionMonitor(
            ILogger<JobExecutionMonitor> logger,
            DbEntityManager dbEntityManager)
        {
            _logger = logger;
            _dbEntityManager = dbEntityManager;
        }

        // Updates the internal state which tracks in progress atomic jobs.
        internal void NotifyAtomicJobCompletion(int jobId, int atomicJobId, AtomicJobResult atomicJobResult)
        {
            // If atomic job failed, mark parent job as failed as well.
            if (atomicJobResult.State == AtomicJobState.Failed)
            {
                _dbEntityManager.UpdateJobState(jobId, JobState.Failed, error: ExceptionMessages.ParentJobFailed);
            }

            if (_inProgressTasks.TryGetValue(jobId, out var jobDetails))
            {
                _logger.LogInformation($"Marking atomic job as completed: {jobId} : {atomicJobId}");
                
                int remainingAtomicJobs = jobDetails.MarkAtomicJobCompleted(atomicJobId, atomicJobResult);

                // If that was the last job in the dictionary, update the state of the parent job.
                // Job is succeeded if all atomic jobs passed.
                // If some atomic job passed, the value of the state of the job should be already set to Failed.
                //
                if (remainingAtomicJobs == 0)
                {
                    JobState endResult = _dbEntityManager.UpdateJobStateToSuccessIfNotFailed(jobId, JobState.Succeeded, $"{jobDetails.AggregatedJobResult}");

                    _logger.LogInformation($"Parent job {jobId} completed. End result: {endResult}");
                }
            }
        }

        internal void AddJob(int jobId, int totalNumberAfAtomicJobs)
        {
            _inProgressTasks.TryAdd(jobId, new JobDetails(jobId, totalNumberAfAtomicJobs));
        }

        internal void AddAtomicJob(int jobId, int atomicJobId, AtomicJobResult atomicJobResult)
        {
            if (_inProgressTasks.TryGetValue(jobId, out var jobDetails))
            {
                jobDetails.TryAddAtomicJob(atomicJobId, atomicJobResult);

                _logger.LogInformation($"Added job to dictionary: {jobId} : {atomicJobId}");
            }
        }

        public IEnumerable<int> GetMonitoredJobs()
        {
            return _inProgressTasks.Keys;
        }
    }
}
