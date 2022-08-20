namespace Frontend.Topology
{
    public class TopologyManager : IToplogyManager
    {
        public TopologyManager()
        {
            // TODO discover available compute nodes
        }

        Task IToplogyManager.AddComputeNodeAsync()
        {
            throw new NotImplementedException();
        }

        Task IToplogyManager.DiscoverAllComputeNodesAsync()
        {
            throw new NotImplementedException();
        }

        Task IToplogyManager.RemoveComputeNodeAsync()
        {
            throw new NotImplementedException();
        }
    }
}
