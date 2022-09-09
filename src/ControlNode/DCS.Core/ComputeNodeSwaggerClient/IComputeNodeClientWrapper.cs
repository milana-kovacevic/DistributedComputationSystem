using FrontendAtomicJobResult = ControlNode.Frontend.Models.AtomicJobResult;
using FrontendAtomicJobType = ControlNode.Frontend.Models.AtomicJobType;

namespace ControlNode.DCS.Core.ComputeNodeSwaggerClient
{
    public interface IComputeNodeClientWrapper
    {
        Task<FrontendAtomicJobResult> RunAsync(int atomicJobId, int parentJobId, FrontendAtomicJobType atomicJobType, string inputData);
    }
}
