using Frontend.Configuration;
using Frontend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestJobsController : ControllerBase
    {
        private readonly ILogger<TestJobsController> _logger;
        private readonly IFrontendConfiguration config;

        public TestJobsController(ILogger<TestJobsController> logger, IFrontendConfiguration feConfig)
        {
            _logger = logger;
            config = feConfig;
        }

        [HttpGet("All")]
        public IEnumerable<Job> Get()
        {
            _logger.LogInformation("Getting jobs");
            //_logger.LogInformation($"Auth enabled: {config.AuthSettings.AuthEnabled}");

            return Enumerable.Range(1, 2).Select(index => new Job
            {
                Id = 42,
                StartTime = DateTime.UtcNow,
                EndTime = null,
                State = JobState.Succeeded
            })
            .ToArray();
        }
    }
}