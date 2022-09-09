using System.Text.Json.Serialization;

namespace ControlNode.Abstraction.Models
{
    public class Job
    {
        public int JobId { get; set; }

        public JobType JobType { get; set; }

        [JsonIgnore]
        public ICollection<AtomicJob> AtomicJobs { get; set; }

        [JsonIgnore]
        public JobResult JobResult { get; set; }
        
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

            this.JobResult = new JobResult()
            {
                State = JobState.Pending,
                StartTime = DateTime.UtcNow
            };
        }
    }
}