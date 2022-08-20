using ComputeNode.Models;

namespace ComputeNode.Executor
{
    public interface IJobExecutor
    {
        Task<AtomicJobResult> ExecuteAsync(AtomicJob atomicJob);
    }
}
