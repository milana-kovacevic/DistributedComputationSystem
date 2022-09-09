using ControlNode.Abstraction.Data;
using ControlNode.DCS.Core.Managers;
using Microsoft.EntityFrameworkCore;

namespace ControlNode.Abstraction.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            IEnumerable<Job> activeJobs = new List<Job>();

            using (var context = new JobContext(serviceProvider.GetRequiredService<DbContextOptions<JobContext>>()))
            {
                // Look for any job in a db.
                if (context.Job.Any())
                {
                    // Populate internally Job manager with active jobs from the db.
                    // This is needed to reschedule unfinished jobs in case of the service restart.

                    // TODO this should be automatically populated due to foreign key constraint when seeding Job object
                    // BUG
                    var activeJobIds = context.JobResult.AsEnumerable()
                        .Where(jobResult => jobResult.IsActive())
                        .Select(jobResult => jobResult.JobId);

                    activeJobs = activeJobs.Where(job => activeJobIds.Contains(job.JobId));

                    if (activeJobs.Any() && context.AtomicJob.Any())
                    {
                        var activeAtomicJobs = context.AtomicJob.AsEnumerable().ToList();

                        // TODO this should be automatically populated due to foreign key constraint when seeding Job object
                        // BUG
                        foreach (var job in activeJobs)
                        {
                            job.AtomicJobs ??= activeAtomicJobs.Where(atomicJob => atomicJob.JobId == job.JobId).ToList();
                            //job.AtomicJobs.AddRange(activeAtomicJobs.Where(atomicJob => atomicJob.JobId == job.Id));
                        }

                        var jobManager = serviceProvider.GetRequiredService<IJobManager>();
                        jobManager.Initialize(activeJobs);

                        return;   // DB has been seeded
                    }

                }
            }
        }
    }
}
