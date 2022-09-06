using Frontend.Models;

namespace Frontend.Data
{
    public class DbEntityManager
    {
        private readonly ILogger<DbEntityManager> _logger;
        private readonly IServiceProvider _serviceProvider;

        public DbEntityManager(
            ILogger<DbEntityManager> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        // TODO deduplicate code
        public void UpdateJobState(int jobId, JobState newState, string error = "")
        {
            _logger.LogInformation($"Updating job {jobId} state to: {newState}");

            // Create a new scope to retrieve scoped services
            using (var scope = _serviceProvider.CreateScope())
            {
                // Get the DbContext instance
                var jobContext = scope.ServiceProvider.GetRequiredService<JobContext>();
                var jobResultFromDb = jobContext.JobResult.Where(j => j.JobId == jobId).SingleOrDefault();

                if (jobResultFromDb != null)
                {
                    jobResultFromDb.State = newState;
                    jobResultFromDb.Error = error;

                    jobContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Updates job state only if it didn't fail previously.
        /// </summary>
        /// <param name="jobId">Job id</param>
        /// <param name="new">Wanted new job state.</param>
        /// <returns>Returns state pushed to database.</returns>
        internal JobState UpdateJobStateToSuccessIfNotFailed(int jobId, JobState newState)
        {
            _logger.LogInformation($"Updating job {jobId} state to: {newState}");

            // Create a new scope to retrieve scoped services
            using (var scope = _serviceProvider.CreateScope())
            {
                // Get the DbContext instance
                var jobContext = scope.ServiceProvider.GetRequiredService<JobContext>();
                var jobResultFromDb = jobContext.JobResult.Where(j => j.JobId == jobId).SingleOrDefault();

                if (jobResultFromDb != null && jobResultFromDb.State != JobState.Failed)
                {
                    jobResultFromDb.State = newState;
                    jobContext.SaveChanges();

                    return newState;
                }
                else
                {
                    return jobResultFromDb.State;
                }
            }
        }

        // TODO deduplicate code
        public void UpdateAtomicJobState(int jobId, int atomicJobId, AtomicJobState newState)
        {
            _logger.LogInformation($"Updating atomic job {jobId}:{atomicJobId} state to: {newState}");

            using (var scope = _serviceProvider.CreateScope())
            {
                var jobContext = scope.ServiceProvider.GetRequiredService<JobContext>();
                var jobResultFromDb = jobContext.GetAtomicJobResult(jobId, atomicJobId);

                if (jobResultFromDb != null)
                {
                    jobResultFromDb.State = newState;

                    jobContext.SaveChanges();
                }
            }
        }

        // TODO deduplicate code
        public void UpdateAtomicJobResult(int jobId, int atomicJobId, AtomicJobResult atomicJobResult)
        {
            _logger.LogInformation($"Updating atomic job {jobId}:{atomicJobId} result: {atomicJobResult.State}");

            using (var scope = _serviceProvider.CreateScope())
            {
                var jobContext = scope.ServiceProvider.GetRequiredService<JobContext>();
                var jobResultFromDb = jobContext.GetAtomicJobResult(jobId, atomicJobId);

                if (jobResultFromDb != null)
                {
                    jobResultFromDb.State = atomicJobResult.State;
                    jobResultFromDb.Result = atomicJobResult.Result;
                    jobResultFromDb.Error = atomicJobResult.Error;
                    jobResultFromDb.EndTime = atomicJobResult.EndTime;

                    jobContext.SaveChanges();
                }

            }
        }
    }
}