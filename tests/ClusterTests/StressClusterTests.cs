using DistributedCalculationSystem;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using TestCommons;
using Xunit;
using Xunit.Abstractions;

namespace FunctionalTests
{
    public class StressClusterTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private const string baseUrl = "https://matf-distr-comp-sys.westeurope.cloudapp.azure.com/";
        private DistributedCalculationSystemClient _client = null;
        private TimeSpan defaultTimeout = TimeSpan.FromSeconds(300);
        private Random randNum;

        private int NumbersToGenerate = 1000;
        private int MinGenerated = 400000;
        private int MaxGenerated = 450000;

        public StressClusterTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            this.randNum = new Random(42);
            this._client = new DistributedCalculationSystemClient(baseUrl, new HttpClient());
        }

        [Fact]
        public async void RunHeavyJob_Success()
        {
            GenerateJobRequestData(NumbersToGenerate, out JobRequestData request, out string expectedResult);

            _testOutputHelper.WriteLine("Running request...");

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

            _testOutputHelper.WriteLine("Verifying result...");
            // Verify aggregated result
            var jobResult = await _client.JobResultsAsync(job.JobId);
            Assert.Equal(string.Empty, jobResult.Error);
            Assert.Equal(expectedResult, jobResult.Result);
            Assert.Equal(JobState.Succeeded, jobResult.State);

            Console.WriteLine(jobResult);

            // Log execution time on cluster
            _testOutputHelper.WriteLine($"Elapsed execution time on cluster: '{jobResult.EndTime - jobResult.StartTime}'.");

            await CleanupJob(job.JobId);
        }

        private async Task CleanupJob(int jobId)
        {
            _testOutputHelper.WriteLine("Cleaning up state in DCS...");

            // Delete job
            await _client.DeleteAsync(jobId);

            // Now getting job should throw 404.
            var exception = Assert.ThrowsAsync<ApiException>(async () => await _client.JobsAsync(jobId));
            Assert.Equal<int>((int)HttpStatusCode.NotFound, exception.Result.StatusCode);
        }

        private void GenerateJobRequestData(int numberOfAtomicJobs, out JobRequestData jobRequest, out string expectedResult)
        {
            _testOutputHelper.WriteLine("Generating request...");

            // Generate input data and expected result.
            int[] numbers = GenerateRandomNumbers(numberOfAtomicJobs);

            Stopwatch stopwatch = new();
            stopwatch.Start();

            expectedResult = numbers.Select(n => CalculateSumOfDigits(n))
                .Aggregate(0, (acc, x) => acc + x)
                .ToString();

            stopwatch.Stop();

            _testOutputHelper.WriteLine($"Elapsed execution time needed to generate request: '{stopwatch.Elapsed}'.");

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
            return Enumerable
                .Repeat(0, numberOfNumbers)
                .Select(i => randNum.Next(MinGenerated, MaxGenerated))
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
