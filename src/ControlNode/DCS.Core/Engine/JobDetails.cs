using ControlNode.Frontend.Models;
using System.Collections.Concurrent;

namespace ControlNode.DCS.Core.Engine
{
    /// <summary>
    /// Class that represents internal state of the in progress job.
    /// </summary>
    public class JobDetails
    {
        public int JobId { get; }

        /// <summary>
        /// Aggregated job result for Map-Reduce
        /// </summary>
        private int _aggregatedJobResult;
        public int AggregatedJobResult => _aggregatedJobResult;

        private int _numberOfRemainingAtomicJobs;

        public int NumberOfRemainingAtomicJobs => _numberOfRemainingAtomicJobs;

        /// <summary>
        /// Dictionary where key is atomic job id, and the value is atomic job result.
        /// This dictionary only contains in progress atomic jobs.
        /// </summary>
        public ConcurrentDictionary<int, AtomicJobResult> InProgressAtomicJobs { get; set;  } = new();

        public JobDetails(int jobId, int totalNumberOfAtomicJobs)
        {
            JobId = jobId;
            _aggregatedJobResult = 0;
            _numberOfRemainingAtomicJobs = totalNumberOfAtomicJobs;
        }

        /// <summary>
        /// Function to execute when atomic job completed.
        /// If atomic job succeeded, execute REDUCE step to produce aggregated result.
        /// Removes the atomic job from the list of in progress jobs.
        /// </summary>
        /// <param name="atomicJobId">atomic job id</param>
        /// <param name="atomicJobResult">atomic job result from Control Node</param>
        /// <returns>Number of remaining in progress atomic jobs</returns>
        internal int MarkAtomicJobCompleted(int atomicJobId, AtomicJobResult atomicJobResult)
        {
            InProgressAtomicJobs.Remove(atomicJobId, out _);

            // Run REDUCE step.
            if (atomicJobResult.State == AtomicJobState.Succeeded)
            {
                // Special handling for sum of digits job type
                // TODO error handling
                if (int.TryParse(atomicJobResult.Result, out int resultSum))
                {
                    // Do thread safe sum here.
                    Interlocked.Add(ref _aggregatedJobResult, resultSum);
                }
            }

            // Return the remaining number of in progress jobs.
            return Interlocked.Decrement(ref _numberOfRemainingAtomicJobs);
        }

        internal bool TryAddAtomicJob(int atomicJobId, AtomicJobResult atomicJobResult)
        {
            return InProgressAtomicJobs.TryAdd(atomicJobId, atomicJobResult);
        }
    }
}
