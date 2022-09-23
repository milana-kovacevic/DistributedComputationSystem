using ControlNode.DCS.Core.ComputeNodeSwaggerClient;
using ControlNode.DCS.Core.Exceptions;
using ControlNode.Abstraction.Models;

namespace ControlNode.DCS.Core.Engine
{
    public class ComputeNodeJobExecutor
    {
        private readonly ILogger<ComputeNodeJobExecutor> _logger;
        private readonly IServiceProvider _serviceProvider;
        private IComputeNodeClientWrapper _computeNodeClientWrapper;
        private readonly int RetryCountMax = 3;

        public ComputeNodeJobExecutor(
            ILogger<ComputeNodeJobExecutor> logger,
            IServiceProvider serviceProvider,
            IComputeNodeClientWrapper computeNodeClientWrapper)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _computeNodeClientWrapper = computeNodeClientWrapper;
        }

        public async Task<AtomicJobResult> ExecuteWithRetryAsync(AtomicJob atomicJob)
        {
            for (int tryCount = 0; ; tryCount++)
            {
                try
                {
                    // Run the job
                    var result = await _computeNodeClientWrapper.RunAsync(
                        atomicJob.AtomicJobId,
                        atomicJob.JobId,
                        atomicJob.JobType,
                        atomicJob.InputData);

                    if (result.State == AtomicJobState.Failed && result.Error.Contains("Unhandled exception"))
                    {
                        throw new JobExecutorException($"Retry {tryCount}");
                    }

                    return result;
                }
                catch (Exception e)
                {
                    if (tryCount == RetryCountMax)
                    {
                        throw;
                    }

                    var errorMessage = string.Format(DCSCoreExceptionMessages.UnhandledExceptionRetry, tryCount, e.Message);
                    _logger.LogError(e, errorMessage);
                }
            }
        }
    }
}
