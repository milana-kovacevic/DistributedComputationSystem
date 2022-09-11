using ControlNode.DCS.Core.Managers;
using Microsoft.Extensions.DependencyInjection;
using TestCommons;
using Xunit;

namespace UnitTests.ControlNode
{
    public class JobManagerTests
    {
        private readonly ServiceProvider serviceProvider;

        public JobManagerTests()
        {
            var services = new ServiceCollection();
            TestBootstraper.ConfigureServices_Frontend(services);

            serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public void JobManager_Initialize()
        {
            var jobManager = serviceProvider.GetService<IJobManager>();

            int numberOfJobs = 5;
            var jobs = UnitTestUtils.GetDummyJobs(numberOfJobs);
            jobManager.Initialize(jobs);

            Assert.Equal(numberOfJobs, jobManager.GetNumberOfJobs());
        }
    }
}
