using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Frontend.Data;
using Frontend.Models;
using Frontend.Managers;
using Frontend.Extensions;
using Frontend.Exceptions;

namespace Frontend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class JobsController : ControllerBase
    {
        private readonly ILogger<JobsController> _logger;
        private readonly JobContext _context;
        private readonly IJobManager _jobManager;

        public JobsController(
            ILogger<JobsController> logger,
            JobContext context,
            IJobManager jobManager)
        {
            _logger = logger;
            _context = context;
            _jobManager = jobManager;
        }

        // GET: api/Jobs/All
        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<Job>>> GetJob()
        {
          if (_context.Job == null)
          {
              return NotFound();
          }
            return await _context.Job.ToListAsync();
        }

        // GET: api/Jobs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Job>> GetJob(int id)
        {
          if (_context.Job == null)
          {
              return NotFound();
          }
            var job = await _context.Job.FindAsync(id);

            if (job == null)
            {
                return NotFound();
            }

            return job;
        }

        // POST: api/Jobs/Create
        [HttpPost("Create")]
        public async Task<ActionResult<Job>> PostJob([FromBody] string inputData)
        {
            if (_context.Job == null)
            {
                return Problem("Entity set 'JobsContext.Job'  is null.");
            }

            var newJob = new Job()
            {
                Data = inputData,
                StartTime = DateTime.UtcNow,
                State = JobState.Pending
            };

            // Using added job as id is auto-populated.
            var addedJob = _context.Job.Add(newJob);

            if (!_jobManager.TryAddJob(addedJob.Entity))
            {
                _logger.LogError("Failed to add new job to queue");
                return Problem(ExceptionMessages.SystemTooBusy, statusCode: (int)StatusCodes.Status503ServiceUnavailable);
            }

            await _context.SaveChangesAsync();

            return AcceptedAtAction("GetJob", new { id = addedJob.Entity.Id }, addedJob.Entity);
        }

        // POST: api/Jobs/Cancel/5
        [HttpPost("Cancel/{id}")]
        public async Task<ActionResult<Job>> CancelJob(int id)
        {
            if (_context.Job == null)
            {
                return NotFound();
            }

            var job = await _context.Job.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            // Only allow to cancel pending and inprogress jobs.
            if (!job.IsActive())
            {
                return BadRequest($"Job is not active so it cannot be cancelled. Job state: {job.State}.");
            }

            // Trying to cancel the job.
            // TODO make this nicer.
            job.State = JobState.PendingCancellation;
            var updatedJob = _context.Job.Update(job);
            await _jobManager.CancelJobAsync(job.Id);

            await _context.SaveChangesAsync();

            return AcceptedAtAction("GetJob", new { id = updatedJob.Entity.Id }, updatedJob.Entity);
        }

        // DELETE: api/Jobs/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            if (_context.Job == null)
            {
                return NotFound();
            }
            var job = await _context.Job.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            _context.Job.Remove(job);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool JobExists(int id)
        {
            return (_context.Job?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
