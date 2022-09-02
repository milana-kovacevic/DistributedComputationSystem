using ComputeNode.Controllers;
using ComputeNode.Executor;
using ComputeNode.Executors;
using ComputeNode.Models;
using Frontend.Engine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.ComputeNode
{
    public class AtomicJobControllerTests
    {
        private readonly ServiceCollection services;
        private readonly ServiceProvider serviceProvider;
        private readonly AtomicJobController controller;

        public AtomicJobControllerTests()
        {
            // TODO put this in ConfigureServices method in test utils
            services = new ServiceCollection();
            services.AddSingleton<IJobExecutor, AtomicJobExecutor>();
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton<ILogger<AtomicJobController>, Logger<AtomicJobController>>();
            services.AddScoped<AtomicJobController>();
            services.AddSingleton<ILogger<AtomicJobExecutor>, Logger<AtomicJobExecutor>>();
            services.AddScoped<IJobExecutor, AtomicJobExecutor>();
            services.AddSingleton<ISpecificJobExecutorFactory, SpecificJobExecutorFactory>();
            services.AddSingleton<CalculateNumberOfDigitsExecutor>();

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
            Assert.Equal("6", result.Result);
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
        {/*
            Mock<IJobExecutor> mockedExecutor = new Mock<IJobExecutor>();

            // Setup mocked scheduler.
            mockedExecutor.Setup(s => s.(It.IsAny<Job>()))
                .Throws(Task.CompletedTask);
            services.AddScoped<IJobExecutor>((services) => mockedExecutor.Object);

            serviceProvider = services.BuildServiceProvider();

            serviceProvider.addser.AddScoped<IJobExecutor, AtomicJobExecutor>();

            mockedExecutor.Setup(s => s.ScheduleJobAsync(It.IsAny<Job>()))
                .Returns(Task.CompletedTask);
            services.AddScoped<IScheduler>((services) => mockedExecutor.Object);

            var result = await controller.PostJob(1, 1, "aaa");

            Assert.IsType<AtomicJobResult>(result);
            Assert.Equal(AtomicJobState.Failed, result.State);*/
        }
    }
}
