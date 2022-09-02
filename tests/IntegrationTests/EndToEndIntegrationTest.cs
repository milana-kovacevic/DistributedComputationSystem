using DistributedCalculationSystem;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IntegrationTests
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
        private const string baseUrl = "http://host.docker.internal:8081";
        DistributedCalculationSystemClient client = null;

        public EndToEndIntegrationTest()
        {
            this.client = new DistributedCalculationSystemClient(baseUrl, new HttpClient());
        }

        [Fact]
        public async Task ListAllJobs_Success()
        {

            var jobs = await client.AllAsync();

            Console.WriteLine("Jobs:");
            foreach (var job in jobs)
            {
                Console.WriteLine(job.ToString());
            }

            Assert.NotEmpty(jobs);
        }

        [Fact]
        public async Task RunJob_Success()
        {
            var inputData = new Collection<AtomicJobRequestData>();
            var request = new JobRequestData() {
                JobType = JobType.CalculateSumOfDigits,
                InputData = inputData
            };

            var job = await client.CreateAsync(request);

            Assert.NotNull(job);

            // Verify job
            var jobFromSystem = await client.JobsAsync(job.Id);
            Assert.NotNull(jobFromSystem);

            // TODO: Poll and verify result

            // Delete job
        }
    }
}