using FrontendService.Models;
using Microsoft.AspNetCore.Mvc;

namespace FrontendService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobController : ControllerBase
    {
        private readonly ILogger<JobController> _logger;

        public JobController(ILogger<JobController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetJobs")]
        public IEnumerable<Job> Get()
        {
            return Enumerable.Range(1, 2).Select(index => new Job
            {
                StartTime = DateTime.UtcNow,
                EndTime = null,
                State = JobState.NotRan
            })
            .ToArray();
        }
    }
}