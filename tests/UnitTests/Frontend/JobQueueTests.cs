using Frontend.Engine;
using Frontend.Models;

namespace UnitTests.Frontend
{
    public class JobQueueTests
    {
        // TODO: update this with value read from config.
        private readonly int jobQueueLimit = 15;

        [Fact]
        public void JobQueue_EnqueueDequeue()
        {
            var jobQueue = new JobQueue();
            var job = UnitTestUtils.GetDummyJob();

            jobQueue.TryEnqueueJob(job);
            Assert.Equal(1, jobQueue.GetNumberOfJobs());

            var dequeuedJob = jobQueue.DequeueJob(CancellationToken.None);
            Assert.Equal(0, jobQueue.GetNumberOfJobs());

            Assert.Equal(job, dequeuedJob);
        }

        [Fact]
        public void JobQueue_Empty_DequeueShouldTimeout()
        {
            var jobQueue = new JobQueue();

            Assert.Equal(0, jobQueue.GetNumberOfJobs());

            // Use cancellation token source which will invoke cancel after 1000ms.
            var cts = new CancellationTokenSource();
            cts.CancelAfter(1000);

            Job? dequeuedJob = null;

            try
            {
                dequeuedJob = jobQueue.DequeueJob(cts.Token);
            }
            catch (OperationCanceledException)
            {
                // Do nothing here.
            }

            Assert.Null(dequeuedJob);
        }

        [Fact]
        public void JobQueue_Limit()
        {
            var jobQueue = new JobQueue();

            int numberOfJobs = jobQueueLimit;
            var jobs = UnitTestUtils.GetDummyJobs(numberOfJobs);

            foreach (var job in jobs)
            {
                var queueResult = jobQueue.TryEnqueueJob(job);
                Assert.True(queueResult, "Job should be added to the queue.");
            }

            Assert.Equal(jobQueueLimit, jobQueue.GetNumberOfJobs());

            // This one should fail as the queue is full.
            var limitJob = UnitTestUtils.GetDummyJob();
            var result = jobQueue.TryEnqueueJob(limitJob);

            Assert.False(result, "JobQueue should not accept more than jobQueueLimit jobs.");
            Assert.Equal(jobQueueLimit, jobQueue.GetNumberOfJobs());
        }
    }
}
