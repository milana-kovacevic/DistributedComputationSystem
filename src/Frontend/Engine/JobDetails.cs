using Frontend.Models;
using System.Collections.Concurrent;

namespace Frontend.Engine
{
    /// <summary>
    /// Class that represents internal state of the in progress job.
    /// </summary>
    public class JobDetails
    {
        public int JobId { get; }

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
            _numberOfRemainingAtomicJobs = totalNumberOfAtomicJobs;
        }

        /// <summary>
        /// Removes atomic job from the dictionary. This is used when atomic job completed.
        /// </summary>
        /// <param name="atomicJobId">atomic job id</param>
        /// <param name="result">Output parameter: atomic job result from the dictionary</param>
        /// <returns>Number of remaining in progress atomic jobs</returns>
        internal int RemoveAtomicJob(int atomicJobId, out AtomicJobResult result)
        {
            InProgressAtomicJobs.Remove(atomicJobId, out result);

            return Interlocked.Decrement(ref _numberOfRemainingAtomicJobs);
        }

        internal bool TryAddAtomicJob(int atomicJobId, AtomicJobResult atomicJobResult)
        {
            return InProgressAtomicJobs.TryAdd(atomicJobId, atomicJobResult);
        }
    }
}
