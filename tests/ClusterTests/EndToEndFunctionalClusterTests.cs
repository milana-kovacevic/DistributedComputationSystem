using ControlNode.DCS.Core.Exceptions;
using ComputeNode.Exceptions;
using DistributedCalculationSystem;
using System.Collections.ObjectModel;
using System.Net;
using TestCommons;
using Xunit;

namespace ClusterTests
{
    public class EndToEndFunctionalClusterTests
    {
        private const string baseUrl = "https://matf-distr-comp-sys.westeurope.cloudapp.azure.com/";
        private DistributedCalculationSystemClient _client = null;
        private TimeSpan defaultTimeout = TimeSpan.FromSeconds(5);

        public EndToEndFunctionalClusterTests()
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
        }

        [Fact]
        public async void RunJob_Success()
        {
            var inputData = new Collection<AtomicJobRequestData>()
            {
                new AtomicJobRequestData() { InputData ="42" },
                new AtomicJobRequestData() { InputData ="142" },
            };
            string expectedTotalSum = "1453";

            var request = new JobRequestData()
            {
                JobType = JobType.CalculateSumOfDigits,
                InputData = inputData
            };

            // Create job.
            var job = await _client.CreateAsync(request);
            Assert.NotNull(job);

            // Verify job is created.
            var jobFromSystem = await _client.JobsAsync(job.JobId);
            Assert.NotNull(jobFromSystem);

            // Poll and verify job state until it's successfully completed.
            await TestUtils.PollUntilSatisfied(
                job.JobId,
                (jobId) =>
                {
                    var jobFromSys = _client.JobsAsync(jobId).GetAwaiter().GetResult();
                    return jobFromSys.State == JobState.Succeeded;
                },
                timeout: defaultTimeout,
                pollingInterval: TimeSpan.FromSeconds(2));

            // Verify aggregated result
            var jobResult = await _client.JobResultsAsync(job.JobId);
            Assert.Null(jobResult.Error);
            Assert.Equal(expectedTotalSum, jobResult.Result);
            Assert.Equal(JobState.Succeeded, jobResult.State);

            // Delete job
            await _client.DeleteAsync(job.JobId);

            // Now getting job should throw 404.
            var exception = Assert.ThrowsAsync<ApiException>(async () => await _client.JobsAsync(job.JobId));
            Assert.Equal<int>((int)HttpStatusCode.NotFound, exception.Result.StatusCode);
        }

        [Fact]
        public async void RunJob_Failure()
        {
            string dummyValue = "dummy_value";
            var inputData = new Collection<AtomicJobRequestData>()
            {
                new AtomicJobRequestData() { InputData = dummyValue },
                new AtomicJobRequestData() { InputData ="2" },
            };

            var request = new JobRequestData()
            {
                JobType = JobType.CalculateSumOfDigits,
                InputData = inputData
            };

            // Create job.
            var job = await _client.CreateAsync(request);
            Assert.NotNull(job);

            // Poll and verify job state until it's completed.
            await TestUtils.PollUntilSatisfied(
                job.JobId,
                (jobId) =>
                {
                    var jobFromSys = _client.JobsAsync(jobId).GetAwaiter().GetResult();
                    return jobFromSys.State == JobState.Failed;
                },
                timeout: defaultTimeout,
                pollingInterval: TimeSpan.FromSeconds(2));

            // Verify aggregated result
            var jobResult = await _client.JobResultsAsync(job.JobId);
            Assert.Equal(JobState.Failed, jobResult.State);
            Assert.Contains(DCSCoreExceptionMessages.ParentJobFailed, jobResult.Error);
            Assert.Contains(string.Format(ExceptionMessages.InvalidInputData, dummyValue), jobResult.Error);
            Assert.Null(jobResult.Result);

            // Delete job
            await _client.DeleteAsync(job.JobId);
        }
    }
}