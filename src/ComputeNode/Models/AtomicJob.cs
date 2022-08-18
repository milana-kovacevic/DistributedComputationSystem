namespace ComputeNode.Models
{
    public class AtomicJob
    {
        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public AtomicJobState State { get; set; }
    }
}
