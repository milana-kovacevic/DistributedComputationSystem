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
        public async Task IndexReturnsARedirectToIndexHomeWhenIdIsNull()
        {
            var result = await controller.PostJob(1, 1, "123");

            Assert.IsType<AtomicJobResult>(result);
        }
    }
}
