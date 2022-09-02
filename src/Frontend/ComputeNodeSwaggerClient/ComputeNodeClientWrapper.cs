using ComputeNodeSwaggerClient;
using Frontend.Topology;

namespace Frontend.ComputeNodeSwaggerClient
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

        public async Task<AtomicJobResult> RunAsync(int atomicJobId, int parentJobId, AtomicJobType atomicJobType, string inputData)
        {
            return await _computeNodeClient.RunAsync(atomicJobId, parentJobId, atomicJobType, inputData);
        }
    }
}
