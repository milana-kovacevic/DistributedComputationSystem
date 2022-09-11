using ComputeNode.Exceptions;
using ComputeNode.Executor;
using ComputeNode.Models;
using Microsoft.Extensions.DependencyInjection;
using TestCommons;
using Xunit;

namespace UnitTests.ComputeNode
{
    public class AtomicJobExecutor_ExecuteAsync_Should
    {
        private readonly ServiceProvider serviceProvider;
        private readonly IAtomicJobExecutor _executor;

        public AtomicJobExecutor_ExecuteAsync_Should()
        {
            // Configure services using common bootstraper for tests
            var services = new ServiceCollection();
            TestBootstraper.ConfigureServices_ComputeNode(services);

            serviceProvider = services.BuildServiceProvider();
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(42, 6)]
        [InlineData(1001, 2)]
        [InlineData(-2, 2)]
        public async Task ExecuteAsync_ValidInput_ReturnsSuccess(long inputData, int expected)
        {
            var atomicJob = GetDummyAtomicJob(inputData.ToString());

            var _executor = serviceProvider.GetService<IAtomicJobExecutor>();
            var result = await _executor.ExecuteAsync(atomicJob);

            VerifyCommons(atomicJob, result);

            Assert.Equal(AtomicJobState.Succeeded, result.State);
            Assert.Equal(expected.ToString(), result.Result);
            Assert.Null(result.Error);
        }

        [Theory]
        [InlineData("")]
        [InlineData("a")]
        [InlineData("123d")]
        [InlineData("1232asdasdasasf2")]
        public async Task ExecuteAsync_WrongInput_ReturnsFailure(string inputData)
        {
            var atomicJob = GetDummyAtomicJob(inputData);

            var _executor = serviceProvider.GetService<IAtomicJobExecutor>();
            var result = await _executor.ExecuteAsync(atomicJob);

            VerifyCommons(atomicJob, result);

            Assert.Equal(AtomicJobState.Failed, result.State);
            Assert.Equal(string.Format(ExceptionMessages.InvalidInputData, inputData), result.Error);
            Assert.Null(result.Result);
        }

        private static void VerifyCommons(AtomicJob atomicJob, AtomicJobResult result)
        {
            Assert.IsType<AtomicJobResult>(result);
            Assert.NotNull(result);

            Assert.Equal(atomicJob.Id, result.Id);
            Assert.Equal(atomicJob.ParentJobId, result.ParentJobId);
        }

        private static AtomicJob GetDummyAtomicJob(string inputData)
        {
            return new AtomicJob()
            {
                Id = 1,
                ParentJobId = 2,
                AtomicJobResult = new AtomicJobResult()
                {
                    State = AtomicJobState.NotRan,
                    StartTime = DateTime.Now
                },
                InputData = inputData
            };
        }
    }
}
