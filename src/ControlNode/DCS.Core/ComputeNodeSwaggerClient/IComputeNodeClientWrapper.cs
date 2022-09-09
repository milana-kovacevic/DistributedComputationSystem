using FrontendAtomicJobResult = ControlNode.Abstraction.Models.AtomicJobResult;
using FrontendAtomicJobType = ControlNode.Abstraction.Models.AtomicJobType;

namespace ControlNode.DCS.Core.ComputeNodeSwaggerClient
{
    public interface IComputeNodeClientWrapper
    {
        Task<FrontendAtomicJobResult> RunAsync(int atomicJobId, int parentJobId, FrontendAtomicJobType atomicJobType, string inputData);
    }
}
