using ControlNode.Frontend.Data;
using ControlNode.Frontend.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControlNode.Frontend.Controllers
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
