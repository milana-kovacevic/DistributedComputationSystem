namespace Frontend.Topology
{
    public interface IToplogyManager
    {
        Task DiscoverAllComputeNodesAsync();
        Task AddComputeNodeAsync();
        Task RemoveComputeNodeAsync();
    }
}
