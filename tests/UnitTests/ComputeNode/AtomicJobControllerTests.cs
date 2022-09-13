using ComputeNode.Controllers;
using ComputeNode.Exceptions;
using ComputeNode.Executors;
using ComputeNode.Models;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TestCommons;
using Xunit;

namespace UnitTests.ComputeNode
{
    public class AtomicJobControllerTests
    {
        private readonly ServiceCollection services;
        private readonly ServiceProvider serviceProvider;
        private readonly AtomicJobController controller;

        public AtomicJobControllerTests()
        {
            // Configure test services.
            services = new ServiceCollection();
            TestBootstraper.ConfigureServices_ComputeNode(services);
            serviceProvider = services.BuildServiceProvider();

            controller = serviceProvider.GetService<AtomicJobController>();
            
            // Controller must be successfully resolved at this point.
            Assert.NotNull(controller);
        }

        [Fact]
        public async Task PostJob_Success()
        {
            var result = await controller.PostJob(1, 1, "123");

            Assert.IsType<AtomicJobResult>(result);
            Assert.Equal(AtomicJobState.Succeeded, result.State);
            Assert.Equal("1038", result.Result);
        }

        [Fact]
        public async Task PostJob_Failure()
        {
            var result = await controller.PostJob(1, 1, "aaa");

            Assert.IsType<AtomicJobResult>(result);
            Assert.Equal(AtomicJobState.Failed, result.State);
        }

        [Fact]
        public async Task PostJob_UnhandledException()
        {
            string testErrorMsg = "Unhandled exception from test!";
            Mock<ISpecificJobExecutor> mockedExecutor = new Mock<ISpecificJobExecutor>();

            // Setup mocked executor.
            mockedExecutor.Setup(s => s.ExecuteAsync(It.IsAny<AtomicJob>()))
                .Throws(new Exception(testErrorMsg));
            services.AddScoped<ISpecificJobExecutor>((services) => mockedExecutor.Object);

            // Setup mocked executor factory.
            Mock<ISpecificJobExecutorFactory> mockedExecutorFactory = new Mock<ISpecificJobExecutorFactory>();
            mockedExecutorFactory.Setup(s => s.BuildAsync(It.IsAny<AtomicJobType>()))
                .Returns(Task.FromResult(mockedExecutor.Object));
            services.AddSingleton<ISpecificJobExecutorFactory>((services) => mockedExecutorFactory.Object);

            var serviceProviderMockedExecutor = services.BuildServiceProvider();
            var controllerMockedExecutor = serviceProviderMockedExecutor.GetService<AtomicJobController>();

            // Run atomic job.
            int jobId = 1;
            int atomicJobId = 1;
            var result = await controllerMockedExecutor.PostJob(jobId, atomicJobId, "1");

            // Verify result.
            Assert.IsType<AtomicJobResult>(result);
            Assert.Equal(AtomicJobState.Failed, result.State);
            Assert.Contains(string.Format(ExceptionMessages.UnhandledException, jobId, atomicJobId, string.Empty).Split('.')[0], result.Error);
            Assert.Contains(testErrorMsg, result.Error);
        }
    }
}
