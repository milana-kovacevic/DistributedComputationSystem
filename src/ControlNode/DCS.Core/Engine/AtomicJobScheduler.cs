using ControlNode.DCS.Core.ComputeNodeSwaggerClient;
using ControlNode.DCS.Core.Exceptions;
using ControlNode.Abstraction.Data;
using ControlNode.Abstraction.Models;

namespace ControlNode.DCS.Core.Engine
{
    public class AtomicJobScheduler : IAtomicJobScheduler
    {
        private readonly ILogger<AtomicJobScheduler> _logger;
        private readonly IServiceProvider _serviceProvider;
        private IComputeNodeClientWrapper _computeNodeClientWrapper;
        private JobExecutionMonitor _jobExecutionMonitor;
        private DbEntityManager _dbEntityManager;


        public AtomicJobScheduler(
            ILogger<AtomicJobScheduler> logger,
            IServiceProvider serviceProvider,
            IComputeNodeClientWrapper computeNodeClientWrapper,
            JobExecutionMonitor jobExecutionMonitor,
            DbEntityManager dbEntityManager)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _computeNodeClientWrapper = computeNodeClientWrapper;
            _jobExecutionMonitor = jobExecutionMonitor;
            _dbEntityManager = dbEntityManager;
        }

        public async Task ScheduleAsync(AtomicJob job)
        {
            // Use thread pool to asyncly run atomic jobs.
            ThreadPool.QueueUserWorkItem(new WaitCallback(async (obj) =>
            {
                AtomicJobResult result = new AtomicJobResult()
                {
                    AtomicJobId = job.AtomicJobId,
                    JobId = job.JobId,
                    StartTime = DateTime.UtcNow,
                    State = AtomicJobState.InProgress
                };

                // TODO Execute with retry
                try
                {
                    var unit = (AtomicJob)obj;
                    _logger.LogInformation($"Thread {Thread.CurrentThread.GetHashCode()} consumes JobId {unit.JobId}; AtomicJobId {unit.AtomicJobId}");

                    _jobExecutionMonitor.AddAtomicJob(unit.JobId, unit.AtomicJobId, result);

                    // Update atomic job state
                    _dbEntityManager.UpdateAtomicJobState(unit.JobId, unit.AtomicJobId, AtomicJobState.InProgress);

                    // Run the job
                    result = await _computeNodeClientWrapper.RunAsync(
                            job.AtomicJobId,
                            job.JobId,
                            job.JobType,
                            job.InputData);

                    // Update atomic job result in database.
                    _dbEntityManager.UpdateAtomicJobResult(unit.JobId, unit.AtomicJobId, result);

                    _logger.LogInformation($"Completed: JobId {unit.JobId}; AtomicJobId {unit.AtomicJobId}; ResultState: {result.State}");
                    
                    _jobExecutionMonitor.NotifyAtomicJobCompletion(unit.JobId, unit.AtomicJobId, result);
                }
                catch (Exception e)
                {
                    var errorMessage = string.Format(DCSCoreExceptionMessages.UnhandledException, e.Message);
                    _logger.LogError(e, errorMessage);

                    result = new AtomicJobResult()
                    {
                        AtomicJobId = job.AtomicJobId,
                        JobId = job.JobId,
                        Error = errorMessage,
                        State = AtomicJobState.Failed
                    };

                    _jobExecutionMonitor.NotifyAtomicJobCompletion(job.JobId, job.AtomicJobId, result);

                    _dbEntityManager.UpdateAtomicJobResult(job.JobId, job.AtomicJobId, result);
                }
            }),
            job);
        }
    }
}
