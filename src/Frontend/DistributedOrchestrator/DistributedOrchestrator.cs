namespace Frontend.DistributedOrchestrator
{
    public class DistributedOrchestrator : IOrchestrator
    {
        public async Task SubmitJobForExecutionAsync(int jobId)
        {
            throw new NotImplementedException();
        }

        public async Task CancelJobAsync(int jobId)
        {
            Task.Delay(100);
            //throw new NotImplementedException();
        }
    }
}
