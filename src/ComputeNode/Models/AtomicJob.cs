namespace ComputeNode.Models
{
    public class AtomicJob
    {
        public int Id { get; set; }

        public int ParentJobId { get; set; }

        public string? Data { get; set; }

        public AtomicJobType JobType { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public AtomicJobState State { get; set; }
    }
}
