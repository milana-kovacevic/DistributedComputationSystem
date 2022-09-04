using Frontend.Controllers;
using Frontend.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using TestCommons;

namespace UnitTests.Frontend
{
    public class JobControllerTests
    {
        private readonly ServiceCollection services;
        private readonly ServiceProvider serviceProvider;
        private readonly JobsController controller;

        public JobControllerTests()
        {
            // Configure services using common bootstraper for tests
            services = new ServiceCollection();
            TestBootstraper.ConfigureServices_Frontend(services);
            serviceProvider = services.BuildServiceProvider();

            controller = serviceProvider.GetService<JobsController>();

            // Controller must be successfully resolved at this point.
            Assert.NotNull(controller);
        }

        [Fact]
        public async Task PostJob_BadRequest()
        {
            var request = new JobRequestData()
            {
                InputData = new Collection<AtomicJobRequestData>()
            };

            var result = await controller.PostJob(request);

            Assert.NotNull(result.Result);
            //Assert.Equal<int>(result.Result, is BadRequest);
            //var exception = Assert.ThrowsAsync<ApiException>(() => controller.PostJob(request));

            //Assert.Equal<int>((int)HttpStatusCode.BadRequest, exception.Result.StatusCode);
        }
    }
}
