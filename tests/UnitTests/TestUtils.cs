using Frontend.Models;
using System.Diagnostics;

namespace UnitTests
{
    internal class TestUtils
    {
        public static async Task PollUntilSatisfied<T>(T pollingObject, Func<T, bool> successCondition, TimeSpan timeout)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (stopwatch.Elapsed < timeout && !successCondition(pollingObject))
            {
                await Task.Delay(500);
            }

            if (!successCondition(pollingObject))
            {
                Assert.True(false, $"Polling failed for object '{pollingObject.ToString()}'.");
            }
        }

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
                Id = id,
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
