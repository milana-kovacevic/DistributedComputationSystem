using Frontend.Data;
using Microsoft.EntityFrameworkCore;

namespace Frontend.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new JobContext( serviceProvider.GetRequiredService<DbContextOptions<JobContext>>()))
            {
                // Look for any movies.
                if (context.Job.Any())
                {
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
