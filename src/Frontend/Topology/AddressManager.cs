using Frontend.Engine;
using Frontend.Exceptions;

namespace Frontend.Topology
{
    public class AddressManager : IAddressManager
    {
        private readonly ILogger<AddressManager> _logger;
        private static readonly string ComputeNodeServiceName = "COMPUTENODE_SERVICE";
        private static readonly string ComputeNodeServiceHostEnvVariableName = $"{ComputeNodeServiceName}_HOST";
        private static readonly string ComputeNodeServicePortEnvVariableName = $"{ComputeNodeServiceName}_PORT";
        private static readonly int ComputeNodeServicePortDefaultValue = 80;

        private string? _computeNodeServiceAddress = null;

        /// <inheritdoc/>
        public string ComputeNodeServiceAddress
        { 
            get
            {
                if (_computeNodeServiceAddress == null)
                {
                    _logger.LogInformation($"Fetching address for {ComputeNodeServiceHostEnvVariableName} from environment...");

                    // Read value of environment variable which holds the address.
                    var host = Environment.GetEnvironmentVariable(ComputeNodeServiceHostEnvVariableName);

                    if (host == null)
                    {
                        throw new NetworkException(ComputeNodeServiceHostEnvVariableName);
                    }

                    string? port = Environment.GetEnvironmentVariable(ComputeNodeServicePortEnvVariableName);
                    if (port == null)
                    {
                        port = $"{ComputeNodeServicePortDefaultValue}";
                        _logger.LogInformation($"{ComputeNodeServicePortEnvVariableName} not set. Using default value {port}.");
                    }

                    _computeNodeServiceAddress = $"http://{host}:{port}";

                    _logger.LogInformation($"Determined address for {ComputeNodeServiceName}: {_computeNodeServiceAddress}");
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
            await Task.Delay(100);
            throw new NotImplementedException();
        }

        public async Task DiscoverAllComputeNodesAsync()
        {
            // TODO: Implement with advanced ingress network setup.
            await Task.Delay(100);
            throw new NotImplementedException();
        }

        public async Task RemoveComputeNodeAsync()
        {
            // TODO: Implement with advanced ingress network setup.
            await Task.Delay(100);
            throw new NotImplementedException();
        }
    }
}
