using ControlNode.Frontend.Models;

namespace UnitTests
{
    internal class UnitTestUtils
    {
        public static IEnumerable<Job> GetDummyJobs(int count)
        {
            var jobs = new List<Job>();

            for (int i = 0; i < count; i++)
            {
                jobs.Add(GetDummyJob(i, $"{i}"));
            }

            return jobs;
        }

        public static Job GetDummyJob(int id = 1, string inputData = "42", JobState state = JobState.Pending)
        {
            return new Job()
            {
                JobId = id,
                JobType = JobType.CalculateSumOfDigits,
                AtomicJobs =  new List<AtomicJob>()
                {
                    new AtomicJob 
                    {
                        JobId = id,
                        AtomicJobId = id,
                        InputData = inputData
                    }
                }
            };
        }
    }
}
