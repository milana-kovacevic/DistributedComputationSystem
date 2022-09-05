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
                var specificJobExecutor = await _specificJobExecutorFactory.BuildAsync(atomicJob.JobType);

                return await specificJobExecutor.ExecuteAsync(atomicJob);

            }
            catch (Exception e)
            {
                var errorMessage = string.Format(ExceptionMessages.UnhandledException, e.Message);
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
