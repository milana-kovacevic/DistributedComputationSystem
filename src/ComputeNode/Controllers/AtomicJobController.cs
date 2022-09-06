using ComputeNode.Executor;
using ComputeNode.Models;
using Microsoft.AspNetCore.Mvc;

namespace ComputeNode.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AtomicJobController : Controller
    {
        private readonly ILogger<AtomicJobController> _logger;
        private readonly IAtomicJobExecutor _executor;

        public AtomicJobController(
            ILogger<AtomicJobController> logger,
            IAtomicJobExecutor executor)
        {
            _logger = logger;
            _executor = executor;
        }

        [HttpPost("Run")]
        public async Task<AtomicJobResult> PostJob(
            int atomicJobId,
            int parentJobId,
            [FromBody] string inputData,
            AtomicJobType jobType = AtomicJobType.CalculateSumOfDigits)
        {
            _logger.LogInformation($"Received request for execution: {parentJobId}:{atomicJobId}");

            var newJob = new AtomicJob()
            {
                Id = atomicJobId,
                ParentJobId = parentJobId,
                JobType = jobType,
                InputData = inputData,
                AtomicJobResult = new AtomicJobResult()
                {
                    State = AtomicJobState.NotRan,
                    StartTime = DateTime.UtcNow
                }
            };
            
            // Run job.
            return await _executor.ExecuteAsync(newJob);
        }
    }
}
