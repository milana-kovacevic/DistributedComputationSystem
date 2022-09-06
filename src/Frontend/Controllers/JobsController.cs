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

            // TODO fetch result as forgein key
            var jobResults = await _context.JobResult.ToListAsync();
            var jobs = await _context.Job.ToListAsync();
            foreach (var job in jobs)
            {
                job.JobResult = jobResults.SingleOrDefault(jr => job.Id == jr.JobId);
            }

            return jobs;
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

            // TODO fetch result as forgein key
            var jobResult = await _context.JobResult.FindAsync(id);
            job.JobResult = jobResult;

            return job;
        }

        // POST: api/Jobs/Create
        [HttpPost("Create")]
        [ProducesResponseTypeAttribute(typeof(Job), StatusCodes.Status202Accepted)]
        public async Task<ActionResult<Job>> PostJob([FromBody] JobRequestData inputData)
        {
            if (_context.Job == null)
            {
                return Problem("Entity set 'JobsContext.Job'  is null.");
            }

            if (inputData == null || inputData.InputData == null || !inputData.InputData.Any())
            {
                return BadRequest(ExceptionMessages.InputDataNotProvided);
            }

            var newJob = new Job(
                inputData.JobType, 
                inputData.InputData.Select(ajrd => new AtomicJob() { InputData = ajrd.InputData }).ToList());

            // Using added job as id is auto-populated.
            var addedJob = _context.Job.Add(newJob);

            await _context.SaveChangesAsync();

            if (!_jobManager.TryAddJob(addedJob.Entity))
            {
                _logger.LogError("Failed to add new job to queue");
                return Problem(ExceptionMessages.SystemTooBusy, statusCode: (int)StatusCodes.Status503ServiceUnavailable);
            }

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

            // Only allow to cancel pending and in progress jobs.
            // TODO BUG job result fetching
            if (!job.JobResult.IsActive())
            {
                return BadRequest($"Job is not active so it cannot be cancelled. Job state: {job.State}.");
            }

            // Trying to cancel the job.
            // TODO make this nicer.
            job.JobResult.State = JobState.PendingCancellation;
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
