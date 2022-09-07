using Frontend.Data;
using Frontend.Mappers;
using Frontend.Models;

namespace Frontend.Engine
{
    public class DistributedScheduler : IScheduler
    {
        private readonly ILogger<DistributedScheduler> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IAtomicJobScheduler _atomicJobScheduler;
        private JobExecutionMonitor _jobExecutionMonitor;
        private DbEntityManager _dbEntityManager;

        // Dictionary for tracking running jobs.
        //public static ConcurrentDictionary<int, Job> _inProgressTasks { get; } = new();

        public DistributedScheduler(
            ILogger<DistributedScheduler> logger,
            IServiceProvider serviceProvider,
            IAtomicJobScheduler atomicJobScheduler,
            JobExecutionMonitor jobExecutionMonitor,
            DbEntityManager dbEntityManager)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _atomicJobScheduler = atomicJobScheduler;
            _jobExecutionMonitor = jobExecutionMonitor;
            _dbEntityManager = dbEntityManager;
        }

        public static void Initialize(string connectionString)
        {
            Console.WriteLine("Starting distributed scheduler...");
        }

        public async Task ScheduleJobAsync(Job job)
        {
            try
            {
                _logger.LogInformation($"Scheduling job with id {job.JobId}...");

                // Add parent job to list in progress tasks.
                _jobExecutionMonitor.AddJob(job.JobId, job.AtomicJobs.Count);

                // Setup initial state for aggregated result stored in parent job.
                // UNDONE: nice handling depending on the job type.
                job.JobResult.Result = string.Empty;

                // Update parent job state.
                _dbEntityManager.UpdateJobState(job.JobId, newState: JobState.InProgress);

                // NOT DONE:
                // [ADVANCED] use custom routing to available ComputeNodes using ingress setup.
                // [ADVANCED] Generate list of atomic jobs when input is more complex.

                // Trigger asyncly all atomic jobs.
                var result = new List<AtomicJobResult>();
                foreach (var atomicJobUnit in job.AtomicJobs)
                {
                    await _atomicJobScheduler.ScheduleAsync(atomicJobUnit);
                }
                
                _logger.LogInformation($"Scheduled job with id {job.JobId}");
                
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to schedule job with id {job.JobId}");
                _dbEntityManager.UpdateJobState(job.JobId, newState: JobState.Failed);
            }
        }

        public IEnumerable<Job> GetInProgressTasks()
        {
            //return _jobExecutionMonitor.GetMonitoredJobs();
            // TODO
            return new List<Job>();
        }
    }
}
