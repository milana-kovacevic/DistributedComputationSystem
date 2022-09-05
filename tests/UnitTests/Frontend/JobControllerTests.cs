﻿using Frontend.Controllers;
using Frontend.Exceptions;
using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Net;
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
            Assert.Equal(typeof(BadRequestObjectResult), result.Result.GetType());

            var badRequestResult = (BadRequestObjectResult)result.Result;
            Assert.Equal(ExceptionMessages.InputDataNotProvided, badRequestResult.Value);
        }
    }
}