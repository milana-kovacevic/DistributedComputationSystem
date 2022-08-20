using Frontend.Data;
using Frontend.Models;
using Frontend.Topology;

namespace Frontend.Engine
{
    public class DistributedScheduler : IScheduler
    {
        private readonly ILogger<DistributedScheduler> _logger;
        private readonly JobContext _context;
        private readonly IToplogyManager _toplogyManager;
        private readonly Dictionary<int, Job> inProgressTasks = new Dictionary<int, Job>();

        public DistributedScheduler(
            ILogger<DistributedScheduler> logger,
            JobContext context,
            IToplogyManager toplogyManager)
        {
            this._logger = logger;
            this._context = context;
            this._toplogyManager = toplogyManager;
        }

        internal void Initialize()
        {
            Console.WriteLine("Starting distributed scheduler...");
        }


        public Task ScheduleJobAsync(Job job)
        {
            // Create tasks for atomic units of execution

            // Determine number of available compute nodes

            // Make a distributed plan of execution

            // Send units of execution to compute nodes

            // Monitor the execution


            inProgressTasks.Add(job.Id, job);
            
            // TODO
            return Task.CompletedTask;
        }
    }
}
