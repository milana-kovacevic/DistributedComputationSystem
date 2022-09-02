using ComputeNodeSwaggerClient;

namespace Frontend.ComputeNodeSwaggerClient
{
    public interface IComputeNodeClientWrapper
    {
        Task<AtomicJobResult> RunAsync(int atomicJobId, int parentJobId, AtomicJobType atomicJobType, string inputData);
    }
}
