using ClusterTests;
using DistributedCalculationSystem;
using System.Collections.ObjectModel;
using TestCommons;
using Xunit;
using Xunit.Abstractions;

namespace ClusterTests
{
    public class StressClusterTests : ClusterTestsBase
    {
        private int NumbersToGenerate = 500;
        private int MinGenerated = 10000;
        private int MaxGenerated = 15000;

        public StressClusterTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public async void RunHeavyJob_Success()
        {
            GenerateJobRequestData(NumbersToGenerate, MinGenerated, MaxGenerated, out JobRequestData request, out string expectedResult);

            _testOutputHelper.WriteLine("Running request...");

            // Create job.
            var job = await _client.CreateAsync(request);
            Assert.NotNull(job);

            // Verify job is created.
            var jobFromSystem = await _client.JobsAsync(job.JobId);
            Assert.NotNull(jobFromSystem);

            // Poll and verify job state until it's successfully completed.
            await PollJobUntilSuccess(job.JobId);

            _testOutputHelper.WriteLine("Verifying result...");
            // Verify aggregated result
            await GetAndVerifyJobResultSuccess(job.JobId, expectedResult);

            await CleanupJob(job.JobId);
        }
    }
}
