using ComputeNode.Models;

namespace ComputeNode.Executors
{
    public interface ISpecificJobExecutor
    {
        Task<AtomicJobResult> ExecuteAsync(AtomicJob atomicJob);
    }
}
