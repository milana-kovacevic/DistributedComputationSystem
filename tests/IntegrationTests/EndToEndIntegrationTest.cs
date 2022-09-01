using DistributedCalculationSystem;

namespace FunctionalTests
{
    /// <summary>
    /// This is end to end test scenario.
    /// In order to run these tests, local setup must be created.
    /// There should be:
    ///     1. Frontend service running locally at http://host.docker.internal:8081
    ///     2. ComputeNode service running at corresponding port (8080).
    /// </summary>
    public class EndToEndIntegrationTest
    {
        [Fact]
        public async Task ListAllJobs_Success()
        {
            string baseUrl = "http://host.docker.internal:8081";
            var client = new DistributedCalculationSystemClient(baseUrl, new HttpClient());

            var jobs = await client.AllAsync();

            Console.WriteLine("Jobs:");
            foreach (var job in jobs)
            {
                Console.WriteLine(job.ToString());
            }

            Assert.NotEmpty(jobs);
        }
    }
}