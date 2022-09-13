using DistributedCalculationSystem;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Net;
using TestCommons;
using Xunit;

namespace FunctionalTests
{
    public class StressClusterTests
    {
        private const string baseUrl = "https://matf-distr-comp-sys.westeurope.cloudapp.azure.com/";
        private DistributedCalculationSystemClient _client = null;
        private TimeSpan defaultTimeout = TimeSpan.FromSeconds(300);
        private Random randNum;

        public StressClusterTests()
        {
            this.randNum = new Random(42);
            this._client = new DistributedCalculationSystemClient(baseUrl, new HttpClient());
        }

        [Fact]
        public async void RunHeavyJob_Success()
        {
            GenerateJobRequestData(500, out JobRequestData request, out string expectedResult);

            Console.WriteLine("Running request...");

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
                timeout: defaultTimeout);

            Console.WriteLine("Verifying result...");
            // Verify aggregated result
            var jobResult = await _client.JobResultsAsync(job.JobId);
            Assert.Equal(string.Empty, jobResult.Error);
            Assert.Equal(expectedResult, jobResult.Result);
            Assert.Equal(JobState.Succeeded, jobResult.State);

            Console.WriteLine(jobResult);

            await CleanupJob(job.JobId);
        }

        private async Task CleanupJob(int jobId)
        {
            Console.WriteLine("Cleaning up state in DCS...");

            // Delete job
            await _client.DeleteAsync(jobId);

            // Now getting job should throw 404.
            var exception = Assert.ThrowsAsync<ApiException>(async () => await _client.JobsAsync(jobId));
            Assert.Equal<int>((int)HttpStatusCode.NotFound, exception.Result.StatusCode);
        }

        private void GenerateJobRequestData(int numberOfAtomicJobs, out JobRequestData jobRequest, out string expectedResult)
        {
            Console.WriteLine("Generating request...");

            // Generate input data and expected result.
            int[] numbers = GenerateRandomNumbers(numberOfAtomicJobs);

            expectedResult = numbers.Select(n => CalculateSumOfDigits(n))
                .Aggregate(0, (acc, x) => acc + x)
                .ToString();

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

        private int[] GenerateRandomNumbers(int numberOfNumbers)
        {
            int Min = 1000;
            int Max = 10000;

            return Enumerable
                .Repeat(0, numberOfNumbers)
                .Select(i => randNum.Next(Min, Max))
                .ToArray();
        }

        // Calculate sum of digits for all numbers from 1 to number.
        private static int CalculateSumOfDigits(int number)
        {
            int sumOfDigits = 0;

            for (int i = 1; i <= number; i++)
            {
                int current = i;
                while (current > 0)
                {
                    sumOfDigits += current % 10;
                    current /= 10;
                }
            }

            return sumOfDigits;
        }

        private static int Abs(int number)
        {
            return number >= 0 ? number : -number;
        }
    }
}
