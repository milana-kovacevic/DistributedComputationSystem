namespace Frontend.DistributedOrchestrator
{
    public interface IOrchestrator
    {
        Task SubmitJobForExecutionAsync(int jobId);

        Task CancelJobAsync(int jobId);
    }
}
