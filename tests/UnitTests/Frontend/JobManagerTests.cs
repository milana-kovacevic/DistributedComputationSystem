using Frontend.Engine;
using Frontend.Managers;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTests.Frontend
{
    public class JobManagerTests
    {
        private readonly ServiceProvider serviceProvider;

        public JobManagerTests()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IJobManager, JobManager>();
            services.AddSingleton<JobQueue>();
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
