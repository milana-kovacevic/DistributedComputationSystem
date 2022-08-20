using Frontend.Models;
using System.Collections.Concurrent;

namespace Frontend.Engine
{
    /// <summary>
    /// Class that manages queue of jobs submitted for distributed computation in a thread safe way.
    /// This class implements Producer-Consumer design pattern through BlockingCollection.
    /// Producer is creating new jobs (through JobController) while job scheduler is the consumer.
    /// </summary>
    public class JobQueue
    {
        /// <summary>
        /// Maximum queue capacity.
        /// TODO: Add to configuration.
        /// </summary>
        public static readonly int MaxJobs = 15;

        /// <summary>
        /// Timeout for adding the job to the queue.
        /// </summary>
        public static readonly TimeSpan timeout = TimeSpan.FromSeconds(3);

        /// <summary>
        /// Blocking collection containing a list of jobs waiting for execution.
        /// </summary>
        private static readonly BlockingCollection<Job> _jobQueue = new BlockingCollection<Job>(new ConcurrentQueue<Job>(), MaxJobs);

        /// <summary>
        /// Adds new job to the execution queue.
        /// </summary>
        /// <param name="job">Job to be added</param>
        /// <returns>Returns True if job is added, false otherwise.</returns>
        public bool TryEnqueueJob(Job job)
        {
            return _jobQueue.TryAdd(job, timeout);
        }

        /// <summary>
        /// Fetches next job in a queue.
        /// Waits if the queue is empty.
        /// </summary>
        public Job DequeueJob()
        {
            return _jobQueue.Take();
        }
    }
}
