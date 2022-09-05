using FrontendAtomicJobResult = Frontend.Models.AtomicJobResult;
using FrontendAtomicJobType = Frontend.Models.AtomicJobType;

namespace Frontend.ComputeNodeSwaggerClient
{
    public interface IComputeNodeClientWrapper
    {
        Task<FrontendAtomicJobResult> RunAsync(int atomicJobId, int parentJobId, FrontendAtomicJobType atomicJobType, string inputData);
    }
}
