using DistributedCalculationSystem;
using Xunit;
using Xunit.Abstractions;

namespace ClusterTests
{
    public class PerformanceTests : ClusterTestsBase
    {
        private int MinGenerated = 400000;
        private int MaxGenerated = 450000;

        public PerformanceTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Theory]
        [InlineData(50)]
        [InlineData(100)]
        [InlineData(500)]
        [InlineData(1000)]
        [InlineData(3000)]
        public async void RunJobs_MeasureExecutionTime(int numbersToGenerate)
        {
            // Generate job.
            GenerateJobRequestData(numbersToGenerate, MinGenerated, MaxGenerated, out JobRequestData request, out string expectedResult);

            // Create job.
            var job = await _client.CreateAsync(request);
            Assert.NotNull(job);

            // Verify job is created.
            var jobFromSystem = await _client.JobsAsync(job.JobId);
            Assert.NotNull(jobFromSystem);

            // Poll and verify job state until it's successfully completed.
            await PollJobUntilSuccess(job.JobId);

            // Verify aggregated result
            var jobResult = await GetAndVerifyJobResultSuccess(job.JobId, expectedResult);

            // Log execution time on cluster
            _testOutputHelper.WriteLine($"Execution time (cluster): '{jobResult.EndTime - jobResult.StartTime}'.");

            await CleanupJob(job.JobId);
        }
    }
}
