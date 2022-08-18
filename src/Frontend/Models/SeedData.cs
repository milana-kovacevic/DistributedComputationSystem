using Frontend.Data;
using Frontend.Extensions;
using Frontend.Managers;
using Microsoft.EntityFrameworkCore;

namespace Frontend.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new JobContext(serviceProvider.GetRequiredService<DbContextOptions<JobContext>>()))
            {
                // Look for any job in a db.
                if (context.Job.Any())
                {
                    // Populate internally Job manager with active jobs from the db.
                    var allJobs = context.Job.AsEnumerable().Where(job => job.IsActive())
                        .ToDictionary(job => job.Id);
                    JobManager.Initialize(allJobs);

                    return;   // DB has been seeded
                }

                context.Job.AddRange(
                    new Job
                    {
                        StartTime = DateTime.UtcNow,
                        EndTime = null,
                        State = JobState.Succeeded
                    },

                    new Job
                    {
                        StartTime = DateTime.UtcNow,
                        EndTime = null,
                        State = JobState.Failed
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
