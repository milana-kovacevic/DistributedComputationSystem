using ComputeNode.Models;

namespace ComputeNode.Executors
{
    public interface ISpecificJobExecutorFactory
    {
        /// <summary>
        /// Build specific job executor.
        /// </summary>
        /// <param name="atomicJobType">Type of job describing which executor should be built.</param>
        /// <returns>ISpecificJobExecutor</returns>
        Task<ISpecificJobExecutor> BuildAsync(AtomicJobType atomicJobType);
    }
}
