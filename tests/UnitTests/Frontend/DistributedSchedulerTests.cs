using Frontend.ComputeNodeSwaggerClient;
using Frontend.Engine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.Frontend
{
    public class DistributedSchedulerTests
    {
        private readonly ServiceProvider serviceProvider;
        private readonly Mock<IComputeNodeClientWrapper> mockedComputeNodeClient = new();

        public DistributedSchedulerTests()
        {
            var services = new ServiceCollection();
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton<ILogger<DistributedScheduler>, Logger<DistributedScheduler>>();

            // Setup mocked ComputeNodeClient
            mockedComputeNodeClient.Setup(m => m.RunAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new ComputeNodeSwaggerClient.AtomicJobResult()));
            services.AddScoped((services) => mockedComputeNodeClient.Object);

            services.AddScoped<IScheduler, DistributedScheduler>();

            serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task ScheduleJobAsync_Success()
        {
            var scheduler = serviceProvider.GetService<IScheduler>();
            Assert.NotNull(scheduler);

            var jobToBeScheduled = TestUtils.GetDummyJob();
            await scheduler.ScheduleJobAsync(jobToBeScheduled);

            mockedComputeNodeClient.Verify(client => client.RunAsync(It.IsAny<int>(), jobToBeScheduled.Id, It.IsAny<string>()), Times.AtLeastOnce());
            Assert.NotEmpty(scheduler.GetInProgressTasks());
        }
    }
}
