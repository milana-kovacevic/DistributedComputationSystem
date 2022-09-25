using DistributedCalculationSystem;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using TestCommons;
using Xunit;
using Xunit.Abstractions;

namespace ClusterTests
{
    public class ClusterTestsBase
    {
        protected readonly ITestOutputHelper _testOutputHelper;
        protected const string baseUrl = "https://matf-distr-comp-sys.westeurope.cloudapp.azure.com/";
        protected DistributedCalculationSystemClient _client = null;
        protected TimeSpan defaultTimeout = TimeSpan.FromSeconds(300);
        protected Random randNum;
        protected TimeSpan defaultPollingInterval = TimeSpan.FromSeconds(3);

        public ClusterTestsBase(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            this.randNum = new Random(42);
            this._client = new DistributedCalculationSystemClient(baseUrl, new HttpClient());
        }

        protected async Task CleanupJob(int jobId)
        {
            _testOutputHelper.WriteLine("Cleaning up state in DCS...");

            // Delete job
            await _client.DeleteAsync(jobId);

            // Now getting job should throw 404.
            var exception = Assert.ThrowsAsync<ApiException>(async () => await _client.JobsAsync(jobId));
            Assert.Equal<int>((int)HttpStatusCode.NotFound, exception.Result.StatusCode);
        }

        protected async Task PollJobUntilSuccess(int jobId)
        {
            await TestUtils.PollUntilSatisfied(
                jobId,
                (jobId) =>
                {
                    try
                    {
                        var jobFromSys = _client.JobsAsync(jobId).GetAwaiter().GetResult();
                        return jobFromSys.State == JobState.Succeeded;
                    }
                    catch (Exception)
                    {
                        // case when system returns 502
                        return false;
                    }
                },
                timeout: defaultTimeout,
                pollingInterval: defaultPollingInterval);
        }

        protected async Task<JobResult> GetAndVerifyJobResultSuccess(int jobId, string expectedResult)
        {
            var jobResult = await _client.JobResultsAsync(jobId);
            Assert.Null(jobResult.Error);
            Assert.Equal(expectedResult, jobResult.Result);
            Assert.Equal(JobState.Succeeded, jobResult.State);

            return jobResult;
        }

        protected void GenerateJobRequestData(int numberOfAtomicJobs, int minNum, int maxNum, out JobRequestData jobRequest, out string expectedResult)
        {
            // Generate input data and expected result.
            int[] numbers = GenerateRandomNumbers(numberOfAtomicJobs, minNum, maxNum);

            Stopwatch stopwatch = new();
            stopwatch.Start();
            expectedResult = CalculateSumOfDigitsExecutor.CalculateSumOfDigits(numbers).ToString();
            stopwatch.Stop();

            _testOutputHelper.WriteLine($"Execution time (local-sequential): '{stopwatch.Elapsed}'.");

            // Generate request.
            var inputData = new Collection<AtomicJobRequestData>();
            foreach (var number in numbers)
            {
                inputData.Add(new AtomicJobRequestData() { InputData = $"{number}" });
            }

            jobRequest = new JobRequestData()
            {
                JobType = JobType.CalculateSumOfDigits,
                InputData = inputData
            };
        }

        protected int[] GenerateRandomNumbers(int numberOfNumbers, int minNumber, int maxNumber)
        {
            return Enumerable
                .Repeat(0, numberOfNumbers)
                .Select(i => randNum.Next(minNumber, maxNumber))
                .ToArray();
        }
    }
}
