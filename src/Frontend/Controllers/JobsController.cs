using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Frontend.Data;
using Frontend.Models;
using Frontend.DistributedOrchestrator;
using Frontend.Managers;
using Frontend.Extensions;

namespace Frontend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class JobsController : ControllerBase
    {
        private readonly JobContext _context;
        private readonly IJobManager _jobManager;

        public JobsController(JobContext context, IJobManager jobManager)
        {
            _context = context;
            _jobManager = jobManager;
        }

        // GET: api/Jobs
        [HttpGet]
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

        // POST: api/Jobs
        [HttpPost]
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
            await _context.SaveChangesAsync();
            _jobManager.AddNewJobToQueue(addedJob.Entity);

            return AcceptedAtAction("GetJob", new { id = addedJob.Entity.Id }, addedJob.Entity);
        }

        // POST: api/Jobs/Cancel/5
        [HttpPost]
        [Route("api/[controller]/Cancel")]
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
            _jobManager.CancelJob(job.Id);

            await _context.SaveChangesAsync();

            return AcceptedAtAction("GetJob", new { id = updatedJob.Entity.Id }, updatedJob.Entity);
        }

        // DELETE: api/Jobs/5
        [HttpDelete("{id}")]
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
