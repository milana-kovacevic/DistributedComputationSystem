using Frontend.Configuration;
using Frontend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobController : ControllerBase
    {
        private readonly ILogger<JobController> _logger;
        private readonly IFrontendConfiguration config;

        public JobController(ILogger<JobController> logger, IFrontendConfiguration feConfig)
        {
            _logger = logger;
            config = feConfig;
        }

        [HttpGet(Name = "GetJobs")]
        public IEnumerable<Job> Get()
        {
            _logger.LogInformation("Getting jobs");
            //_logger.LogInformation($"Auth enabled: {config.AuthSettings.AuthEnabled}");

            return Enumerable.Range(1, 2).Select(index => new Job
            {
                StartTime = DateTime.UtcNow,
                EndTime = null,
                State = JobState.Succeeded
            })
            .ToArray();
        }
    }
}