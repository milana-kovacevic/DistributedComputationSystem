using Frontend.Engine;
using Frontend.Exceptions;

namespace Frontend.Topology
{
    public class AddressManager : IAddressManager
    {
        private readonly ILogger<AddressManager> _logger;
        private static readonly string ComputeNodeAddresEnvVariableName = "COMPUTENODE_SERVICE_HOST";

        private string _computeNodeServiceAddress = null;

        /// <inheritdoc/>
        public string ComputeNodeServiceAddress
        { 
            get
            {
                if (_computeNodeServiceAddress == null)
                {
                    _logger.LogInformation($"Fetching address for {ComputeNodeAddresEnvVariableName} from environment...");

                    // Read value of environment variable which holds the address.
                    _computeNodeServiceAddress = Environment.GetEnvironmentVariable(ComputeNodeAddresEnvVariableName);

                    if (_computeNodeServiceAddress == null)
                    {
                        throw new NetworkException(ComputeNodeAddresEnvVariableName);
                    }

                    _logger.LogInformation($"Determined address for {ComputeNodeAddresEnvVariableName}: {_computeNodeServiceAddress}");
                }

                return _computeNodeServiceAddress;
            }

        }

        public AddressManager(ILogger<AddressManager> logger)
        {
            this._logger = logger;
        }

        public async Task AddComputeNodeAsync()
        {
            // TODO: Implement with advanced ingress network setup.
            throw new NotImplementedException();
        }

        public async Task DiscoverAllComputeNodesAsync()
        {
            // TODO: Implement with advanced ingress network setup.
            throw new NotImplementedException();
        }

        public async Task RemoveComputeNodeAsync()
        {
            // TODO: Implement with advanced ingress network setup.
            throw new NotImplementedException();
        }
    }
}
