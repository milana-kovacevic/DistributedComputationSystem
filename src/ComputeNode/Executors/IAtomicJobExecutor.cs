using ComputeNode.Models;

namespace ComputeNode.Executor
{
    public interface IAtomicJobExecutor
    {
        Task<AtomicJobResult> ExecuteAsync(AtomicJob atomicJob);
    }
}
