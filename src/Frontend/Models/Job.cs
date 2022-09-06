using System.Text.Json.Serialization;

namespace Frontend.Models
{
    public class Job
    {
        public int Id { get; set; }

        public JobType JobType { get; set; }

        [JsonIgnore]
        public ICollection<AtomicJob> AtomicJobs { get; set; }

        public JobResult JobResult { get; set; }
        
        [JsonIgnore]
        public JobState State => JobResult.State;

        public Job()
        {
            JobResult = new JobResult()
            {
                State = JobState.Pending,
                StartTime = DateTime.UtcNow
            };
        }

        public Job(JobType jobType, ICollection<AtomicJob> atomicJobs)
        {
            this.JobType = jobType;
            this.AtomicJobs = atomicJobs;

            JobResult = new JobResult()
            {
                State = JobState.Pending,
                StartTime = DateTime.UtcNow
            };
        }
    }
}