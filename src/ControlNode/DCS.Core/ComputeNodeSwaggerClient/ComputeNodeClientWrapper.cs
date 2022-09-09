using ComputeNodeSwaggerClient;
using ControlNode.DCS.Core.Mappers;
using ControlNode.DCS.Core.Topology;
using ComputeNodeAtomicJobResult = ComputeNodeSwaggerClient.AtomicJobResult;
using FrontendAtomicJobResult = ControlNode.Frontend.Models.AtomicJobResult;
using FrontendAtomicJobState = ControlNode.Frontend.Models.AtomicJobType;

namespace ControlNode.DCS.Core.ComputeNodeSwaggerClient
{
    public class ComputeNodeClientWrapper : IComputeNodeClientWrapper
    {
        private readonly IAddressManager _addressManager;
        private readonly HttpClient _httpClient;
        private readonly ComputeNodeClient _computeNodeClient;

        public ComputeNodeClientWrapper(IAddressManager addressManager)
        {
            this._addressManager = addressManager;
            this._httpClient = new HttpClient();

            var computeNodeServiceAddress = _addressManager.ComputeNodeServiceAddress;
            _computeNodeClient = new ComputeNodeClient(computeNodeServiceAddress, _httpClient);
        }

        public async Task<FrontendAtomicJobResult> RunAsync(int atomicJobId, int parentJobId, FrontendAtomicJobState atomicJobType, string inputData)
        {
            var computeNodeAtomicJobType = AtomicJobTypeMapper.Map(atomicJobType);
            ComputeNodeAtomicJobResult result = await _computeNodeClient.RunAsync(atomicJobId, parentJobId, computeNodeAtomicJobType, inputData);

            return AtomicJobResultMapper.Map(result);
        }
    }
}
