using DistributedCalculationSystem;
using System.Collections.ObjectModel;
using System.Net;
using Xunit;

namespace FunctionalTests
{
    public class EndToEndClusterTests
    {
        private const string baseUrl = "https://matf-distr-comp-sys.westeurope.cloudapp.azure.com/";
        DistributedCalculationSystemClient client = null;

        public EndToEndClusterTests()
        {
            this.client = new DistributedCalculationSystemClient(baseUrl, new HttpClient());
        }

        [Fact]
        public async void ListJobs_Success()
        {
            var jobs = await client.AllAsync();

            Console.WriteLine("Jobs:");
            foreach (var job in jobs)
            {
                Console.WriteLine(job.ToString());
            }

            Assert.NotEmpty(jobs);
        }

        [Fact]
        public async void RunJob_Success()
        {
            int i = 0;
            while (true)
            {
                var inputData = new Collection<AtomicJobRequestData>()
                {
                    new AtomicJobRequestData() { InputData =$"{i++}" }
                };

                var request = new JobRequestData()
                {
                    JobType = JobType.CalculateSumOfDigits,
                    InputData = inputData
                };

                var job = await client.CreateAsync(request);

                Assert.NotNull(job);

                await Task.Delay(30000);
            }
        }
    }
}