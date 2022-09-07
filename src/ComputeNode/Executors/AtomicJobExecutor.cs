using ComputeNode.Controllers;
using ComputeNode.Exceptions;
using ComputeNode.Executors;
using ComputeNode.Models;

namespace ComputeNode.Executor
{
    /// <summary>
    /// Atomic job executor.
    /// </summary>
    public class AtomicJobExecutor : IAtomicJobExecutor
    {
        private readonly ILogger<AtomicJobExecutor> _logger;
        private readonly ISpecificJobExecutorFactory _specificJobExecutorFactory;

        public AtomicJobExecutor(ILogger<AtomicJobExecutor> logger, ISpecificJobExecutorFactory specificJobExecutorFactory)
        {
            _logger = logger;
            _specificJobExecutorFactory = specificJobExecutorFactory;
        }

        public async Task<AtomicJobResult> ExecuteAsync(AtomicJob atomicJob) 
        {
            try
            {
                _logger.LogInformation($"Executing AtomicJob: {atomicJob.ParentJobId}:{atomicJob.Id}");

                var specificJobExecutor = await _specificJobExecutorFactory.BuildAsync(atomicJob.JobType);
                var result =  await specificJobExecutor.ExecuteAsync(atomicJob);

                _logger.LogInformation($"Completed execution for AtomicJob: {atomicJob.ParentJobId}:{atomicJob.Id}");

                return result;
            }
            catch (Exception e)
            {
                var errorMessage = string.Format(ExceptionMessages.UnhandledException, atomicJob.ParentJobId, atomicJob.Id, e.Message);
                _logger.LogError(e, errorMessage);

                return new AtomicJobResult()
                {
                    Id = atomicJob.Id,
                    ParentJobId = atomicJob.ParentJobId,
                    Error = errorMessage,
                    State = AtomicJobState.Failed
                };
            }
        }
    }
}
