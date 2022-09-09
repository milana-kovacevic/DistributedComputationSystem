using ControlNode.DCS.Core.Engine;
using ControlNode.Frontend.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TestCommons;

namespace UnitTests.ControlNode
{
    public class SchedulerBackgroundServiceTests
    {
        private readonly ServiceProvider serviceProvider;
        private readonly Mock<IScheduler> mockedScheduler = new Mock<IScheduler>();

        public SchedulerBackgroundServiceTests()
        {
            var services = new ServiceCollection();
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton<ILogger<SchedulerBackgroundService>, Logger<SchedulerBackgroundService>>();
            services.AddScoped<JobQueue>();

            // Setup mocked scheduler.
            mockedScheduler.Setup(s => s.ScheduleJobAsync(It.IsAny<Job>()))
                .Returns(Task.CompletedTask);
            services.AddScoped<IScheduler>((services) => mockedScheduler.Object);

            services.AddScoped<SchedulerBackgroundService>();

            serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task ExecuteAsync_CancelBackroundService()
        {
            var backgroundService = serviceProvider.GetService<SchedulerBackgroundService>();
            Assert.NotNull(backgroundService);

            var cts = new CancellationTokenSource();
            int cancelAfterMs = 1000;
            cts.CancelAfter(cancelAfterMs);

            await backgroundService.StartAsync(cts.Token);

            // Wait for task to be completed (canceled).
            await PollTask(backgroundService.ExecuteTask, timeout: TimeSpan.FromSeconds(3));

            Assert.Equal(TaskStatus.RanToCompletion, backgroundService.ExecuteTask.Status);
            mockedScheduler.Verify(mock => mock.ScheduleJobAsync(It.IsAny<Job>()), Times.Never());
        }

        [Fact]
        public async Task ExecuteAsync_ScheduleJob_Success()
        {
            var backgroundService = serviceProvider.GetService<SchedulerBackgroundService>();

            var cts = new CancellationTokenSource();
            int cancelAfterMs = 3000;
            cts.CancelAfter(cancelAfterMs);

            await backgroundService.StartAsync(cts.Token);

            var jobToBeScheduled = UnitTestUtils.GetDummyJob();
            var jobQueue = serviceProvider.GetService<JobQueue>();

            Assert.True(jobQueue.TryEnqueueJob(jobToBeScheduled), "Job failed to be added to the queue.");

            // Wait for task to be completed (canceled).
            await PollTask(backgroundService.ExecuteTask, timeout: TimeSpan.FromSeconds(4));

            Assert.Equal(TaskStatus.RanToCompletion, backgroundService.ExecuteTask.Status);
            mockedScheduler.Verify(mock => mock.ScheduleJobAsync(jobToBeScheduled), Times.Once());
        }

        private async Task PollTask(Task executeTask, TimeSpan timeout) =>
            await TestUtils.PollUntilSatisfied(
                executeTask,
                (task) => task.Status == TaskStatus.RanToCompletion,
                timeout: timeout);
    }
}
