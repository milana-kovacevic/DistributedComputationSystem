namespace ComputeNode.Models
{
    public class AtomicJobResult
    {
        public int Id { get; set; }

        public int ParentJobId { get; set; }

        public string? Result { get; set; }

        public AtomicJobState State { get; set; }

        public string? Error { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }
    }
}
