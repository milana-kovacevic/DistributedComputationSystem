namespace Frontend.Models
{
    public class Job
    {
        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public JobState State { get; set; }
    }
}