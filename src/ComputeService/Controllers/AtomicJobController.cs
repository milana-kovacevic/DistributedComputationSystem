using Microsoft.AspNetCore.Mvc;
using ComputeService.Models;
using System;

namespace ComputeService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AtomicJobController : Controller
    {
        private readonly ILogger<AtomicJobController> _logger;

        public AtomicJobController(ILogger<AtomicJobController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetAtomicJobs")]
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
    }
}
