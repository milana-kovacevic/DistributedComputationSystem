using DistributedCalculationSystem;
using System.Collections.ObjectModel;
using System.Net;
using TestCommons;
using Xunit;

namespace IntegrationTests
{
    /// <summary>
    /// This is end to end test scenario.
    /// In order to run these tests, local setup must be created.
    /// There should be:
    ///     1. Frontend service running locally at http://host.docker.internal:8081
    ///         a. It must have environment variable COMPUTENODE_SERVICE_HOST set to host.docker.internal
    ///     2. ComputeNode service running at corresponding port (8080).
    /// </summary>
    public class EndToEndIntegrationTest
    {
        private const string baseUrl = "http://host.docker.internal:8081";
        private DistributedCalculationSystemClient _client = null;
        private TimeSpan defaultTimeout = TimeSpan.FromSeconds(5);

        public EndToEndIntegrationTest()
        {
            _client = new DistributedCalculationSystemClient(baseUrl, new HttpClient());
        }

        [Fact]
        public async Task RunJob_Success()
        {
            var inputData = new Collection<AtomicJobRequestData>()
            {
                new AtomicJobRequestData() { InputData ="123" },
                new AtomicJobRequestData() { InputData ="42" }
            };

            var request = new JobRequestData() {
                JobType = JobType.CalculateSumOfDigits,
                InputData = inputData
            };

            var job = await _client.CreateAsync(request);
            Assert.NotNull(job);

            // Verify job
            var jobFromSystem = await _client.JobsAsync(job.JobId);
            Assert.NotNull(jobFromSystem);

            // Poll and verify job state
            await TestUtils.PollUntilSatisfied(
                job.JobId,
                (jobId) =>
                {
                    var jobFromSys = _client.JobsAsync(jobId).GetAwaiter().GetResult();
                    return jobFromSys.JobResult.State == JobState.Succeeded;
                },
                timeout: defaultTimeout);

            // Verify aggregated result
            var completedJob = await _client.JobsAsync(job.JobId);
            Assert.Equal("12", completedJob.JobResult.Result);

            // Delete job
            await _client.DeleteAsync(job.JobId);

            // Now getting job should throw 404.
            var exception = Assert.ThrowsAsync<ApiException>(async () => await _client.JobsAsync(job.JobId));
            Assert.Equal<int>((int)HttpStatusCode.NotFound, exception.Result.StatusCode);
        }

        [Fact]
        public async Task ListAllJobs_Success()
        {
            var jobs = await _client.AllAsync();

            Console.WriteLine("Jobs:");
            foreach (var job in jobs)
            {
                Console.WriteLine(job.ToString());
            }

            Assert.NotEmpty(jobs);
        }

        [Fact]
        public async Task RunJob_BadRequest()
        {
            var request = new JobRequestData()
            {
                InputData = new Collection<AtomicJobRequestData>()
            };

            var exception = Assert.ThrowsAsync<ApiException>(() => _client.CreateAsync(request));
            Assert.Equal<int>((int)HttpStatusCode.BadRequest, exception.Result.StatusCode);
        }
    }
}