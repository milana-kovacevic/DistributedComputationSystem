namespace Frontend.Topology
{
    public interface IAddressManager
    {
        /// <summary>
        /// Gets the ComputeNode service Cluster IP address.
        /// Used in SIMPLE networking setup - relying on the Kubernetes load balancing
        /// when communicating with the service.
        /// </summary>
        /// <returns>ComputeNode Cluster IP address.</returns>
        string ComputeNodeServiceAddress { get; }

        Task DiscoverAllComputeNodesAsync();

        Task AddComputeNodeAsync();

        Task RemoveComputeNodeAsync();
    }
}
