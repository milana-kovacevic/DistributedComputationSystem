using ComputeNode.Controllers;
using ComputeNode.Executor;
using ComputeNode.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace UnitTests.ComputeNode
{
    public class AtomicJobControllerTests
    {
        private readonly AtomicJobController controller;

        public AtomicJobControllerTests()
        {
            // TODO put this in ConfigureServices method in test utils
            var services = new ServiceCollection();
            services.AddSingleton<IJobExecutor, AtomicJobExecutor>();
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton<ILogger<AtomicJobController>, Logger<AtomicJobController>>();
            services.AddScoped<AtomicJobController>();

            var serviceProvider = services.BuildServiceProvider();

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
    }
}
