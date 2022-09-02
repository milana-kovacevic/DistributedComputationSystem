using Frontend.Data;
using Frontend.Extensions;
using Frontend.Managers;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using System.Collections.ObjectModel;
using System.Linq;

namespace Frontend.Models
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
                    activeJobs = context.Job.AsEnumerable().Where(job => job.IsActive());

                    if (activeJobs.Any() && context.AtomicJob.Any())
                    {
                        var activeAtomicJobs = context.AtomicJob.AsEnumerable().ToList();

                        // TODO this should be automatically populated due to forgein key constraint when seeding Job object
                        // BUG
                        foreach (var job in activeJobs)
                        {
                            job.AtomicJobs ??= activeAtomicJobs.Where(atomicJob => atomicJob.JobId == job.Id).ToList();
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
