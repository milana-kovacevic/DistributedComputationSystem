using ComputeNodeSwaggerClient;
using Frontend.ComputeNodeSwaggerClient;
using Frontend.Data;
using Frontend.Mappers;
using Frontend.Models;
using Microsoft.EntityFrameworkCore;
using AtomicJobResult = ComputeNodeSwaggerClient.AtomicJobResult;

namespace Frontend.Engine
{
    public class DistributedScheduler : IScheduler
    {
        private readonly ILogger<DistributedScheduler> _logger;
        //private IDbContextFactory<JobContext> _contextFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<int, Job> inProgressTasks = new();
        private IComputeNodeClientWrapper _computeNodeClientWrapper;

        public DistributedScheduler(
            ILogger<DistributedScheduler> logger,
            IServiceProvider serviceProvider,
            //IDbContextFactory<JobContext> _contextFactory,
            IComputeNodeClientWrapper computeNodeClientWrapper)
        {
            this._logger = logger;
            //this._contextFactory = context;
            this._serviceProvider = serviceProvider;
            this._computeNodeClientWrapper = computeNodeClientWrapper;
        }

        public static void Initialize(string connectionString)
        {
            Console.WriteLine("Starting distributed scheduler...");
        }

        public async Task ScheduleJobAsync(Job job)
        {
            try
            {/*
                // Create a new scope to retrieve scoped services
                using (var scope = _serviceProvider.CreateScope())
                {
                    // Get the DbContext instance
                    var myDbContext = scope.ServiceProvider.GetRequiredService<JobContext>();

                    //TODO update job state
                }*/

                _logger.LogInformation($"Scheduling job with id {job.Id}...");
                // NOT DONE:
                // [ADVANCED] use custom routing to available ComputeNodes using ingress setup

                // Send units of execution to compute nodes
                var result = new List<AtomicJobResult>();
                int i = 0;
                foreach (var atomicJobUnit in job.AtomicJobs)
                {
                    i++;
                    var r = await _computeNodeClientWrapper.RunAsync(
                        i, // TODO
                        job.Id,
                        AtomicJobTypeMapper.Map(atomicJobUnit.JobType),
                        atomicJobUnit.InputData);

                    result.Add(r);
                    _logger.LogInformation($"JobId {i}; AtomicJobId {job.Id}; ResultState: {r.State}; Result {r.Result}; Error {r.Error}");
                }

                // Monitor the execution
                // TODO
                _logger.LogInformation($"Scheduled job with id {job.Id}");

                // TODO
                inProgressTasks.Add(job.Id, job);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to schedule job with id {job.Id}");
            }
        }

        public IEnumerable<Job> GetInProgressTasks()
        {
            return this.inProgressTasks.Values;
        }
    }
}
