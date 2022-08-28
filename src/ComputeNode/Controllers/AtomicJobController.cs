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
        private readonly IJobExecutor _executor;

        public AtomicJobController(
            ILogger<AtomicJobController> logger,
            IJobExecutor executor)
        {
            _logger = logger;
            _executor = executor;
        }

        [HttpGet("All")]
        public IEnumerable<AtomicJob> GetRunningJobs()
        {
            return Enumerable.Range(1, 2).Select(index => new AtomicJob()
            {
                StartTime = DateTime.Now.AddDays(index),
                EndTime = null,
                State = AtomicJobState.NotRan
            })
            .ToArray();
        }

        [HttpPost("Run")]
        public async Task<AtomicJobResult> PostJob(int atomicJobId, int parentJobId, [FromBody] string inputData)
        {
            _logger.LogInformation($"Executing job with id {parentJobId}, atomic job id: {atomicJobId}");

            var newJob = new AtomicJob()
            {
                Id = atomicJobId,
                ParentJobId = parentJobId,
                Data = inputData,
                StartTime = DateTime.UtcNow,
                State = AtomicJobState.NotRan
            };
            
            // Run job.
            return await _executor.ExecuteAsync(newJob);
        }
    }
}
