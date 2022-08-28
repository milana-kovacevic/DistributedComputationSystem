using Frontend.Managers;
using Frontend.Models;
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
            serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public void JobManager_Initialize()
        {
            var jobManager = serviceProvider.GetService<IJobManager>();

            int numberOfJobs = 5;
            var jobs = GetDummyJobs(numberOfJobs);
            jobManager.Initialize(jobs);

            Assert.Equal(numberOfJobs, jobManager.GetNumberOfJobs());
        }

        private static IEnumerable<Job> GetDummyJobs(int count)
        {
            var jobs = new List<Job>();

            for (int i = 0; i < count; i++)
            {
                jobs.Add(GetDummyJob(i, $"{i}"));
            }

            return jobs;
        }

        private static Job GetDummyJob(int id, string inputData)
        {
            return new Job()
            {
                Id = id,
                StartTime = DateTime.Now,
                State = JobState.Pending,
                Data = inputData
            };
        }
    }
}
