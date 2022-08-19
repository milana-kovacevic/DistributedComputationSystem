using Microsoft.AspNetCore.Mvc;
using Frontend.Models;
using Frontend.Data;

namespace Frontend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobResultsController : ControllerBase
    {
        private readonly JobContext _context;

        public JobResultsController(JobContext context)
        {
            _context = context;
        }

        // GET: api/JobResults/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JobResult>> GetJobResult(int id)
        {
            if (_context.JobResult == null)
            {
                return NotFound();
            }
            var jobResult = await _context.JobResult.FindAsync(id);

            if (jobResult == null)
            {
                return NotFound();
            }

            return jobResult;
        }
    }
}
