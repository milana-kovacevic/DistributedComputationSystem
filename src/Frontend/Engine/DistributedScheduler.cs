using ComputeNodeSwaggerClient;
using Frontend.ComputeNodeSwaggerClient;
using Frontend.Data;
using Frontend.Models;
using Microsoft.EntityFrameworkCore;

namespace Frontend.Engine
{
    public class DistributedScheduler : IScheduler
    {
        private readonly ILogger<DistributedScheduler> _logger;
        //private IDbContextFactory<JobContext> _contextFactory;
        private readonly Dictionary<int, Job> inProgressTasks = new();
        private IComputeNodeClientWrapper _computeNodeClientWrapper;

        public DistributedScheduler(
            ILogger<DistributedScheduler> logger,
            //IDbContextFactory<JobContext> _contextFactory,
            IComputeNodeClientWrapper computeNodeClientWrapper)
        {
            this._logger = logger;
            //this._contextFactory = context;
            this._computeNodeClientWrapper = computeNodeClientWrapper;
        }

        public static void Initialize(string connectionString)
        {
            Console.WriteLine("Starting distributed scheduler...");
        }

        public async Task ScheduleJobAsync(Job job)
        {
            try
            {
                // Create tasks for atomic units of execution
                string[] unitsOfWork = job.Data.Split(';');

                // NOT DONE:
                // [ADVANCED] use custom routing to available ComputeNodes using ingress setup

                // Send units of execution to compute nodes
                var result = new List<AtomicJobResult>();
                int i = 0;
                foreach (string unit in unitsOfWork)
                {
                    i++;
                    var r = await _computeNodeClientWrapper.RunAsync(i, job.Id, unit);
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
