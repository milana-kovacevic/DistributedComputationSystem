using DistributedCalculationSystem;
using System.Collections.ObjectModel;
using System.Net;
using TestCommons;
using Xunit;

namespace FunctionalTests
{
    public class EndToEndClusterTests
    {
        private const string baseUrl = "https://matf-distr-comp-sys.westeurope.cloudapp.azure.com/";
        private DistributedCalculationSystemClient _client = null;
        private TimeSpan defaultTimeout = TimeSpan.FromSeconds(5);

        public EndToEndClusterTests()
        {
            this._client = new DistributedCalculationSystemClient(baseUrl, new HttpClient());
        }

        [Fact]
        public async void ListJobs_Success()
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
        public async void RunJob_Success()
        {
            int i = 1;
            var inputData = new Collection<AtomicJobRequestData>()
            {
                new AtomicJobRequestData() { InputData =$"{i++}" },
                new AtomicJobRequestData() { InputData =$"{i++}" },
                new AtomicJobRequestData() { InputData =$"{i++}" },
                new AtomicJobRequestData() { InputData =$"{i++}" },
            };

            var request = new JobRequestData()
            {
                JobType = JobType.CalculateSumOfDigits,
                InputData = inputData
            };

            var job = await _client.CreateAsync(request);
            Assert.NotNull(job);

            // Verify job
            var jobFromSystem = await _client.JobsAsync(job.Id);
            Assert.NotNull(jobFromSystem);

            // Poll and verify job state.
            await TestUtils.PollUntilSatisfied(
                job.Id,
                (jobId) =>
                {
                    var jobFromSys = _client.JobsAsync(jobId).GetAwaiter().GetResult();
                    return jobFromSys.JobResult.State == JobState.Succeeded;
                },
                timeout: defaultTimeout);

            // Verify aggregated result
            var completedJob = await _client.JobsAsync(job.Id);
            Assert.Equal("10", completedJob.JobResult.Result);

            // Delete job
            await _client.DeleteAsync(job.Id);

            // Now getting job should throw 404.
            var exception = Assert.ThrowsAsync<ApiException>(async () => await _client.JobsAsync(job.Id));
            Assert.Equal<int>((int)HttpStatusCode.NotFound, exception.Result.StatusCode);
        }
    }
}