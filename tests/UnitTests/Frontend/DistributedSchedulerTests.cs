using ComputeNodeSwaggerClient;
using Frontend.ComputeNodeSwaggerClient;
using Frontend.Engine;
using Frontend.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using FrontendAtomicJobType = Frontend.Models.AtomicJobType;
using FrontendAtomicJobResult = Frontend.Models.AtomicJobResult;
using Frontend.Controllers;
using Microsoft.AspNetCore.Mvc;
using TestCommons;

namespace UnitTests.Frontend
{
    public class DistributedSchedulerTests
    {
        private readonly ServiceProvider serviceProvider;
        private readonly Mock<IComputeNodeClientWrapper> mockedComputeNodeClient = new();

        public DistributedSchedulerTests()
        {
            // Configure services using common bootstraper for tests
            var services = new ServiceCollection();
            TestBootstraper.ConfigureServices_Frontend(services);

            // Setup mocked ComputeNodeClient
            mockedComputeNodeClient.Setup(m => m.RunAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<FrontendAtomicJobType>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new FrontendAtomicJobResult()));
            services.AddScoped((services) => mockedComputeNodeClient.Object);
            
            // TODO db setup

            services.AddScoped<IScheduler, DistributedScheduler>();

            serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task ScheduleJobAsync_Success()
        {
            var scheduler = serviceProvider.GetService<IScheduler>();
            Assert.NotNull(scheduler);

            var jobToBeScheduled = UnitTestUtils.GetDummyJob();
            await scheduler.ScheduleJobAsync(jobToBeScheduled);

            mockedComputeNodeClient.Verify(client => client.RunAsync(It.IsAny<int>(), jobToBeScheduled.JobId, It.IsAny<FrontendAtomicJobType>(), It.IsAny<string>()), Times.AtLeastOnce());
            Assert.NotEmpty(scheduler.GetInProgressTasks());
        }
    }
}
