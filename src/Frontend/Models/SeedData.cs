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
                    // This is needed to reschedule unfinished jobs in case of the service restart.
                    var activeJobs = context.Job.AsEnumerable().Where(job => job.IsActive());
                    JobManager.Initialize(activeJobs);

                    return;   // DB has been seeded
                }
            }
        }
    }
}
