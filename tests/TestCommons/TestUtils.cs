using System.Diagnostics;
using Xunit;

namespace TestCommons
{
    public class TestUtils
    {
        public static async Task PollUntilSatisfied<T>(
            T pollingObject,
            Func<T, bool> successCondition,
            TimeSpan timeout,
            TimeSpan pollingInterval)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (stopwatch.Elapsed < timeout && !successCondition(pollingObject))
            {
                await Task.Delay(pollingInterval);
            }

            if (!successCondition(pollingObject))
            {
                Assert.True(false, $"Timeout occurred. Polling failed for object '{pollingObject.ToString()}'.");
            }
        }
    }
}
