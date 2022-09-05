using ComputeNodeSwaggerClient;
using Frontend.ComputeNodeSwaggerClient;
using Frontend.Exceptions;
using Frontend.Mappers;
using Frontend.Models;

namespace Frontend.Engine
{
    public class AtomicJobScheduler : IAtomicJobScheduler
    {
        private readonly ILogger<AtomicJobScheduler> _logger;
        private IComputeNodeClientWrapper _computeNodeClientWrapper;

        public AtomicJobScheduler(
            ILogger<AtomicJobScheduler> logger,
            IComputeNodeClientWrapper computeNodeClientWrapper)
        {
            _logger = logger;
            this._computeNodeClientWrapper = computeNodeClientWrapper;
        }

        public async Task<AtomicJobResult> ScheduleAsync(AtomicJob job)
        {
            try
            {
                ComputeNodeSwaggerClient.AtomicJobResult result = _computeNodeClientWrapper.RunAsync(
                        job.AtomicJobId, // TODO
                        job.JobId,
                        AtomicJobTypeMapper.Map(job.JobType),
                        job.InputData);
                _logger.LogInformation($"JobId {0}; AtomicJobId {job.JobId}; ResultState: {result.Status}; Result {result.Result}; Error {result.Error}");

            }
            catch (Exception e)
            {
                var errorMessage = string.Format(ExceptionMessages.UnhandledException, e.Message);
                _logger.LogError(e, errorMessage);

                return new AtomicJobResult()
                {
                    AtomicJobId = job.AtomicJobId,
                    JobId = job.JobId,
                    Error = errorMessage,
                    State = AtomicJobState.Failed
                };
            }
        }
    }
}
